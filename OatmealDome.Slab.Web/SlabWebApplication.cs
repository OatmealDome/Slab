using Microsoft.AspNetCore.Builder;

namespace OatmealDome.Slab.Web;

public abstract class SlabWebApplication : SlabApplication<WebApplicationBuilder, WebApplication>
{
    protected override WebApplicationBuilder CreateBuilder(string[]? args) => WebApplication.CreateBuilder();

    protected override WebApplication CreateHost(WebApplicationBuilder builder) => builder.Build();
}
