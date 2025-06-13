using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using QOC.Api.Helpers;
using QOC.Application.Interfaces;
using QOC.Domain.Entities;
using QOC.Infrastructure.Contracts;
using QOC.Infrastructure.Mapping;
using QOC.Infrastructure.Persistence;
using QOC.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
var corsSettings = builder.Configuration.GetSection("Cors");
var policyName = corsSettings["PolicyName"];
var origins = corsSettings.GetSection("Origins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(policyName, policy =>
    {
        policy.WithOrigins(origins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "ar" };
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});
builder.Services.AddMemoryCache();

// Add Identity & JWT
builder.Services.AddIdentityInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAboutUsService, AboutUsService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IProjectCategoryService, ProjectCategoryService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializer.SeedRolesAndAdmin(userManager, roleManager);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = ""
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseRequestLocalization();
app.UseCors("AllowAngularClient");
app.UseRequestLocalization();
app.UseAuthentication();
app.MapControllers();

app.Run();
  