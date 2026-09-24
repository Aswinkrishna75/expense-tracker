namespace ExpenseTracker.Models;

public record CategoryTotal(string Category, decimal Total);

public class IndexViewModel
{
    public List<Expense> Expenses { get; set; } = new();
    public List<CategoryTotal> ByCategory { get; set; } = new();
    public decimal Total { get; set; }
    public decimal ThisMonth { get; set; }
    public string? SelectedCategory { get; set; }
}
