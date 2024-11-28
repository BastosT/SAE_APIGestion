using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;
using SAE_CLIENTGestion.ViewModels;
using TD1Client.Services;

namespace SAE_CLIENTGestion
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped<IBabylonJSService, BabylonJSService>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7155/") });

            builder.Services.AddScoped<IService<Batiment>, WSServiceBatiment>();
            builder.Services.AddScoped<IService<Salle>, WSServiceSalle>();
            builder.Services.AddScoped<IService<Capteur>, WSServiceCapteur>();
            builder.Services.AddScoped<IService<Equipement>, WSServiceEquipement>();
            builder.Services.AddScoped<IService<Mur>, WSServiceMur>();

            builder.Services.AddScoped<BatimentsViewModel>();
            builder.Services.AddScoped<SallesViewModel>();

            builder.Services.AddScoped<IBabylonJSService, BabylonJSService>();

            builder.Services.AddBlazorBootstrap();

            await builder.Build().RunAsync();
        }
    }
}
