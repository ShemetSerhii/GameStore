using GameStore.DAL.DBContexts.EF;
using GameStore.DAL.DBContexts.EF.Repositories;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.Tests.DBContexts.EF.Repositories
{
    [TestFixture]
    public class CommentRepositoryTests
    {
        private const string CommentTestName = "TestName";

        private Mock<SqlContext> _contextMock;
        private Mock<DbSet<Comment>> _entitiesMock;

        [SetUp]
        public void Init()
        {
            _contextMock = new Mock<SqlContext>();
            _entitiesMock = new Mock<DbSet<Comment>>();
        }

        [Test]
        public void Create_WhenCommentNotNull_CallsActionFromDbContext()
        {
            var comment = new Comment() {Id = 1};

            _entitiesMock.Setup(x => x.Add(comment));
            _contextMock.Setup(x => x.Set<Comment>()).Returns(_entitiesMock.Object);
            var repos = new SqlCommentRepository(_contextMock.Object);

            repos.Create(comment);

            _entitiesMock.Verify(x => x.Add(comment), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateIsId_ReturnAllCommentsByGame()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<Comment>()).Returns(_entitiesMock.Object);
            var repository = new SqlCommentRepository(_contextMock.Object);

            var result = repository.Get(x => x.Id == 1).FirstOrDefault();

            Assert.AreEqual(result.Name, CommentTestName);
        }

        [Test]
        public void Get_WhenPredicateIsNull_ReturnAllComments()
        {
            SetMockDbSet();

            _contextMock.Setup(x => x.Set<Comment>()).Returns(_entitiesMock.Object);
            var repository = new SqlCommentRepository(_contextMock.Object);

            var result = repository.Get();

            Assert.AreEqual(result.Count(), 2);
        }

        private void SetMockDbSet()
        {
            var comment = new List<Comment>
            {
                new Comment() {Id = 1, Name = CommentTestName, GameId = 1},
                new Comment() {Id = 2, Name = "2", GameId = 2}
            }.AsQueryable();

            _entitiesMock.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(comment.Provider);
            _entitiesMock.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(comment.Expression);
            _entitiesMock.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(comment.ElementType);
            _entitiesMock.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(comment.GetEnumerator());
        }
    }
}
