using GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace GameStore.DAL.DBContexts.MongoDB.Logging
{
    public class EntityLogging : ILogging
    {
        private readonly MongoDbContext _context;
        public Dictionary<CUDEnum, string> CudDictionary { get; }

        public EntityLogging(MongoDbContext context)
        {
            _context = context;

            CudDictionary = new Dictionary<CUDEnum, string>
            {
                {CUDEnum.Create, "Create" },
                {CUDEnum.Delete, "Delete" },
                {CUDEnum.Update, "Update" }
            };
        }

        public void Log(Type type, string action, BsonDocument item, BsonDocument itemOld = null)
        {
            var log = new LogModel
            {
                LogDate = DateTime.UtcNow,
                Action = action,
                EntityType = type.Name,
                Versions = new VersionModel
                {
                    NewVersion = item,
                    OldVersion = itemOld
                }
            };

            _context.EntityLogs.InsertOne(log);
        }
    }
}
