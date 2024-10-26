using AusSemClient_1_.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddScoped<PropertyService>();
//builder.Services.AddScoped<ParcelService>();
//builder.Services.AddScoped<HomeService>();
builder.Services.AddSingleton<PropertyService>();
builder.Services.AddSingleton<ParcelService>();
builder.Services.AddSingleton<HomeService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
 