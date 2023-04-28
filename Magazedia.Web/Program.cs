using Magazedia.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
                <action type=""Redirect"" url=""https://magazedia.site{{REQUEST_URI}}"" appendQueryString=""false"" redirectType=""308"" />
            </rule>
            <rule enabled=""true"">
                <match url=""(.*)"" />
                <conditions logicalGrouping=""MatchAll"" trackAllCaptures=""false"">
                    <add input=""{{HTTP_HOST}}"" pattern=""^magazedia\.site$|^ja.magazedia.site$"" negate=""true"" />
                </conditions>
                <action type=""Redirect"" url=""https://magazedia.site/{{R:1}}"" redirectType=""308"" />
            </rule>
        </rules>
    </rewrite>
"))
{
        var options = new RewriteOptions()
                .AddIISUrlRewrite(sr)
                .AddRewrite(@"^(?!.*\.(?:jpg|jpeg|gif|png|bmp|css|js)$)(.*)", "Article?UrlSlug=$1", skipRemainingRules: true);
//              .AddRewrite(@"^(.*)", "Article?title=$1", skipRemainingRules: true);
		app.UseRewriter(options);
}


//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
