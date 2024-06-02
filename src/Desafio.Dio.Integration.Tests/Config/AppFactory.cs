using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Desafio.Dio.Integration.Tests.Config
{
    public class AppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.UseStartup<TProgram>();
        //    //builder.UseEnvironment("Testing");
        //}
    }
}