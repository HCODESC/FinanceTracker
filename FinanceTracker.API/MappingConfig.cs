using AutoMapper;
using FinanceTracker.Shared.DTOs;
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
        CreateMap<BudgetRequestDto, Budget>().ReverseMap();
        
    }
}