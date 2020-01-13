using GameStore.BLL.Services;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class CommentServiceTests
    {
        private Mock<IUnitOfWork> mock;

        [Test]
        public void AddComment_WhenArgumentNotNull_CallsActionFromRepository()
        {
            var service = new CommentService(mock.Object);
            var comment = new Comment();

            service.AddComment(comment);

            mock.Verify(m => m.Comments.Create(comment), Times.Once);
        }

        [Test]
        public void GetComment_WhenParameterIsId_CallsActionFromRepository()
        {
            var service = new CommentService(mock.Object);

            service.GetComment(1);

            mock.Verify(m => m.Comments.Get(It.IsAny<Func<Comment, bool>>(), null), Times.Once);
        }

        [Test]
        public void Delete_WhenItemNotNull_CallRemoveActionFromAdapter()
        {
            var service = new CommentService(mock.Object);

            service.Delete(1);

            mock.Verify(m => m.Comments.Remove(It.Is<Comment>(x => x.Id == 1)), Times.Once);
        }

        [Test]
        public void GetAllCommentsByGame_WhenParameteGameKey_CallGetCrossActionFromAdapter()
        {
            var service = new CommentService(mock.Object);

            service.GetAllCommentsByGame("Key");

            mock.Verify(m => m.Comments.GetCross(1, "1"), Times.Once);
        }

        [SetUp]
        public void Init()
        {
            mock = new Mock<IUnitOfWork>();

            mock.Setup(m => m.Comments.Get()).Returns(new List<Comment>()
            {
                new Comment()
                {
                    Id = 1,
                    Body = "Body",
                    Name = "Name",
                    GameId = 1,
                    Order = 1,
                    ParentId = null,
                    ParentComment = null,
                    Game = new Game()
                }
            });

            mock.Setup(m => m.Comments.Get(It.IsAny<Func<Comment, bool>>(), null)).Returns(new List<Comment>
            {
                new Comment
                {
                    Id = 1
                }
            });

            mock.Setup(m => m.Games.Get(It.IsAny<Func<Game, bool>>(), null)).Returns(new List<Game>
            {
                new Game
                {
                    Id = 1,
                    CrossProperty = "1"
                }
            });
        }
    }
}
