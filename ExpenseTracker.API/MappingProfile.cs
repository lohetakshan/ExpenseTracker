using AutoMapper;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.API.DTOs;

namespace ExpenseTracker.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<Expense, ExpenseDTO>();
            CreateMap<Category, CategoryDTO>();

            CreateMap<Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<Category, UpdateCategoryRequest>().ReverseMap();
        }
    }
}
