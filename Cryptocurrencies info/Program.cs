var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database settings
builder.Configuration.AddJsonFile("dbsettings.json");

// Add additional services
builder.Services.AddServices();

var app = builder.Build();

// Getting service and run async
CoinGecko coinGecko = app.Services.GetService<CoinGecko>() ?? throw new SystemException();
Task parser = Task.Run(coinGecko.FindMarkets);

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
