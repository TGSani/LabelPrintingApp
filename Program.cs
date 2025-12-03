using LabelPrintingApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// DI setup
builder.Services.AddSingleton<LabelGenerator>();
builder.Services.AddSingleton<BatchLabelGenerator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
