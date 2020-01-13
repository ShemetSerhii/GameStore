using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity
{
    public class LogModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime LogDate { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public VersionModel Versions { get; set; }
    }
}
