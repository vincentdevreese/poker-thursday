using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using PokerThursday;

using PokerThursdayBlazorApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

InMemoryDebtRegister debtRegister = new();
debtRegister.Save(new DebtRegister([new Debt("a", "b", 100)]));

builder.Services.AddSingleton<IInMemoryDebtRegister>(debtRegister);

builder.Services.AddSingleton<AddDebt>();
builder.Services.AddSingleton<PayDebt>();

await builder.Build().RunAsync();
