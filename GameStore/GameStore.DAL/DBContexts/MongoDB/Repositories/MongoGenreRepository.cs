using AutoMapper;
using GameStore.DAL.DBContexts.EF;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.DBContexts.MongoDB.Repositories
{
    public class MongoGenreRepository : IAdvancedMongoRepository<Genre>
    {
        private const string CategoryIdProperty = "CategoryID";

        private readonly IMongoContext _mongoContext;
        private readonly SqlContext _sqlContext;

        public MongoGenreRepository(IMongoContext context, SqlContext sqlContextContext)
        {
            _mongoContext = context;
            _sqlContext = sqlContextContext;
        }

        public IEnumerable<Genre> Get()
        {
            var categories = _mongoContext.Categories.AsQueryable().Where(cat => cat.IsDeleted == null || cat.IsDeleted == false);

            var genres = Mapper.Map<IEnumerable<CategorieMongo>, List<Genre>>(categories);

            GetParentGenres(genres);

            return genres;
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate, Func<IEnumerable<Genre>, IOrderedEnumerable<Genre>> sorting = null)
        {
            var categories = _mongoContext.Categories.AsQueryable().Where(cat => cat.IsDeleted == null || cat.IsDeleted == false);

            var genres = Mapper.Map<IEnumerable<CategorieMongo>, List<Genre>>(categories);

            genres = genres.Where(predicate).ToList();

            GetParentGenres(genres);

            return genres;
        }

        public void Remove(Genre item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        public void Update(Genre item)
        {
            var genre = Mapper.Map<Genre, CategorieMongo>(item);

            var filterForGenre = Builders<CategorieMongo>.Filter.Eq(CategoryIdProperty, genre.CategoryID);

            var recoverItem = _mongoContext.Categories.Find(filterForGenre).ToList().SingleOrDefault();

            genre.Id = recoverItem.Id;
            genre.CategoryID = recoverItem.CategoryID;

            _mongoContext.Categories.ReplaceOne(filterForGenre, genre);
        }

        private void GetParentGenres(IEnumerable<Genre> genres)
        {
            var genresId = genres.Select(g => int.Parse(g.CrossProperty.Substring(1)));

            var partCategories = _mongoContext.Categories.AsQueryable().Where(cat =>
                (cat.IsDeleted == null || cat.IsDeleted == false) && genresId.Contains(cat.CategoryID));

            foreach (var genre in genres)
            {
                var parent = GetParentGenre(partCategories.SingleOrDefault(x => x.CategoryName == genre.Name));

                genre.Parent = parent;
            }
        }

        private Genre GetParentGenre(CategorieMongo categorie)
        {
            var genreSql = _sqlContext.Genres.SingleOrDefault(x => x.Name == categorie.Parent && x.IsDeleted == false);

            if (genreSql == null && categorie.Parent != null)
            {
                var genreMongo = _mongoContext.Categories.AsQueryable().SingleOrDefault(cat =>
                    (cat.IsDeleted == null || cat.IsDeleted == false) && cat.Parent == categorie.Parent);

                var genre = Mapper.Map<CategorieMongo, Genre>(genreMongo);

                return genre;
            }

            return genreSql;
        }
    }
}
