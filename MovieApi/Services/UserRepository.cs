using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _ctx;

        public UserRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var users = await _ctx.Users.ToListAsync();
       
            return users.Count > 0 ? users : null;
        }
    }
}
