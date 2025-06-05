using FinanceTracker.API.DTOs.Transaction;
using FinanceTracker.API.Helpers;

namespace FinanceTracker.API.Services.Transaction;

public interface ITransactionService
{
    // Create transaction
    Task<ServiceResult<TransactionResponseDto>> CreateTransactionAsync(TransactionRequestDto transactionRequestDto, Guid userId); 
    // Update Transaction
    Task<ServiceResult<TransactionResponseDto>> EditTransactionAsync(TransactionRequestDto transactionRequestDto, Guid id, Guid userId);
    // Delete Transaction 
    Task<bool> DeleteTransactionAsync(Guid id, Guid userId);
    // FindTransactionById
    Task<ServiceResult<TransactionResponseDto>> GetTransactionByIdAsync(Guid id, Guid userId);
    //GetAllTransaction
    Task<PagedResult<TransactionResponseDto>> GetAllTransactionsAsync(Guid userId, int page = 1, int pageSize = 20); 
}