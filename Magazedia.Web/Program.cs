using Magazedia.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

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
						<add input=""{{HTTP_HOST}}"" pattern=""^en\.magazedia\.site$|^ja\.magazedia\.site$|^en\.localhost|^ja\.localhost"" negate=""true"" />
					</conditions>
					<action type=""Redirect"" url=""https://en.magazedia.site/{{R:1}}"" redirectType=""308"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^create-article:"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Create"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/edit"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Edit?UrlSlug={{R:1}}"" />
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
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Firehose?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/history$"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/History?UrlSlug={{R:1}}"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.+)/revision/(.+)"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/Article?UrlSlug={{R:1}}&amp;Id={{R:2}}"" />
				</rule>
			</rules>
		</rewrite>
	"))
{
	var options = new RewriteOptions()
			.AddIISUrlRewrite(sr)
			.AddRewrite(@"^(?!Create)(?!History)(?!Firehose)(?!Edit)(?!Talk)(?!Article)(?!DbHelper)(?!TalkSubject)(?!Identity\/)(?!$)(?!.*\.(?:jpg|jpeg|gif|png|bmp|css|js)$)(.*)", "Article?UrlSlug=$1",
						skipRemainingRules: true);

	app.UseRewriter(options);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
