using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FreeCourse.Services.Catalog.Dtos
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
    }
}
