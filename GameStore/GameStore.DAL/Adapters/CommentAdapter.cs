using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters
{
    public class CommentAdapter : ICrossAdapter<Comment>
    {
        private readonly IGenericRepository<Comment> _repository;

        public CommentAdapter(IGenericRepository<Comment> context)
        {
            _repository = context;
        }

        public void Create(Comment comment)
        {
            _repository.Create(comment);
        }

        public IEnumerable<Comment> GetCross(int id, string crossProperty)
        {
            if (crossProperty != null)
            {
                var comments = _repository.Get(x => x.CrossProperty == crossProperty);

                return comments;
            }
            else
            {
                var comments = _repository.Get(x => x.GameId == id);

                return comments;
            }
        }

        public IEnumerable<Comment> Get()
        {
            return _repository.Get();
        }

        public IEnumerable<Comment> Get(Func<Comment, bool> predicate, 
            Func<IEnumerable<Comment>, IOrderedEnumerable<Comment>> sorting = null)
        {
            var query = _repository.Get(predicate, sorting);

            return query;
        }

        public void Update(Comment item)
        {
            _repository.Update(item);
        }

        public void Remove(Comment item)
        {
            _repository.Remove(item);
        }
    }
}
