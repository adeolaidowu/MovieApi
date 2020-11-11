using MovieApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
    }
}
