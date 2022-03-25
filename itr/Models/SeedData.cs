using itr.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itr.Models
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new itrContext(serviceProvider.GetRequiredService<DbContextOptions<itrContext>>()))
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
