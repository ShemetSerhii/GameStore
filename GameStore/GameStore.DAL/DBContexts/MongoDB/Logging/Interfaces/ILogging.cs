using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces
{
    public interface ILogging
    {
        Dictionary<CUDEnum, string> CudDictionary { get; }
        void Log(Type type, string action, BsonDocument item, BsonDocument itemOld = null);
    }
}