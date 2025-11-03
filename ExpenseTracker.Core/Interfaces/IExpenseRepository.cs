using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Entities;

namespace ExpenseTracker.Core.Interfaces
{
    public interface IExpenseRepository
    {
        //Fetch all expenses for a specific user.
        Task<IEnumerable<Expense>> GetAllExpensesAsync(Guid UserId);

        //Fetch a specific expense for a specific user
        Task<Expense?> GetExpenseByIdAsync(Guid ExpenseId, Guid UserId);

        //Add a new expense
        Task AddExpenseSync(Expense expense);

        //Update an existing expense (parameter has UserID)
        Task UpdateExpenseAsync(Expense expense);

        //Delete an expense for a specific user
        Task DeleteExpenseAsync(Guid ExpenseId, Guid UserId);
    }
}
