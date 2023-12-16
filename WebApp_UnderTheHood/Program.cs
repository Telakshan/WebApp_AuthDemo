using Microsoft.AspNetCore.Authorization;
using WebApp_AuthDemo.Authorization;
using WebApp_AuthDemo.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Get cookie name from appsettings
IConfigurationSection configurationSection = builder.Configuration.GetSection(nameof(SessionSettings));

//Add SessionSettings to services so that we can use it anywhere
builder.Services.Configure<SessionSettings>(configurationSection);

var sessionCookie = configurationSection.Get<SessionSettings>()!.SessionCookie;

builder.Services.AddAuthentication(sessionCookie).AddCookie(sessionCookie, options =>
{
    options.Cookie.Name = sessionCookie;
    options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
    options.AccessDeniedPath = "/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
    options.AddPolicy("MustBelongToHRDepartment", policy => policy.RequireClaim("Department", "HR"));
    options.AddPolicy("HRManager", policy => policy
    .RequireClaim("Department", "HR")
    .RequireClaim("Manager")
    .Requirements.Add(new HRManagerProbationRequirement(3)));
});

builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementhandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); 

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();