using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SAE_CLIENTGestion.Models;
using SAE_CLIENTGestion.Services;
using SAE_CLIENTGestion.ViewModels;

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
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<IService<Equipement>, WSServiceEquipement>();

            builder.Services.AddScoped<EquipementsViewModel>();

            builder.Services.AddScoped<IBabylonJSService, BabylonJSService>();

            builder.Services.AddBlazorBootstrap();

            await builder.Build().RunAsync();
        }
    }
}
