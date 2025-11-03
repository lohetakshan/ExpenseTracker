using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
       private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        //Controller Actions

        //Get: api/Expense/{id}
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetAllExpenses([FromQuery] Guid UserId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (UserId.ToString() != loggedInUserId && !isAdmin)
                return Forbid();

            var expenses = await _expenseRepository.GetAllExpensesAsync(UserId);
            var expenseDTOs = expenses.Select(MapToDTO).ToList();
            return Ok(expenseDTOs);
        }

        [Authorize]
        [HttpGet("{ExpenseId}")]
        public async Task<ActionResult<ExpenseDTO>> GetAllExpensesById(Guid ExpenseId, [FromQuery] Guid UserId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (UserId.ToString() != loggedInUserId && !isAdmin)
                return Forbid();

            var expenses = await _expenseRepository.GetExpenseByIdAsync(ExpenseId, UserId);
            if (expenses is null)
                return NotFound();
            return Ok(MapToDTO(expenses));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ExpenseDTO>> AddExpense([FromBody] CreateExpenseRequest request, [FromQuery] Guid UserId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (UserId.ToString() != loggedInUserId && !isAdmin)
                return Forbid();

            var expense = new Expense
            {
                ExpenseId = Guid.NewGuid(),
                Title = request.Title,
                Amount = request.Amount,
                Date = request.Date,
                Notes = request.Notes,
                UserId = UserId,
                CategoryId = request.CategoryId
            };

            await _expenseRepository.AddExpenseSync(expense);
            return CreatedAtAction(nameof(GetAllExpensesById), new { ExpenseId = expense.ExpenseId, UserId }, MapToDTO(expense));
        }

        [Authorize]
        [HttpPut("{ExpenseId}")]
        public async Task<IActionResult> UpdateExpense(Guid ExpenseId, [FromBody] CreateExpenseRequest request, [FromQuery] Guid UserId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (UserId.ToString() != loggedInUserId && !isAdmin)
                return Forbid();

            var existing = await _expenseRepository.GetExpenseByIdAsync(ExpenseId, UserId);
            if (existing is null)
                return NotFound();

            existing.Title = request.Title;
            existing.Amount = request.Amount;
            existing.Date = request.Date;
            existing.Notes = request.Notes;
            existing.UserId = UserId;
            existing.CategoryId = request.CategoryId;
            await _expenseRepository.UpdateExpenseAsync(existing);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{ExpenseId}")]
        public async Task<IActionResult> DeleteExpense(Guid ExpenseId, [FromQuery] Guid UserId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (UserId.ToString() != loggedInUserId && !isAdmin)
                return Forbid();

            var existing = await _expenseRepository.GetExpenseByIdAsync(ExpenseId, UserId);
            if (existing is null)
                return NotFound();
            await _expenseRepository.DeleteExpenseAsync(ExpenseId, UserId);
            return NoContent();
        }


        //Manual Mapping
        private ExpenseDTO MapToDTO(Expense expense)
        {

            return new ExpenseDTO
            {
                ExpenseId = expense.ExpenseId,
                Title = expense.Title,
                Amount = expense.Amount,
                Date = expense.Date,
                Notes = expense.Notes,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category?.Name
            };
        }

    }
}
