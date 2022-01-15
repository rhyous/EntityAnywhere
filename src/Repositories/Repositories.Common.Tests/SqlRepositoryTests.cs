using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using TId = System.Int32;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests
{
    [TestClass]
    public class SqlRepositoryTests
    {
        internal MockRepository _MockRepository;
        private Mock<IObjectCreator<IBaseDbContext<User>>> _MockBaseDbContextCreator;
        private Mock<IBaseDbContext<User>> _MockUserDbContext;
        private Mock<DbSet<User>> _MockUserDbSet;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockBaseDbContextCreator = _MockRepository.Create<IObjectCreator<IBaseDbContext<User>>>();
            _MockUserDbContext = _MockRepository.Create<IBaseDbContext<User>>();
            _MockUserDbSet = _MockRepository.Create<DbSet<User>>();

        }

        public SqlRepository<User, IUser, int> CreateBaseRepository()
        {
            return new SqlRepository<User, IUser, int>(_MockBaseDbContextCreator.Object);
        }

        #region Create
        [TestMethod]
        [ListTNullOrEmpty(typeof(User))]
        public void BaseRepository_Create_NullOrEmptyItems_Test(List<User> users)
        {
            // Arrange
            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Create(users);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Create_ItemsValid_Test()
        {
            // Arrange
            var users = new List<User> { new User { Id = 1 }, new User { Id = 2 } };
            var dbSetUserList = new List<User>();
            _MockUserDbContext.Setup(m => m.Entities).Returns(_MockUserDbSet.Object);
            _MockUserDbContext.Setup(m => m.SaveChanges()).Returns(1);
            _MockUserDbContext.Setup(m => m.Dispose());
            _MockUserDbSet.Setup(m => m.AddRange(users))
                         .Returns((IEnumerable<User> inUsers) =>
                         {
                             dbSetUserList.AddRange(inUsers);
                             return users;
                         });
            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);
            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Create(users);

            // Assert
            CollectionAssert.AreEqual(users, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Delete
        [TestMethod]
        public void BaseRepository_Delete_NotFoundItem_ReturnsTrue_Test()
        {
            // Arrange
            var baseRepository = CreateBaseRepository();
            int id = 100;
            var dbSetUserList = new List<User>();
            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Provider)
                          .Returns(dbSetUserList.AsQueryable().Provider);
            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            // Must call _MockUserDbSet.Object after the _MockUserDbSet.As<IQueryable<User>>.Setup()
            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);
            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            // Act
            var result = baseRepository.Delete(id);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Delete_FoundAndDeletedItem_ReturnsTrue_Test()
        {
            // Arrange
            var baseRepository = CreateBaseRepository();
            int id = 100;
            var user100 = new User { Id = 100 };
            var dbSetUserList = new List<User> { user100 };
            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Provider)
                          .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbSet.Setup(m => m.Remove(user100))
                          .Returns((User inUser) =>
                          {
                              dbSetUserList.Remove(inUser);
                              return inUser;
                          });

            // Must call _MockUserDbSet.Object after the _MockUserDbSet.As<IQueryable<User>>.Setup()
            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);
            _MockUserDbContext.Setup(m => m.SaveChanges()).Returns(1);
            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            // Act
            var result = baseRepository.Delete(id);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, dbSetUserList.Count);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region DeleteMany
        [TestMethod]
        [ListTNullOrEmpty(typeof(int))]
        public void BaseRepository_DeleteMany_NullOrEmptyIds_Test(List<int> ids)
        {
            // Arrange
            var baseRepository = CreateBaseRepository();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                baseRepository.DeleteMany(ids);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_DeleteMany_FoundIds_ReturnTrue_Test()
        {
            // Arrange
            var user100 = new User { Id = 100 };
            var user200 = new User { Id = 200 };
            var dbSetUserList = new List<User> { user100, user200 };
            var ids = new List<int> { user100.Id, user200.Id };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<User>>()))
                          .Returns((IEnumerable<User> removeList) =>
                          {
                              foreach (var user in removeList.ToList())
                                  dbSetUserList.Remove(user);
                              return removeList;
                          });

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);
            _MockUserDbContext.Setup(m => m.SaveChanges()).Returns(1);
            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var actual = baseRepository.DeleteMany(ids);

            // Assert
            Assert.IsTrue(actual[user100.Id]);
            Assert.IsTrue(actual[user200.Id]);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_DeleteMany_NotFoundIds_ReturnTrue_Test()
        {
            // Arrange
            var user100 = new User { Id = 100 };
            var user200 = new User { Id = 200 };
            var user300 = new User { Id = 300 };
            var dbSetUserList = new List<User> { user100, user200 };
            var ids = new List<int> { user100.Id, user200.Id, user300.Id };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<User>>()))
                          .Returns((IEnumerable<User> removeList) =>
                          {
                              foreach (var user in removeList.ToList())
                                  dbSetUserList.Remove(user);
                              return removeList;
                          });

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);
            _MockUserDbContext.Setup(m => m.SaveChanges()).Returns(1);
            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var actual = baseRepository.DeleteMany(ids);

            // Assert
            Assert.IsTrue(actual[user100.Id]);
            Assert.IsTrue(actual[user200.Id]);
            Assert.IsTrue(actual[user300.Id]);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_DeleteMany_NotDeleted_ReturnsFalse_Test()
        {
            // Arrange
            var user100 = new User { Id = 100 };
            var user200 = new User { Id = 200 };
            var user300 = new User { Id = 300 };
            var dbSetUserList = new List<User> { user100, user200, user300 };
            var ids = new List<int> { user100.Id, user200.Id, user300.Id };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<User>>()))
                          .Returns((IEnumerable<User> removeList) =>
                          {
                              var leaveOneList = removeList.Where(u => u != user300).ToList();
                              foreach (var user in leaveOneList)
                                  dbSetUserList.Remove(user);
                              return removeList;
                          });

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);
            _MockUserDbContext.Setup(m => m.SaveChanges()).Returns(1);
            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var actual = baseRepository.DeleteMany(ids);

            // Assert
            Assert.IsTrue(actual[user100.Id]);
            Assert.IsTrue(actual[user200.Id]);
            Assert.IsFalse(actual[user300.Id]);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Get<TProperty>(orderBy, sortOrder)
        [TestMethod]
        [StringIsNullEmptyOrWhitespace()]
        public void BaseRepository_Get_OrderByNullEmptyOrWhitespace_DefaultOrerByProperty_Id_Ascending_Test(string orderBy)
        {
            // Arrange
            SortOrder sortOrder = SortOrder.Ascending;
            var user100 = new User { Id = 100 };
            var user200 = new User { Id = 200 };
            var user300 = new User { Id = 300 };
            var dbSetUserList = new List<User> { user300, user200, user100 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get<int>(orderBy, sortOrder).ToList();

            // Assert
            Assert.AreEqual(100, result[0].Id);
            Assert.AreEqual(200, result[1].Id);
            Assert.AreEqual(300, result[2].Id);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace()]
        public void BaseRepository_Get_OrderByNullEmptyOrWhitespace_DefaultOrerByProperty_Id_Descending_Test(string orderBy)
        {
            // Arrange
            SortOrder sortOrder = SortOrder.Descending;
            var user100 = new User { Id = 100 };
            var user200 = new User { Id = 200 };
            var user300 = new User { Id = 300 };
            var dbSetUserList = new List<User> { user300, user200, user100 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get<int>(orderBy, sortOrder).ToList();

            // Assert
            Assert.AreEqual(300, result[0].Id);
            Assert.AreEqual(200, result[1].Id);
            Assert.AreEqual(100, result[2].Id);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Get_OrderByNullEmptyOrWhitespace_NameProperty_Ascending_Test()
        {
            // Arrange
            string orderBy = "Name";
            SortOrder sortOrder = SortOrder.Ascending;
            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get<string>(orderBy, sortOrder).ToList();

            // Assert
            Assert.AreEqual(200, result[0].Id);
            Assert.AreEqual(300, result[1].Id);
            Assert.AreEqual(100, result[2].Id);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Get_OrderByNullEmptyOrWhitespace_NameProperty_Descending_Test()
        {
            // Arrange
            string orderBy = "Name";
            SortOrder sortOrder = SortOrder.Descending;
            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get<string>(orderBy, sortOrder).ToList();

            // Assert
            Assert.AreEqual(100, result[0].Id);
            Assert.AreEqual(300, result[1].Id);
            Assert.AreEqual(200, result[2].Id);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Get(orderExpression)
        [TestMethod]
        public void BaseRepository_Get_OrderExpressionNull_ExistingOrder()
        {
            // Arrange
            Expression<Func<User, int>> orderExpression = null;
            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IEnumerable<User>>()
                          .Setup(m => m.GetEnumerator())
                          .Returns(dbSetUserList.GetEnumerator());

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(orderExpression);
            var list = result.ToList();

            // Assert
            Assert.AreEqual(100, list[0].Id);
            Assert.AreEqual(200, list[1].Id);
            Assert.AreEqual(300, list[2].Id);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Get_OrderExpression_ByName_ExistingOrder()
        {
            // Arrange
            Expression<Func<User, string>> orderExpression = u => u.Name;
            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(orderExpression);
            var list = result.ToList();

            // Assert
            Assert.AreEqual(200, list[0].Id);
            Assert.AreEqual(300, list[1].Id);
            Assert.AreEqual(100, list[2].Id);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Get(Ids)
        [TestMethod]
        [ListTNullOrEmpty(typeof(int))]
        public void BaseRepository_Get_IdsNullOrEmpty_Throws(List<int> ids)
        {
            // Arrange
            var baseRepository = CreateBaseRepository();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                baseRepository.Get(ids);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Get_OneId_Found_Returns()
        {
            // Arrange
            IEnumerable<int> ids = new List<int> { 100 };

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(ids).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(100, result[0].Id);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Get_TwoIds_Found_Returns()
        {
            // Arrange
            IEnumerable<int> ids = new List<int> { 100, 300 };

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(ids).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(100, result[0].Id);
            Assert.AreEqual(300, result[1].Id);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Get(id)
        [TestMethod]
        public void BaseRepository_Get_IdNotFound_ReturnsNull()
        {
            // Arrange
            TId id = 400;
            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(id);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Get_IdFound_Returns()
        {
            // Arrange
            TId id = 200;
            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(id);

            // Assert
            Assert.AreEqual(user200, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Get(propertyValue, propertyExpression)
        [TestMethod]
        public void BaseRepository_Get_PropertyExpression_Null_Throws()
        {
            // Arrange
            string propertyValue = null;
            Expression<Func<User, string>> propertyExpression = null;

            var baseRepository = CreateBaseRepository();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                baseRepository.Get(propertyValue, propertyExpression);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void BaseRepository_Get_PropertyValue_NullEmptyOrWhitespace_TreatedAsFindableValue(string propertyValue)
        {
            // Arrange
            Expression<Func<User, string>> propertyExpression = u => u.Name;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(propertyValue, propertyExpression);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Get_PropertyValue_ValidValue_Returned()
        {
            // Arrange
            string propertyValue = "Lucy";
            Expression<Func<User, string>> propertyExpression = u => u.Name;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(propertyValue, propertyExpression);

            // Assert
            Assert.AreEqual(user300, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Get_PropertyValue_ValidValueButNotDistinct_FirstFoundReturned()
        {
            // Arrange
            string propertyValue = "Lucy";
            Expression<Func<User, string>> propertyExpression = u => u.Name;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Get(propertyValue, propertyExpression);

            // Assert
            Assert.AreEqual(user300, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetByExpression orderBy = "PropertyName"
        [TestMethod]
        public void BaseRepository_GetByExpression_ExpressionNull_Throws()
        {
            // Arrange
            Expression<Func<User, bool>> expression = null;

            var baseRepository = CreateBaseRepository();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                baseRepository.GetByExpression(expression);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_ExpressionValid_OrderByPropertyNotMemberOfType_Throws()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Id == 100;
            string orderBy = "Property1";

            var baseRepository = CreateBaseRepository();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                baseRepository.GetByExpression(expression, orderBy);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_ValidExpression_OrderByNullDefaultsToId_NotFound_ReturnsEmpty()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Id == 400;
            string orderBy = null;
            SortOrder sortOrder = SortOrder.Ascending;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GetByExpression(expression, orderBy, sortOrder).ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_ValidExpression_OrderByNullDefaultsToId_FoundMultiple_ReturnsMultiple()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Name == "Lucy";
            string orderBy = null;
            SortOrder sortOrder = SortOrder.Ascending;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GetByExpression(expression, orderBy, sortOrder).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(user300, result[0]);
            Assert.AreEqual(user400, result[1]);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_ValidExpression_OrderByNullDefaultsToId_FoundMultiple_Descending_ReturnsMultiple()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Name == "Lucy";
            string orderBy = null;
            SortOrder sortOrder = SortOrder.Descending;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GetByExpression(expression, orderBy, sortOrder).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(user400, result[0]);
            Assert.AreEqual(user300, result[1]);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_ValidExpression_OrderByName_FoundMultiple_ReturnsMultiple()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Id > 1;
            string orderBy = "Name";
            SortOrder sortOrder = SortOrder.Ascending;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GetByExpression(expression, orderBy, sortOrder).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(user200, result[0]);
            Assert.AreEqual(user300, result[1]);
            Assert.AreEqual(user100, result[2]);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetByExpression OrderExpression
        [TestMethod]
        public void BaseRepository_GetByExpression_OrderExpression_ExpressionNull_Throws()
        {
            // Arrange
            Expression<Func<User, bool>> expression = null;
            Expression<Func<User, int>> orderExpression = null;
            SortOrder sortOrder = SortOrder.Ascending;

            var baseRepository = CreateBaseRepository();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                baseRepository.GetByExpression<int>(expression, orderExpression, sortOrder);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_OrderExpression_ValidExpression_OrderByNullDefaultsToId_NotFound_ReturnsEmpty()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Id == 400;
            Expression<Func<User, int>> orderExpression = null;
            SortOrder sortOrder = SortOrder.Ascending;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GetByExpression<int>(expression, orderExpression, sortOrder).ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_OrderExpression_ValidExpression_OrderByNullDefaultsToId_FoundMultiple_ReturnsMultiple()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Name == "Lucy";
            Expression<Func<User, int>> orderExpression = null;
            SortOrder sortOrder = SortOrder.Ascending;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GetByExpression<int>(expression, orderExpression, sortOrder).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(user300, result[0]);
            Assert.AreEqual(user400, result[1]);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_OrderExpression_ValidExpression_OrderByNullDoesNotOrder_FoundMultiple_Descending_ReturnsMultiple()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Name == "Lucy";
            Expression<Func<User, int>> orderExpression = null;
            SortOrder sortOrder = SortOrder.Descending;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GetByExpression<int>(expression, orderExpression, sortOrder).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(user300, result[0]);
            Assert.AreEqual(user400, result[1]);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GetByExpression_OrderExpression_ValidExpression_OrderByName_FoundMultiple_ReturnsMultiple()
        {
            // Arrange
            Expression<Func<User, bool>> expression = u => u.Id > 1;
            Expression<Func<User, string>> orderExpression = u => u.Name;
            SortOrder sortOrder = SortOrder.Ascending;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300 };

            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GetByExpression<string>(expression, orderExpression, sortOrder).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(user200, result[0]);
            Assert.AreEqual(user300, result[1]);
            Assert.AreEqual(user100, result[2]);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Search
        [TestMethod]
        public void BaseRepository_Search_PropertyExpressions_Null_Throws()
        {
            // Arrange
            var baseRepository = CreateBaseRepository();
            string searchValue = "abc";
            Expression<Func<User, string>>[] propertyExpressions = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                baseRepository.Search(searchValue, propertyExpressions);
            });
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Update
        [TestMethod]
        public void BaseRepository_Update_PatchedEntity_Null_Throws()
        {
            // Arrange
            var baseRepository = CreateBaseRepository();
            PatchedEntity<IUser, int> patchedEntity = null;
            bool stage = false;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                baseRepository.Update(patchedEntity, stage);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Update_OneValid_Updates()
        {
            // Arrange
            var entity = new User { Id = 400, Name = "Jenny" };
            var patchedEntity = new PatchedEntity<IUser, TId>
            {
                Entity = entity,
                ChangedProperties = new HashSet<string> { nameof(User.Name) }
            };

            bool stage = false;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            _MockUserDbSet.Setup(m => m.Attach(It.IsAny<User>()))
                          .Returns((User u) => u);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockUserDbContext.Setup(m => m.SaveChanges()).Returns(1);
            _MockUserDbContext.Setup(m => m.Dispose());

            var dbEntityEntryUser = _MockRepository.Create<DbEntityEntry<User>>();

            _MockUserDbContext.Setup(m => m.SetIsModified(It.IsAny<User>(), nameof(User.Name), true));

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IUpdateDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.Update(patchedEntity, stage);

            // Assert
            Assert.AreEqual("Jenny", result.Name);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region BulkUpdate
        [TestMethod]
        public void BaseRepository_BulkUpdate_PatchedEntityCollection_Null_Throws()
        {
            // Arrange
            PatchedEntityCollection<IUser, int> patchedEntityCollection = null;
            bool stage = false;

            var baseRepository = CreateBaseRepository();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                baseRepository.BulkUpdate(patchedEntityCollection, stage);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_BulkUpdate_PatchedEntityCollectionDotPatchedEntities_Empty_Throws()
        {
            // Arrange
            var patchedEntityCollection = new PatchedEntityCollection<IUser, TId>();
            bool stage = false;

            var baseRepository = CreateBaseRepository();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                baseRepository.BulkUpdate(patchedEntityCollection, stage);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_BulkUpdate_PatchedEntityCollectionDotPatchedEntities_HasNullItem_Throws()
        {
            // Arrange
            var patchedEntityCollection = new PatchedEntityCollection<IUser, TId>();
            patchedEntityCollection.PatchedEntities.Add(null);
            bool stage = false;

            var baseRepository = CreateBaseRepository();
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                baseRepository.BulkUpdate(patchedEntityCollection, stage);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_BulkUpdate_OneValid_Updates()
        {
            // Arrange
            var patchedEntityCollection = new PatchedEntityCollection<IUser, TId>();
            var entity = new User { Id = 400, Name = "Jenny" };
            var patchedEntity = new PatchedEntity<IUser, TId>
            {
                Entity = entity,
                ChangedProperties = new HashSet<string> { nameof(User.Name) }
            };
            patchedEntityCollection.PatchedEntities.Add(patchedEntity);
            bool stage = false;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            _MockUserDbSet.Setup(m => m.Attach(It.IsAny<User>()))
                          .Returns((User u) => u);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockUserDbContext.Setup(m => m.SaveChanges()).Returns(1);
            _MockUserDbContext.Setup(m => m.Dispose());

            var dbEntityEntryUser = _MockRepository.Create<DbEntityEntry<User>>();

            _MockUserDbContext.Setup(m => m.SetIsModified(It.IsAny<User>(), nameof(User.Name), true));

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IUpdateDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.BulkUpdate(patchedEntityCollection, stage);

            // Assert
            Assert.AreEqual("Jenny", result[0].Name);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_BulkUpdate_TwoValid_ChangedPropertiesInCollection_Updates()
        {
            // Arrange
            var patchedEntityCollection = new PatchedEntityCollection<IUser, TId>
            {
                ChangedProperties = new HashSet<string> { nameof(User.Name) }
            };
            var entity1 = new User { Id = 300, Name = "Jenny" };
            var patchedEntity1 = new PatchedEntity<IUser, TId>
            {
                Entity = entity1
            };
            patchedEntityCollection.PatchedEntities.Add(patchedEntity1);
            var entity2 = new User { Id = 400, Name = "Michelle" };
            var patchedEntity2 = new PatchedEntity<IUser, TId>
            {
                Entity = entity2
            };
            patchedEntityCollection.PatchedEntities.Add(patchedEntity2);

            bool stage = false;

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            _MockUserDbSet.Setup(m => m.Attach(It.IsAny<User>()))
                          .Returns((User u) => u);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);

            _MockUserDbContext.Setup(m => m.SaveChanges()).Returns(1);
            _MockUserDbContext.Setup(m => m.Dispose());

            var dbEntityEntryUser = _MockRepository.Create<DbEntityEntry<User>>();

            _MockUserDbContext.Setup(m => m.SetIsModified(It.IsAny<User>(), nameof(User.Name), true));

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IUpdateDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.BulkUpdate(patchedEntityCollection, stage);

            // Assert
            Assert.AreEqual("Jenny", result[0].Name);
            Assert.AreEqual("Michelle", result[1].Name);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GenerateRepository
        [TestMethod]
        public void BaseRepository_GenerateRepository_True()
        {
            // Arrange

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };
            
            _MockUserDbSet.As<IQueryable<User>>()
              .Setup(m => m.Provider)
              .Returns(dbSetUserList.AsQueryable().Provider);

            _MockUserDbSet.As<IQueryable<User>>()
                          .Setup(m => m.Expression)
                          .Returns(dbSetUserList.AsQueryable().Expression);

            _MockUserDbContext.Setup(m => m.Entities)
                              .Returns(_MockUserDbSet.Object);            
            
            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GenerateRepository();

            // Assert
            Assert.AreEqual("User", result.Name);
            Assert.IsTrue(result.RepositoryReady);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_GenerateRepository_False()
        {
            // Arrange

            var user100 = new User { Id = 100, Name = "Sally" };
            var user200 = new User { Id = 200, Name = "Annie" };
            var user300 = new User { Id = 300, Name = "Lucy" };
            var user400 = new User { Id = 400, Name = "Lucy" };
            var dbSetUserList = new List<User> { user100, user200, user300, user400 };

            var failureMsg = "It's broke!";
            _MockUserDbContext.Setup(m => m.Entities)
                              .Throws(new Exception(failureMsg));

            _MockUserDbContext.Setup(m => m.Dispose());

            _MockBaseDbContextCreator.Setup(m => m.Create(typeof(IBaseDbContext<User>)))
                                     .Returns(_MockUserDbContext.Object);

            var baseRepository = CreateBaseRepository();

            // Act
            var result = baseRepository.GenerateRepository();

            // Assert
            Assert.AreEqual("User", result.Name);
            Assert.IsFalse(result.RepositoryReady);
            Assert.AreEqual(failureMsg, result.FailureReason);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Dispose
        [TestMethod]
        public void BaseRepository_Dispose_NothingToDispose()
        {
            // Arrange
            var baseRepository = CreateBaseRepository();

            // Act
            baseRepository.Dispose();

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseRepository_Dispose_UndisposedContextsAreDisposed()
        {
            // Arrange
            var mockContext1 = _MockRepository.Create<IBaseDbContext<User>>();
            var mockContext2 = _MockRepository.Create<IBaseDbContext<User>>();

            mockContext1.Setup(m => m.Dispose());
            mockContext2.Setup(m => m.Dispose());

            var baseRepository = CreateBaseRepository();
            baseRepository._UndisposedContexts.Add(mockContext1.Object);
            baseRepository._UndisposedContexts.Add(mockContext2.Object);

            // Act
            baseRepository.Dispose();

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
