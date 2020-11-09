
using Microsoft.AspNetCore.Identity;
using MovieApi.Data;
using MovieApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groundforce.Services.Data
{
    public static class PreSeeder
    {
        public static async Task Seeder(AppDbContext ctx)
        {
            ctx.Database.EnsureCreated();

            string[] Genres = {"Romance","Drama", "Action",
            "Fantasy","Adventure" };
            if (!ctx.Genres.Any())
            {
                foreach (var type in Genres)
                {
                    ctx.Genres.Add(new Genre { Name = type });
                    ctx.SaveChanges();
                }
            }
        }
    }
}
