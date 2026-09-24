using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public class Expense
{
    public static readonly string[] Categories =
        { "Food", "Transport", "Bills", "Shopping", "Health", "Entertainment", "Other" };

    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = "";

    [Range(0.01, 10000000, ErrorMessage = "Enter an amount greater than 0.")]
    public decimal Amount { get; set; }

    [Required]
    public string Category { get; set; } = "Other";

    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [StringLength(300)]
    public string? Note { get; set; }
}
