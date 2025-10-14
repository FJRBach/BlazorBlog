using Blazored.LocalStorage;
using ClienteBlogWASM;
using ClienteBlogWASM.Servicios;
using ClienteBlogWASM.Servicios.IServicio;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IPostsServicio, PostsServicio>();
builder.Services.AddScoped<IServicioAutenticacion, ServicioAutenticacion>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
//Add to auth
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthStateProvider>());


// Url de la API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5269/") });
await builder.Build().RunAsync();
