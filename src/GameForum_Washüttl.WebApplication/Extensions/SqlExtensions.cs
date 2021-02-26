using GameForum_Washüttl.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameForum_Washüttl.WebApplication.Extensions
{
    public static class SqlExtensions
    {
        public static void ConfigureSql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<GameForumDBContext>(optionsBuilder =>
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlite(connectionString);
                }
            });
        }
    }
}
