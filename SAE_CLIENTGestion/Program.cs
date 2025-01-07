using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Models.DTO;
using SAE_CLIENTGestion.Services;
using SAE_CLIENTGestion.Services.DTO;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7155/") });

            // Services web
            builder.Services.AddScoped<IService<Batiment>, WSServiceBatiment>();
            builder.Services.AddScoped<IService<Salle>, WSServiceSalle>();
            builder.Services.AddScoped<IService<TypeSalle>, WSServiceTypeSalle>();
            builder.Services.AddScoped<IService<TypeEquipement>, WSServiceTypeEquipement>();
            builder.Services.AddScoped<IService<Capteur>, WSServiceCapteur>();
            builder.Services.AddScoped<IService<Equipement>, WSServiceEquipement>();
            builder.Services.AddScoped<IService<Mur>, WSServiceMur>();

            builder.Services.AddScoped<IService<SalleDTO>, WSServiceSalleDTO>();
            builder.Services.AddScoped<IService<TypeSalleDTO>, WSServiceTypeSalleDTO>();
            builder.Services.AddScoped<IService<BatimentDTO>, WSServiceBatimentDTO>();
            builder.Services.AddScoped<IService<EquipementDTO>, WSServiceEquipementDTO>();
            builder.Services.AddScoped<IService<CapteurDTO>, WSServiceCapteurDTO>();

            builder.Services.AddScoped<PredictionService>();

            // ViewModels
            builder.Services.AddScoped<BatimentsViewModel>();
            builder.Services.AddScoped<SallesViewModel>();
            builder.Services.AddScoped<PredictionViewModel>();


            // BabylonJS service (seulement une fois, en singleton)
            builder.Services.AddSingleton<IBabylonJSService>(serviceProvider =>
                BabylonJSService.Initialize(
                    serviceProvider.GetRequiredService<IJSRuntime>(),
                    serviceProvider // Passez le service provider au lieu du service directement
                )
            );

            builder.Services.AddBlazorBootstrap();
            await builder.Build().RunAsync();
        }
    }
}