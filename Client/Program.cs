using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Mediwatch.Client.services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.JSInterop;
using Blazored.LocalStorage;

namespace Mediwatch.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services
				  .AddBlazorise(options =>
				 {
					 options.ChangeTextOnKeyPress = true;
				 })
				  .AddBootstrapProviders()
				  .AddFontAwesomeIcons();

			builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddLocalization(options => options.ResourcesPath = "Ressources");

			builder.Services.AddOptions();
			builder.Services.AddAuthorizationCore();
			builder.Services.AddScoped<MediwatchAuthentifiacationProvider>();
			builder.Services.AddScoped<AuthenticationStateProvider>(serviceProvider => serviceProvider.GetRequiredService<MediwatchAuthentifiacationProvider>());

			builder.Services.AddBlazoredLocalStorage();

			var host = builder.Build();

			var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
			var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");

			if (result != null) {
				var culture = new CultureInfo(result);
				CultureInfo.DefaultThreadCurrentCulture = culture;
				CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
			
			host.Services
			  .UseBootstrapProviders()
			  .UseFontAwesomeIcons();

			await host.RunAsync();
		}
	}
}
