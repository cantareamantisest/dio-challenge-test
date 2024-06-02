using Desafio.Dio.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Dio.Identity.Data
{
    public class IdentityDataContext : IdentityDbContext<CustomUser>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
            : base(options)
        {

        }
    }
}