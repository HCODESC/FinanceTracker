using AutoMapper;
using FinanceTracker.API.DTOs.Category;
using FinanceTracker.API.DTOs.Transaction;
using FinanceTracker.API.Model;

namespace FinanceTracker.API;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<TransactionRequestDto, Transaction>();
        CreateMap<Transaction, TransactionResponseDto>(); 
        CreateMap<CategoryRequestDto, Category>();
        CreateMap<Category, CategoryResponseDto>();
    }
}