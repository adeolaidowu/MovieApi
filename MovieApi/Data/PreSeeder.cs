using MovieApi.Data;
using MovieApi.Models;
using System.Linq;
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
                    await ctx.Genres.AddAsync(new Genre { Name = type });
                    await ctx.SaveChangesAsync();
                }
            }
        }
    }
}
