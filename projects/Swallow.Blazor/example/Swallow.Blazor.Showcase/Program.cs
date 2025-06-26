using Swallow.Blazor.Showcase;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents();
var app = builder.Build();
app.UseAntiforgery();
app.MapRazorComponents<App>();
app.Run();
