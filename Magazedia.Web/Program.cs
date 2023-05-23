using Magazedia;
using Magazedia.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Gives better integration with the systemd service on Linux
builder.Host.UseSystemd();

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

	var SupportedCultures = new[]
	{
		new CultureInfo(DefaultCulture),
		new CultureInfo("ja"),
		new CultureInfo("ar"),
		new CultureInfo("xx-test")
	};

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
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
				<rule enabled=""true"">
					<match url=""(.*)"" />
					<conditions logicalGrouping=""MatchAll"" trackAllCaptures=""false"">
						<add input=""{{HTTP_HOST}}"" pattern=""^en\.magazedia\.site$|^xx-test\.magazedia\.site$|^ja\.magazedia\.site$|^ar\.magazedia\.site$|^en\.localhost|^xx-test\.localhost|^ja\.localhost|^ar\.localhost"" negate=""true"" />
					</conditions>
					<action type=""Redirect"" url=""https://en.magazedia.site/{{R:1}}"" redirectType=""308"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^dmca:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Dmca"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^create-article:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Create"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/edit"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article/Edit?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/talk$"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Talk?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/talk/(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/TalkSubject?ArticleUrlSlug={{R:1}}&amp;ArticleTalkSubjectUrlSlug={{R:2}}"" />
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
					<match url=""^(?!Create)(?!Dmca)(?!Article/History)(?!Article/Firehose)(?!Article/Edit)(?!Talk)(?!Article/View)(?!DbHelper)(?!TalkSubject)(?!Identity\/)(?!$)(?!.*\.(?:jpg|jpeg|gif|png|bmp|css|js|ico|txt)$)(.*)"" />
					<action type=""Rewrite"" url=""Article/View?UrlSlug={{R:1}}"" appendQueryString=""true"" />
				</rule>
			</rules>
		</rewrite>
	"))
{
	var options = new RewriteOptions()
			.AddIISUrlRewrite(sr);
			//.AddRewrite(@"^(?!Create)(?!History)(?!Firehose)(?!Edit)(?!Talk)(?!Article)(?!DbHelper)(?!TalkSubject)(?!Identity\/)(?!$)(?!.*\.(?:jpg|jpeg|gif|png|bmp|css|js)$)(.*)", "Article?UrlSlug=$1",
			//			skipRemainingRules: true);

	app.UseRewriter(options);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapRazorPages();

app.Logger.LogInformation("Information - Hello World");
app.Logger.LogWarning("Warning - Hello World");
app.Logger.LogError("Error - Hello World");
app.Logger.LogCritical("Critical - Hello World");

app.Run();
