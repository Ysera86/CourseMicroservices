using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Net;

namespace FreeCourse.Services.Catalog.Services
{
    internal interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryDto);
        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
