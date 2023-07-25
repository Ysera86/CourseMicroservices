using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtoss;
using MongoDB.Driver;
using System.Net;

namespace FreeCourse.Services.Catalog.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            _mapper = mapper;

            var client = new MongoClient(databaseSettings.ConnectionString);
            var db = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection = db.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        }


        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), (int)HttpStatusCode.OK);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), (int)HttpStatusCode.Created);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find(category => category.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return Response<CategoryDto>.Fail($"{nameof(Category)} {id} {HttpStatusCode.NotFound}", (int)HttpStatusCode.NotFound);
            }

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), (int)HttpStatusCode.OK);
        }

    }
}
