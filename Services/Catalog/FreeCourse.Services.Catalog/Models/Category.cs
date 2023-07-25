using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Models
{
    public class Category
    {
        [BsonId] //Mongo bu Id
        [BsonRepresentation(BsonType.ObjectId)] // mongoda Id tipi ObjectId
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
