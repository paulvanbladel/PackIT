using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using PackIT.Api;

namespace PackIT.ScenarioTests
{
    public sealed class TestApplication : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder);
        }
    }
}