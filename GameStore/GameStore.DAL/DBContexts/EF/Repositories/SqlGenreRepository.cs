using AutoMapper;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories
{
    public class SqlGenreRepository : IGenericRepository<Genre>
    {
        private const string CategoryNameProperty = "CategoryName";
        private const string CategoryIdProperty = "CategoryID";

        private readonly SqlContext _context;
        private readonly IMongoContext _mongo;

        public SqlGenreRepository(SqlContext context, IMongoContext mongoContext)
        {
            _context = context;
            _mongo = mongoContext;
        }

        public void Create(Genre item)
        {
            SaveMongoParent(item);

            _context.Genres.Add(item);

            _context.SaveChanges();
        }

        public IEnumerable<Genre> Get()
        {
            return VerificationIsDeleted();
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate, 
            Func<IEnumerable<Genre>, IOrderedEnumerable<Genre>> sorting = null)
        {
            var query = VerificationIsDeleted().Where(predicate);

            foreach (var genre in query)
            {
                GetMongoParent(genre);
            }

            return query.Where(predicate).ToList();
        }

        public void Update(Genre item)
        {
            SaveMongoParent(item);

            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Genres.Attach(item);
            }

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Remove(Genre item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        private IEnumerable<Genre> VerificationIsDeleted()
        {
            var genres = _context.Genres.Where(x => x.IsDeleted == false).ToList();

            return genres;
        }

        private void SaveMongoParent(Genre genre)
        {
            DropMongoParent(genre);

            if (genre.Parent != null && genre.Parent.CrossProperty != null)
            {
                var filterForCategories = Builders<CategorieMongo>.Filter.Eq(CategoryNameProperty, genre.Parent.Name);

                var categories = _mongo.Categories.Find(filterForCategories).ToList().SingleOrDefault();

                categories.SqlChildrenId.Add(genre.Id);

                _mongo.Categories.ReplaceOne(filterForCategories, categories);

                genre.Parent = null;
            }
        }

        private void DropMongoParent(Genre genre)
        {
            var categories = _mongo.Categories.Find(new BsonDocument()).ToList();

            foreach (var category in categories)
            {
                category.SqlChildrenId = category.SqlChildrenId.Where(x => x != genre.Id).ToList();

                var filterForCategory = Builders<CategorieMongo>.Filter.Eq(CategoryIdProperty, category.CategoryID);

                _mongo.Categories.ReplaceOne(filterForCategory, category);
            }
        }

        private void GetMongoParent(Genre genre)
        {
             if (genre.ParentId != null)
             {
                 var categories = _mongo.Categories.AsQueryable().SingleOrDefault(cat =>
                     (cat.IsDeleted == null || cat.IsDeleted == false) && cat.SqlChildrenId.Contains(genre.Id));

                if (categories != null)
                {
                    var parentGenre = Mapper.Map<CategorieMongo, Genre>(categories);

                    genre.Parent = parentGenre;
                }
            }
        }
    }
}
