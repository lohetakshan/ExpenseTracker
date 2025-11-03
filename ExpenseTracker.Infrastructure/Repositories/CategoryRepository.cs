using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(Guid userId)
    {
        return await _context.Categories
            .Where(c => c.CreatedByUserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }

    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category is not null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CategoryExistsAsync(Guid categoryId)
    {
        return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
    }

    public async Task<bool> CategoryBelongsToUserAsync(Guid categoryId, Guid userId)
    {
        return await _context.Categories
            .AnyAsync(c => c.CategoryId == categoryId && c.CreatedByUserId == userId);
    }

    public async Task<bool> CategoryNameExistsAsync(string name, Guid userId)
    {
        return await _context.Categories
            .AnyAsync(c => c.Name == name && c.CreatedByUserId == userId);
    }

    public async Task<IEnumerable<Category>> GetDefaultCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.IsDefault)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetUserDefinedCategoriesAsync(Guid userId)
    {
        return await _context.Categories
            .Where(c => !c.IsDefault && c.CreatedByUserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}