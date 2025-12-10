using FinanceTracker.API.Data;
using FinanceTracker.API.Model;
using FinanceTracker.API.Services.Report;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FinanceTracker.Shared.Enums;

namespace FinanceTracker.Test;

public class ReportServiceTest
{
    private readonly ReportService _reportService;
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly ILogger<ReportService>? _logger;

    public ReportServiceTest() 
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new FinanceTrackerDbContext(options);
        _reportService = new ReportService(_dbContext, _logger);
    }
    
    [Fact]
    public async void GetMonthlySummary_ReturnCorrectDetails()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var targetMonth = 6;
        var targetYear = 2025;
        decimal expectedTotalExpenses = 0m;
        
        _dbContext.Transactions.Add(new Transaction()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = 50m,
            TransactionDate = new DateTime(targetYear, targetMonth, 10, 0, 0, 0, DateTimeKind.Utc),
            CategoryId = Guid.NewGuid(),
            Note = "Lunch out",
            Type = TransactionType.EXPENSE
        });
        expectedTotalExpenses += 50m; // Add to our expected sum

        // Transaction 2: Another one for the target user, in the target month/year
        _dbContext.Transactions.Add(new Transaction()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = 70m,
            TransactionDate = new DateTime(targetYear, targetMonth, 20, 0, 0, 0, DateTimeKind.Utc),
            CategoryId = Guid.NewGuid(),
            Note = "Groceries shopping",
            Type = TransactionType.EXPENSE
        });
        expectedTotalExpenses += 70m; // Add to our expected sum

        _dbContext.Transactions.Add(new Transaction()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(), // Fix: Ensure this is a DIFFERENT user ID
            Amount = 200m,
            TransactionDate = new DateTime(targetYear, targetMonth, 15, 0, 0, 0, DateTimeKind.Utc),
            CategoryId = Guid.NewGuid(),
            Note = "Another user's expense",
            Type = TransactionType.EXPENSE
        });
        // expectedTotalExpenses += 200m; // Removed: Should not be included for the target user
        // Transaction for a different month/year (same user)
        _dbContext.Transactions.Add(new Transaction()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = 30m,
            TransactionDate = new DateTime(2024, 12, 5, 0, 0, 0, DateTimeKind.Utc), // Different year/month
            CategoryId = Guid.NewGuid(),
            Note = "Old expense",
            Type = TransactionType.INCOME
        });
        
        await _dbContext.SaveChangesAsync();
        //act
        var result = await _reportService.GetMonthlySummaryAsync(userId, 6, 2025); 
        
        //Assert
        Assert.True(result.IsSuccess, "The operation should be successful");
        Assert.NotNull(result.Data); 
        Assert.Equal(expectedTotalExpenses, result.Data.TotalExpenses);
    }
}