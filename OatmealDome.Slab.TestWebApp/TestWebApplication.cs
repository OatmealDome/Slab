using OatmealDome.Slab.TestWebApp.Components;
using OatmealDome.Slab.Web;

namespace OatmealDome.Slab.TestWebApp;

public class TestWebApplication : SlabWebApplication
{
    protected override void BuildApplication(ISlabApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
    }

    protected override void SetupApplication(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
    }
}
