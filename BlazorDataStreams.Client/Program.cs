using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorDataStreams.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddScoped<BlazorDataStreams.Shared.IMessageService, BlazorDataStreams.Shared.MessageService>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<BlazorDataStreams.Shared.IDataService, BlazorDataStreams.Shared.DataService>();
            
            await builder.Build().RunAsync();
        }
    }
}
