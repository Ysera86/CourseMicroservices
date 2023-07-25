using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FreeCourse.Services.Catalog.Dtos
{
    internal class CategoryCreateDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
