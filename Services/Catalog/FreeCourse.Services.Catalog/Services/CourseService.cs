using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Net;

namespace FreeCourse.Services.Catalog.Services
{
    internal class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            _mapper = mapper;

            var client = new MongoClient(databaseSettings.ConnectionString);
            var db = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = db.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = db.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        }


        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();
            if (courses.Any())
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            else
                courses = new List<Course>();

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), (int)HttpStatusCode.OK);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(course => course.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return Response<CourseDto>.Fail($"{nameof(Course)} {id} {HttpStatusCode.NotFound}", (int)HttpStatusCode.NotFound);
            }

            course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), (int)HttpStatusCode.OK);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(course => course.UserId == userId).ToListAsync();
            if (courses.Any())
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            else
                courses = new List<Course>();

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), (int)HttpStatusCode.OK);
        }
    
        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto CourseDto)
        {
            var course = _mapper.Map<Course>(CourseDto);
            course.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(course);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), (int)HttpStatusCode.Created);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseDto)
        {
            var updateCourse = _mapper.Map<Course>(courseDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseDto.Id, updateCourse);
            if (result == null)
            {
                return Response<NoContent>.Fail($"{nameof(Course)} {courseDto.Id} {HttpStatusCode.NotFound}", (int)HttpStatusCode.NotFound);
            }

            return Response<NoContent>.Success((int)HttpStatusCode.NoContent); // 204
        }
      
        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount == 0)
            {
                return Response<NoContent>.Fail($"{nameof(Course)} {id} {HttpStatusCode.NotFound}", (int)HttpStatusCode.NotFound);
            }

            return Response<NoContent>.Success((int)HttpStatusCode.NoContent); // 204
        }

    }
}
