using Desafio.Dio.Application.Interfaces;
using Desafio.Dio.Application.Services;
using Desafio.Dio.Core.Interfaces;
using Desafio.Dio.Identity.Data;
using Desafio.Dio.Identity.Models;
using Desafio.Dio.Identity.Services;
using Desafio.Dio.Repository.Context;
using Desafio.Dio.Repository.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Desafio.Dio.IoC
{
    public static class NativeInjectorDependency
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDbContext<DesafioContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IdentityDataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<CustomUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddTransient<IIdentityService, IdentityService>();

            services.AddScoped<DesafioContext>();
        }
    }
}