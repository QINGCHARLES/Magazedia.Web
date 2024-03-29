using Magazedia;
using Magazedia.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Gives better integration with the systemd service on Linux
builder.Host.UseSystemd();

builder.Services.AddResponseCompression(options =>
{
	options.MimeTypes =
	ResponseCompressionDefaults.MimeTypes.Concat(
		new[]
		{
			"text/css",
			"application/javascript",
			"application/manifest+json"
		}
	);

	options.EnableForHttps = true;
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
	options.Level = CompressionLevel.SmallestSize;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
	options.Level = CompressionLevel.SmallestSize;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages().AddViewLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    const string DefaultCulture = "en";

    // Support all cultures
    List<CultureInfo> SupportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)// & ~CultureTypes.NeutralCultures)
                    .Where(Culture => !String.IsNullOrEmpty(Culture.Name))
                    .ToList();

    SupportedCultures.Add(new CultureInfo("xx-test"));

    //var SupportedCultures = new[]
    //{
    //	new CultureInfo(DefaultCulture),
    //	new CultureInfo("ja"),
    //	new CultureInfo("ar"),
    //	new CultureInfo("xx-test")
    //};

    options.DefaultRequestCulture = new RequestCulture(DefaultCulture);
    options.SupportedCultures = SupportedCultures;
    options.SupportedUICultures = SupportedCultures;

    // Remove the QueryStringRequestCultureProvider, CookieRequestCultureProvider, and AcceptLanguageHeaderRequestCultureProvider
    // as we force site culture based on hostname
    options.RequestCultureProviders.Clear();

    // Add a custom RequestCultureProvider that extracts the culture from the subdomain of the site hostname
    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async Context =>
    {
        string Hostname = Context.Request.Host.Host;
        string Culture = Helpers.GetCultureFromHostname(Hostname, DefaultCulture);

        return await Task.FromResult(new ProviderCultureResult(Culture));
    }));
});

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();

	app.UseStaticFiles(new StaticFileOptions
	{
		ContentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider
		{
			Mappings = { [".avif"] = "image/avif" }
		}
	});
}
else
{
	app.UseStaticFiles(new StaticFileOptions
	{
		ContentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider
        {
			Mappings = { [".avif"] = "image/avif" }
		},
		OnPrepareResponse = ctx =>
		{
			const string headerValue = "public,max-age=" + "604800"; // 1 week
			ctx.Context.Response.Headers[HeaderNames.CacheControl] = headerValue;
		}
	});

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (TextReader sr = new StringReader(@$"
		<rewrite>
			<rules>
				<clear />
				<rule enabled=""true"" stopProcessing=""true"">
					<match url=""(.*)"" />
					<conditions logicalGrouping=""MatchAll"" trackAllCaptures=""false"">
						<add input=""{{HTTPS}}"" pattern=""^OFF$"" />
					</conditions>
					<action type=""Redirect"" url=""https://{{HTTP_HOST}}{{REQUEST_URI}}"" appendQueryString=""false"" redirectType=""308"" />
				</rule>
				<rule enabled=""true"" stopProcessing=""true"">
					<match url=""^info:sitemap$"" />
					<action type=""Rewrite"" url=""Sitemap"" />
				</rule>
				<rule enabled=""true"" stopProcessing=""true"">
					<match url=""^$"" />
					<conditions logicalGrouping=""MatchAll"" trackAllCaptures=""false"">
						<add input=""{{HTTP_HOST}}"" pattern=""^magazedia\.wiki$|^localhost(:[0-9]+)?$"" />
					</conditions>
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/CultureSelect"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.*)"" />
					<conditions logicalGrouping=""MatchAll"" trackAllCaptures=""false"">
						<add input=""{{HTTP_HOST}}"" pattern=""^magazedia\.wiki$|^en\.magazedia\.wiki$|^xx-test\.magazedia\.wiki$|^ja\.magazedia\.wiki$|^ar\.magazedia\.wiki$|^localhost|^en\.localhost|^xx-test\.localhost|^ja\.localhost|^ar\.localhost"" negate=""true"" />
					</conditions>
					<action type=""Redirect"" url=""https://en.magazedia.wiki/{{R:1}}"" redirectType=""308"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^login:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Identity/Account/Login"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^register:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Identity/Account/Register"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^dmca:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Dmca"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^privacy-policy:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Privacy"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^terms:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Terms"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^file:(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/View?UrlSlug=file:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^image:(.+)/edit"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Edit?UrlSlug=image:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^image:(.+)/history$"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/History?UrlSlug=image:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^image:(.+)/talk$"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Talk?UrlSlug=image:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^image:(.+)/revision/(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/View?UrlSlug=image:{{R:1}}&amp;Id={{R:2}}"" appendQueryString=""true"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^image:(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/View?UrlSlug=image:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^404:(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/NotFound?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/edit"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Edit?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^category:(.+)/edit"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Edit?UrlSlug=category:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^category:(.+)/history$"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/History?UrlSlug=category:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^category:(.+)/talk$"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Talk?UrlSlug=category:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^category:(.+)/revision/(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/View?UrlSlug=category:{{R:1}}&amp;Id={{R:2}}"" appendQueryString=""true"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^category:(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/View?UrlSlug=category:{{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^create:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Create"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/talk$"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Talk?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/talk/(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/TalkSubject?ArticleUrlSlug={{R:1}}&amp;ArticleTalkSubjectUrlSlug={{R:2}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^firehose:(.*)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Firehose?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/history$"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/History?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/revision/(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/View?UrlSlug={{R:1}}&amp;Id={{R:2}}"" appendQueryString=""true"" />
				</rule>
				<rule name=""Rewrite Rule"">
					<match url=""^(?!Privacy)(?!Terms)(?!NotFound)(?!Article/Create)(?!Dmca)(?!Sitemap)(?!dev/CoverList)(?!Article/History)(?!Article/Firehose)(?!Article/Edit)(?!Article/Talk)(?!Article/View)(?!DbHelper)(?!Article/TalkSubject)(?!Identity\/)(?!$)(?!.*\.(?:jpg|jpeg|gif|png|webp|avif|css|js|ico|txt|webmanifest)$)(.*)"" />
					<action type=""Rewrite"" url=""Article/View?UrlSlug={{R:1}}"" appendQueryString=""true"" />
				</rule>
			</rules>
		</rewrite>
	"))
{
    var options = new RewriteOptions()
            .AddIISUrlRewrite(sr);

    app.UseRewriter(options);
}



app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapRazorPages();

app.Logger.LogInformation("Information - Hello World");
app.Logger.LogWarning("Warning - Hello World");
app.Logger.LogError("Error - Hello World");
app.Logger.LogCritical("Critical - Hello World");

app.Run();
