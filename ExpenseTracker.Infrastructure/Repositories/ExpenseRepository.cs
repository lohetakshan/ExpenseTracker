using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync(Guid UserId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == UserId)
                .Include(e => e.Category) // Include Category details
                .ToListAsync();
        }

        public async Task<Expense?> GetExpenseByIdAsync(Guid ExpenseId, Guid UserId)
        {
            return await _context.Expenses
                .Include(e => e.Category) // Include Category details
                .FirstOrDefaultAsync(e => e.ExpenseId == ExpenseId && e.UserId == UserId);
        }

        public async Task AddExpenseSync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateExpenseAsync(Expense expense)
        {
            var existingExpense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.ExpenseId == expense.ExpenseId && e.UserId == expense.UserId);
            if (existingExpense != null)
            {
                existingExpense.Title = expense.Title;
                existingExpense.Amount = expense.Amount;
                existingExpense.Date = expense.Date;
                existingExpense.Notes = expense.Notes;
                existingExpense.CategoryId = expense.CategoryId;
                await _context.SaveChangesAsync();
            }
        }
       
        public async Task DeleteExpenseAsync(Guid ExpenseId, Guid UserId)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.ExpenseId == ExpenseId && e.UserId == UserId);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }
    }
}