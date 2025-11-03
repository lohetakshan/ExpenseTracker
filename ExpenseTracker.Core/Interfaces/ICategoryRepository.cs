using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Entities;

namespace ExpenseTracker.Core.Interfaces
{
    public interface ICategoryRepository
    {   
        //Fetch all categories (default + user-defined)
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        //Fetch categories created by a specific user
        Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(Guid userId);

        //Fetch a specific category by ID
        Task<Category?> GetCategoryByIdAsync(Guid categoryId);

        //Add a new category
        Task AddCategoryAsync(Category category);

        //Update an existing category
        Task UpdateCategoryAsync(Category category);

        //Delete a category
        Task DeleteCategoryAsync(Guid categoryId);

        //Supporting methods for secure update/delete logic

        //Check if a category exists
        Task<bool> CategoryExistsAsync(Guid categoryId);

        // Check if a category belongs to a specific user
        Task<bool> CategoryBelongsToUserAsync(Guid categoryId, Guid userId);

        //Check for duplicate category name for a user
        Task<bool> CategoryNameExistsAsync(string name, Guid userId);

        //Get default categories only
        Task<IEnumerable<Category>> GetDefaultCategoriesAsync();

        //Get user-defined categories only
        Task<IEnumerable<Category>> GetUserDefinedCategoriesAsync(Guid userId);
    }
}
