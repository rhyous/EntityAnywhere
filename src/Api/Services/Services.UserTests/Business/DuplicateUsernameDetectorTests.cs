using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass]
    public class DuplicateUsernameDetectorTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<User, IUser, long>> _MockRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRepo = _MockRepository.Create<IRepository<User, IUser, long>>();
        }

        private DuplicateUsernameDetector CreateDuplicateUsernameDetector()
        {
            return new DuplicateUsernameDetector(_MockRepo.Object);
        }

        [TestMethod]
        public void DuplicateUsernameDetector_Detect_DuplicateFound_Exception_Test()
        {
            // Arrange
            var user = new User { Id = 27, Username = "User1" };

            _MockRepo.Setup(m => m.GetByExpression(It.IsAny<Expression<Func<User, bool>>>(), "Id", SortOrder.Ascending))
                     .Returns(new IUser[] { user }.AsQueryable());
            var detector = CreateDuplicateUsernameDetector();

            // Act & Assert
            Assert.ThrowsException<DuplicateUsernameException>(() => detector.Detect(new[] { "User1" }));
        }

        [TestMethod]
        public void DuplicateUsernameDetector_Detect_DuplicateFound_NoException_Test()
        {
            // Arrange
            var user = new User { Id = 27, Username = "User1" };

            _MockRepo.Setup(m => m.GetByExpression(It.IsAny<Expression<Func<User, bool>>>(), "Id", SortOrder.Ascending))
                     .Returns(new[] { user }.AsQueryable());
            var detector = CreateDuplicateUsernameDetector();

            // Act
            var duplicates = detector.Detect(new[] { "User1" }, false)?.ToList();

            // Assert
            Assert.IsNotNull(duplicates);
            Assert.AreEqual(1, duplicates.Count);
            Assert.AreEqual("User1", duplicates[0]);
        }

        [TestMethod]
        public void DuplicateUsernameDetector_Detect_NoDuplicateFound_Null_Test()
        {
            // Arrange
            _MockRepo.Setup(m => m.GetByExpression(It.IsAny<Expression<Func<User, bool>>>(), "Id", SortOrder.Ascending))
                     .Returns((IQueryable<IUser>)null);
            var detector = CreateDuplicateUsernameDetector();

            // Act
            var duplicates = detector.Detect(new[] { "User1" }, false)?.ToList();

            // Assert
            Assert.IsNull(duplicates);
        }

        [TestMethod]
        public void DuplicateUsernameDetector_Detect_NoDuplicateFound_Empty_Test()
        {
            // Arrange
            var user = new User { Id = 27, Username = "User1" };

            _MockRepo.Setup(m => m.GetByExpression(It.IsAny<Expression<Func<User, bool>>>(), "Id", SortOrder.Ascending))
                     .Returns(new IUser[] { }.AsQueryable());
            var detector = CreateDuplicateUsernameDetector();

            // Act
            var duplicates = detector.Detect(new[] { "User1" }, false)?.ToList();

            // Assert
            Assert.IsNull(duplicates);
        }
    }
}
