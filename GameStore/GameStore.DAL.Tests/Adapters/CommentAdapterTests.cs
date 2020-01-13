using GameStore.DAL.Adapters;
using GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GameStore.DAL.Tests.Adapters
{
    [TestFixture]
    public class CommentAdapterTests
    {
        private Mock<IGenericRepository<Comment>> _commentMock;
        private Mock<ILogging> _loggerMock;

        [Test]
        public void Create_WhenItemNotNull_CallCreateActionFromSqlRepository()
        {
            var adapter = new CommentAdapter(_commentMock.Object, _loggerMock.Object);

            var comment = new Comment();

            adapter.Create(comment);

            _commentMock.Verify(x => x.Create(comment), Times.Once);
        }

        [Test]
        public void Get_Always_CallGetActionFromSqlRepository()
        {
            var adapter = new CommentAdapter(_commentMock.Object, _loggerMock.Object);

            adapter.Get();

            _commentMock.Verify(x => x.Get(), Times.Once);
        }

        [Test]
        public void Get_WhenPredicateNotNull_CallGetActionWithPredicateFromSqlRepository()
        {
            var adapter = new CommentAdapter(_commentMock.Object, _loggerMock.Object);

            adapter.Get(It.IsAny<Func<Comment, bool>>());

            _commentMock.Verify(x => x.Get(It.IsAny<Func<Comment, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetCross_WhenCrossPropertyIsNull_CallGetActionWithPredicateFromSqlRepository()
        {
            var adapter = new CommentAdapter(_commentMock.Object, _loggerMock.Object);

            adapter.GetCross(1, null);

            _commentMock.Verify(x => x.Get(It.IsAny<Func<Comment, bool>>(), null), Times.Once);
        }

        [Test]
        public void GetCross_WhenCrossPropertyIsNotNull_CallGetActionWithPredicateFromSqlRepository()
        {
            var adapter = new CommentAdapter(_commentMock.Object, _loggerMock.Object);

            adapter.GetCross(1, "1");

            _commentMock.Verify(x => x.Get(It.IsAny<Func<Comment, bool>>(), null), Times.Once);
        }

        [Test]
        public void Update_WhenItemNotNull_CallUpdateActionFromSqlRepository()
        {
            var adapter = new CommentAdapter(_commentMock.Object, _loggerMock.Object);

            var comment = new Comment();

            adapter.Update(comment);

            _commentMock.Verify(x => x.Update(comment), Times.Once);
        }

        [Test]
        public void Remove_WhenItemNotNull_CallRemoveActionFromSqlRepository()
        {
            var adapter = new CommentAdapter(_commentMock.Object, _loggerMock.Object);

            var comment = new Comment();

            adapter.Remove(comment);

            _commentMock.Verify(x => x.Remove(comment), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            _commentMock = new Mock<IGenericRepository<Comment>>();
            _loggerMock = new Mock<ILogging>();

            _loggerMock.Setup(x => x.CudDictionary).Returns(new Dictionary<CUDEnum, string>
            {
                {CUDEnum.Update, "Update" },
                {CUDEnum.Delete, "Delete" },
                {CUDEnum.Create, "Create" }
            });
        }
    }
}
