namespace ExpenseTracker.API.DTOs
{
    public class ExpenseDTO
    {
        public Guid ExpenseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }

        //UserId should be added only when required
        //public Guid UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
    }
}
