using Cryptocurrencies_info.Services;
using Cryptocurrencies_info.Services.CryptoCurrencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database settings
builder.Configuration.AddJsonFile("Properties/dbsettings.json");

// Add additional services
builder.Services.AddServices();

// Getting service and run async
builder.Services.AddHostedService<CoinGecko>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
