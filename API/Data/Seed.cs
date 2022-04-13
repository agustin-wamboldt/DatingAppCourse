using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach(var user in users)
            {
                using var hmac = new HMACSHA512();
                
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("passworderino")); // Seed data for developing, don't care about security of fake users
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user); // Not pushed to the DB yet, only tracked the user in EF.
            }

            await context.SaveChangesAsync();
        }
    }
}
