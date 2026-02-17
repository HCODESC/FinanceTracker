using AutoMapper;
using FinanceTracker.API.Data;
using FinanceTracker.Shared.DTOs;
using FinanceTracker.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Transaction;

public class TransactionService(FinanceTrackerDbContext context, IMapper mapper, ILogger<TransactionService> logger)
    : ITransactionService
{
    public async Task<ServiceResult<TransactionResponseDto>> CreateTransactionAsync(
        TransactionRequestDto transactionRequestDto, Guid userId)
    {
        try
        {
            Guid categoryId;

            //Check if user provided a category name to create or find
            if (!string.IsNullOrWhiteSpace(transactionRequestDto.CreateCategoryName))
            {
                var categoryName = transactionRequestDto.CreateCategoryName.Trim();
                
                var existingCategory = await context.Categories
                    .FirstOrDefaultAsync(x => x.Name == categoryName && x.UserProfileId == userId);

                if (existingCategory != null)
                {
                    categoryId = existingCategory.Id;
                }
                else
                {
                    // Create new category on the fly
                    var newCategory = new Model.Category
                    {
                        Id = Guid.NewGuid(),
                        Name = categoryName,
                        UserProfileId = userId
                    };

                    await context.Categories.AddAsync(newCategory);
                    await context.SaveChangesAsync();
                    categoryId = newCategory.Id;
                }
            }
            //Use provided CategoryId
            else if (transactionRequestDto.CategoryId.HasValue && transactionRequestDto.CategoryId.Value != Guid.Empty)
            {
                categoryId = transactionRequestDto.CategoryId.Value;
                
                context.ChangeTracker.Clear();
                var categoryExists = await context.Categories.IgnoreQueryFilters()
                    .AnyAsync(c => c.Id == categoryId && c.UserProfileId == userId);

                if (!categoryExists)
                    return ServiceResult<TransactionResponseDto>.Failure(
                        "Category not found or doesn't belong to user");
            }
            //Fallback to "Uncategorized"
            else
            {
                var uncategorized = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == "Uncategorized" && x.UserProfileId == userId);

                if (uncategorized == null)
                {
                    uncategorized = new Model.Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Uncategorized",
                        UserProfileId = userId
                    };

                    await context.Categories.AddAsync(uncategorized);
                    await context.SaveChangesAsync();
                }

                categoryId = uncategorized.Id;
            }

            var transaction = mapper.Map<Model.Transaction>(transactionRequestDto);
            transaction.Id = Guid.NewGuid();
            transaction.UserProfileId = userId;
            transaction.CategoryId = categoryId;

            await context.AddAsync(transaction);
            await context.SaveChangesAsync();

            var createdTransaction = await context.Transactions.Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == transaction.Id);
            var responseDto = mapper.Map<TransactionResponseDto>(createdTransaction);
            return ServiceResult<TransactionResponseDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Creating transaction for user: {userId}", userId);
            return ServiceResult<TransactionResponseDto>.Failure("Failed to create transaction");
        }
    }


    public async Task<ServiceResult<TransactionResponseDto>> EditTransactionAsync(
        TransactionRequestDto transactionRequestDto, Guid transactionId, Guid userId)
    {
        try
        {
            var transaction = context.Transactions.FirstOrDefault(t => t.Id == transactionId);

            if (transaction == null)
                return ServiceResult<TransactionResponseDto>.Failure("Transaction Not Found");

            if (transaction.UserProfileId != userId)
                return ServiceResult<TransactionResponseDto>.Failure("You are not authorize to edit this transaction");

            // Priority 1: Check if user provided a category name to create or find
            if (!string.IsNullOrWhiteSpace(transactionRequestDto.CreateCategoryName))
            {
                var categoryName = transactionRequestDto.CreateCategoryName.Trim();
                
                var existingCategory = await context.Categories
                    .FirstOrDefaultAsync(x => x.Name == categoryName && x.UserProfileId == userId);

                if (existingCategory != null)
                {
                    transaction.CategoryId = existingCategory.Id;
                }
                else
                {
                    // Create new category on the fly
                    var newCategory = new Model.Category
                    {
                        Id = Guid.NewGuid(),
                        Name = categoryName,
                        UserProfileId = userId
                    };

                    await context.Categories.AddAsync(newCategory);
                    await context.SaveChangesAsync();
                    transaction.CategoryId = newCategory.Id;
                }
            }
            // Priority 2: Use provided CategoryId
            else if (transactionRequestDto.CategoryId.HasValue && transactionRequestDto.CategoryId.Value != Guid.Empty)
            {
                var categoryId = transactionRequestDto.CategoryId.Value;
                var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId && c.UserProfileId == userId);

                if (!categoryExists)
                    return ServiceResult<TransactionResponseDto>.Failure("Invalid category or unathorize access");
                
                transaction.CategoryId = categoryId;
            }
             // Priority 3: Fallback to "Uncategorized" if nothing provided (or keep existing? Assuming full update means Uncategorized if omitted)
            else
            {
                var uncategorized = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == "Uncategorized" && x.UserProfileId == userId);

                if (uncategorized == null)
                {
                    uncategorized = new Model.Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Uncategorized",
                        UserProfileId = userId
                    };

                    await context.Categories.AddAsync(uncategorized);
                    await context.SaveChangesAsync();
                }

                transaction.CategoryId = uncategorized.Id;
            }

            transaction.Amount = transactionRequestDto.Amount;
            transaction.Type = transactionRequestDto.Type;
            transaction.Note = transactionRequestDto.Note;

            context.Update(transaction);
            await context.SaveChangesAsync();
            var responseDto = mapper.Map<TransactionResponseDto>(transaction);
            return ServiceResult<TransactionResponseDto>.Success(responseDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error Updating transaction for user: {userId}", userId);
            return ServiceResult<TransactionResponseDto>.Failure("Failed to update transaction");
        }
    }

    public async Task<bool> DeleteTransactionAsync(Guid id, Guid userId)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserProfileId == userId);

            if (transaction == null)
                return false;

            context.Remove(transaction);
            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error Deleting transaction for user: {userId}", userId);
            return false;
        }
    }

    public async Task<ServiceResult<TransactionResponseDto>> GetTransactionByIdAsync(Guid id, Guid userId)
    {
        try
        {
            var transaction = await context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return ServiceResult<TransactionResponseDto>.Failure("Transaction not found");


            if (transaction.UserProfileId != userId)
                return ServiceResult<TransactionResponseDto>.Failure("You are not authorize to view this transaction");

            var responseDto = mapper.Map<TransactionResponseDto>(transaction);
            return ServiceResult<TransactionResponseDto>.Success(responseDto);
        }
        catch (Exception e)
        {
            logger.LogError("There has been an error getting transaction with userid: {userId}", userId);
            return ServiceResult<TransactionResponseDto>.Failure("Failed to get transaction");
        }
    }

    public async Task<PagedResult<TransactionResponseDto>> GetAllTransactionsAsync(Guid userId, int page = 1,
        int pageSize = 20)
    {
        try
        {
            var query = context.Transactions
                .Include(ca => ca.Category)
                .Where(t => t.UserProfileId == userId)
                .OrderByDescending(t => t.TransactionDate);

            var totalCount = await query.CountAsync();

            var transactions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var responseDto = transactions.Select(transaction => mapper.Map<TransactionResponseDto>(transaction))
                .ToList();

            return new PagedResult<TransactionResponseDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = responseDto
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "There was issue getting transactions for user: {UserId}", userId);
            return new PagedResult<TransactionResponseDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = 0,
                Items = new List<TransactionResponseDto>()
            };
        }
    }
}