using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Entities;

namespace ExpenseTracker.Core.Interfaces
{
    public interface IUserRepository
    {
        //Fetch a user
        Task<User?> GetUserByIdAsync(Guid UserId);

        //Fetch a user by username
        Task<User?> GetUserByUsernameAsync(string Username);

        //Fetch a user by email
        Task<User?> GetUserByEmailAsync(string Email);

        //Add a new user
        Task AddUserAsync(User user);

        //Update an existing user
        Task UpdateUserAsync(User user);

        //Delete a user
        Task DeleteUserAsync(Guid UserId);

        //Check if a user exists
        //Secure Authentication Flow
        //Prevent Duplicate Users
        Task<bool> UserExistsAsync(Guid UserId);

        User ValidateUser(string UserName, string Password);
    }
}