using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NaradX.Web.Services.Implementations.Common;
using NaradX.Web.Services.Implementations.Contact;
using NaradX.Web.Services.Implementations.Security;
using NaradX.Web.Services.Interfaces.Common;
using NaradX.Web.Services.Interfaces.Contact;
using NaradX.Web.Services.Interfaces.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Configure Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configure Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "NaradX";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async context =>
            {
                var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                await tokenService.ValidateTokenAsync(context);
            }
        };
    });

// Add Authorization with policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy =>
        policy.RequireRole("SuperAdmin"));

    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin", "SuperAdmin"));

    options.AddPolicy("User", policy =>
        policy.RequireRole("User", "Admin", "SuperAdmin"));
});

// Register services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IApiHelper, ApiHelper>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IIpAddressService, IpAddressService>();
builder.Services.AddScoped<IConfigValueService, ConfigValueService>();
builder.Services.AddScoped<IContactService, ContactService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// Add security headers using dedicated package
//app.UseSecurityHeaders(new HeaderPolicyCollection()
//    .AddFrameOptionsDeny()
//    .AddXssProtectionBlock()
//    .AddContentTypeOptionsNoSniff()
//    .AddReferrerPolicyStrictOriginWhenCrossOrigin()
//    .AddContentSecurityPolicy(builder =>
//    {
//        builder.AddObjectSrc().None(); // No embedded objects
//        builder.AddFormAction().Self(); // Forms can only submit to same origin
//        builder.AddFrameAncestors().None(); // No framing
//        builder.AddDefaultSrc().Self(); // Default to same origin
//        builder.AddScriptSrc().Self(); // Only allow scripts from same origin
//        builder.AddStyleSrc().Self(); // Only allow CSS from same origin
//    })
//    .AddPermissionsPolicy(builder =>
//    {
//        builder.AddAccelerometer().None();
//        builder.AddCamera().None();
//        builder.AddGeolocation().None();
//        builder.AddMicrophone().None();
//        builder.AddPayment().None();
//    }));

app.Use(async (context, next) =>
{
    // Only check for authenticated users
    if (context.User.Identity.IsAuthenticated)
    {
        // Example: check for a required session key
        if (string.IsNullOrEmpty(context.Session.GetString("authToken")))
        {
            // Session expired, sign out and redirect to login
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/Auth/Login");
            return;
        }
    }
    await next();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
