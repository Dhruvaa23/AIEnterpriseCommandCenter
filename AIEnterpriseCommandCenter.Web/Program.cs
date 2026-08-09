
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Infrastructure.Data;
using AIEnterpriseCommandCenter.Infrastructure.DependencyInjection;
using AIEnterpriseCommandCenter.Infrastructure.Identity;
using AIEnterpriseCommandCenter.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using QuestPDF.Infrastructure;
using AIEnterpriseCommandCenter.Infrastructure.AI;
using AIEnterpriseCommandCenter.Application.Services;

namespace AIEnterpriseCommandCenter.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Console.WriteLine("Environment: " + builder.Environment.EnvironmentName);

            Console.WriteLine("Connection String:");
            Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<IAssetRepository, AssetRepository>();
            builder.Services.AddScoped<IServiceDeskRepository, ServiceDeskRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddHttpClient<IAIService, GroqAIService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Groq:BaseUrl"]!);
            });
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddScoped<IAIToolsService, AIToolsService>();
            builder.Services.AddScoped<IEmployeeAITool, EmployeeAITool>();
            builder.Services.AddScoped<IAssetAITool, AssetAITool>();
            builder.Services.AddScoped<IAITicketAssistant, AITicketAssistant>();
            builder.Services.AddScoped<IServiceDeskAITool, ServiceDeskAITool>();
            builder.Services.AddScoped<IDashboardAITool, DashboardAITool>();
            builder.Services.AddScoped<IAIInsightsService, AIInsightsService>();


            QuestPDF.Settings.License = LicenseType.Community;
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DbInitializer.SeedAsync(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.UseSession();

            app.Run();
        }
    }
}
