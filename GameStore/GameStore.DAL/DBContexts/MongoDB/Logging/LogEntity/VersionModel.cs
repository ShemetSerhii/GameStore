using MongoDB.Bson;

namespace GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity
{
    public class VersionModel
    {
        public BsonDocument NewVersion { get; set; }
        public BsonDocument OldVersion { get; set; }
    }
}
