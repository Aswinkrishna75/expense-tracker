using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers;

public class ExpensesController : Controller
{
    private readonly AppDbContext _db;

    public ExpensesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? category)
    {
        // SQLite can't SUM decimals in SQL, so we load and total in memory (fine for a small app)
        var all = await _db.Expenses.AsNoTracking().ToListAsync();
        var shown = string.IsNullOrEmpty(category)
            ? all
            : all.Where(e => e.Category == category).ToList();

        var today = DateTime.Today;

        var vm = new IndexViewModel
        {
            SelectedCategory = category,
            Expenses = shown.OrderByDescending(e => e.Date).ThenByDescending(e => e.Id).ToList(),
            Total = shown.Sum(e => e.Amount),
            ThisMonth = all.Where(e => e.Date.Year == today.Year && e.Date.Month == today.Month)
                           .Sum(e => e.Amount),
            ByCategory = all.GroupBy(e => e.Category)
                            .Select(g => new CategoryTotal(g.Key, g.Sum(x => x.Amount)))
                            .OrderByDescending(c => c.Total)
                            .ToList()
        };
        return View(vm);
    }

    public IActionResult Create() => View(new Expense());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Expense expense)
    {
        if (!ModelState.IsValid) return View(expense);
        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var expense = await _db.Expenses.FindAsync(id);
        return expense == null ? NotFound() : View(expense);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Expense expense)
    {
        if (id != expense.Id) return BadRequest();
        if (!ModelState.IsValid) return View(expense);
        _db.Update(expense);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var expense = await _db.Expenses.FindAsync(id);
        if (expense != null)
        {
            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
