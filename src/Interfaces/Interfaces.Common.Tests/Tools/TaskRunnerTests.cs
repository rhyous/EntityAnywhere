using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Threading.Tasks;
using T = System.String;
using T1 = System.Int32;
using T2 = System.String;
using T3 = System.Int64;
using T4 = System.String;
using T5 = System.Double;
using T6 = System.String;
using T7 = System.Guid;
using TResult = System.Object;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    [TestClass]
    public class TaskRunnerTests
    {
        #region Return Task
        // No params
        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_NullMethod_Throws_Test()
        {
            // Arrange            
            Func<Task> method = null;
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_MethodWasCalled_Test()
        {
            // Arrange            
            bool wasCalled = false;
            Func<Task> method = () =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            bool ignoreException = false;

            // Act
            TaskRunner.RunSynchonously(method, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 1 param
        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_1Input_NullMethod_Throws_Test()
        {
            // Arrange            
            Func<T, Task> method = null;
            bool ignoreException = false;
            T input = null;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_1Input_MethodWasCalled_Test()
        {
            // Arrange            
            bool wasCalled = false;
            Func<T, Task> method = (T t) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };
            T input = null;

            bool ignoreException = false;

            // Act
            TaskRunner.RunSynchonously(method, input, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }


        // 2 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_2Input_NullMethod_Throws_Test()
        {
            // Arrange            
            Func<T1, T2, Task> method = null;
            bool ignoreException = false;
            T1 input1 = default(T1);
            T2 input2 = null;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_2Input_MethodWasCalled_Test()
        {
            // Arrange            
            bool ignoreException = false;
            T1 input1 = default(T1);
            T2 input2 = null;
            bool wasCalled = false;
            Func<T1, T2, Task> method = (T1 t1, T2 t2) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            // Act
            TaskRunner.RunSynchonously(method, input1, input2, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 3 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_3Input_NullMethod_Throws_Test()
        {
            // Arrange            
            Func<T1, T2, T3, Task> method = null;
            bool ignoreException = false;
            T1 input1 = default(T1);
            T2 input2 = null;
            T3 input3 = default(T3);

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, input3, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_3Input_MethodWasCalled_Test()
        {
            // Arrange            
            bool ignoreException = false;
            T1 input1 = default(T1);
            T2 input2 = null;
            T3 input3 = default(T3);
            bool wasCalled = false;
            Func<T1, T2, T3, Task> method = (T1 t1, T2 t2, T3 t3) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            // Act
            TaskRunner.RunSynchonously(method, input1, input2, input3, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 4 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_4Input_NullMethod_Throws_Test()
        {
            // Arrange            
            Func<T1, T2, T3, T4, Task> method = null;
            bool ignoreException = false;
            T1 input1 = default(T1);
            T2 input2 = null;
            T3 input3 = default(T3);
            T4 input4 = null;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, input3, input4, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_Task_4Input_MethodWasCalled_Test()
        {
            // Arrange            
            bool ignoreException = false;
            T1 input1 = default(T1);
            T2 input2 = null;
            T3 input3 = default(T3);
            T4 input4 = null;
            bool wasCalled = false;
            Func<T1, T2, T3, T4, Task> method = (T1 t1, T2 t2, T3 t3, T4 t4) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            // Act
            TaskRunner.RunSynchonously(method, input1, input2, input3, input4, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }
        #endregion

        #region return Task<TResult>
        // No Params
        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_NullMethod_Throws_Test()
        {
            // Arrange
            Func<Task<TResult>> method = null;
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_MethodWasCalled_Test()
        {
            // Arrange
            bool wasCalled = false;
            Func<Task<TResult>> method = () =>
            {
                wasCalled = true;
                return Task.FromResult(new TResult());
            };
            bool ignoreException = false;

            // Act
            var result = TaskRunner.RunSynchonously(method, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 1 param
        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_1Input_NullMethod_Throws_Test()
        {
            // Arrange
            Func<T1, Task<TResult>> method = null;
            T1 input1 = default(T1);
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_1Input_MethodWasCalled_Test()
        {
            // Arrange
            bool wasCalled = false;
            Func<T1, Task<TResult>> method = (T1 t1) =>
            {
                wasCalled = true;
                return Task.FromResult(new TResult());
            };
            T1 input1 = default(T1);
            bool ignoreException = false;

            // Act
            var result = TaskRunner.RunSynchonously(method, input1, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 2 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_2Input_NullMethod_Throws_Test()
        {
            // Arrange
            Func<T1, T2, Task<TResult>> method = null;
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_2Input_MethodWasCalled_Test()
        {
            // Arrange
            bool wasCalled = false;
            Func<T1, T2, Task<TResult>> method = (T1 t1, T2 t2) =>
            {
                wasCalled = true;
                return Task.FromResult(new TResult());
            };
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            bool ignoreException = false;

            // Act
            var result = TaskRunner.RunSynchonously(method, input1, input2, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 3 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_3Input_NullMethod_Throws_Test()
        {
            // Arrange
            Func<T1, T2, T3, Task<TResult>> method = null;
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, input3, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_3Input_MethodWasCalled_Test()
        {
            // Arrange
            bool wasCalled = false;
            Func<T1, T2, T3, Task<TResult>> method = (T1 t1, T2 t2, T3 t3) =>
            {
                wasCalled = true;
                return Task.FromResult(new TResult());
            };
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            bool ignoreException = false;

            // Act
            var result = TaskRunner.RunSynchonously(method, input1, input2, input3, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 4 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_4Input_NullMethod_Throws_Test()
        {
            // Arrange
            Func<T1, T2, T3, T4, Task<TResult>> method = null;
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            T4 input4 = default(T4);
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, input3, input4, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_4Input_MethodWasCalled_Test()
        {
            // Arrange
            bool wasCalled = false;
            Func<T1, T2, T3, T4, Task<TResult>> method = (T1 t1, T2 t2, T3 t3, T4 t4) =>
            {
                wasCalled = true;
                return Task.FromResult(new TResult());
            };
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            T4 input4 = default(T4);
            bool ignoreException = false;

            // Act
            var result = TaskRunner.RunSynchonously(method, input1, input2, input3, input4, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 5 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_5Input_NullMethod_Throws_Test()
        {
            // Arrange
            Func<T1, T2, T3, T4, T5, Task<TResult>> method = null;
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            T4 input4 = default(T4);
            T5 input5 = default(T5);
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, input3, input4, input5, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_5Input_MethodWasCalled_Test()
        {
            // Arrange
            bool wasCalled = false;
            Func<T1, T2, T3, T4, T5, Task<TResult>> method = (T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) =>
            {
                wasCalled = true;
                return Task.FromResult(new TResult());
            };
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            T4 input4 = default(T4);
            T5 input5 = default(T5);
            bool ignoreException = false;

            // Act
            var result = TaskRunner.RunSynchonously(method, input1, input2, input3, input4, input5, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 6 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_6Input_NullMethod_Throws_Test()
        {
            // Arrange
            Func<T1, T2, T3, T4, T5, T6, Task<TResult>> method = null;
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            T4 input4 = default(T4);
            T5 input5 = default(T5);
            T6 input6 = default(T6);
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, input3, input4, input5, input6, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_6Input_MethodWasCalled_Test()
        {
            // Arrange
            bool wasCalled = false;
            Func<T1, T2, T3, T4, T5, T6, Task<TResult>> method = (T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) =>
            {
                wasCalled = true;
                return Task.FromResult(new TResult());
            };
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            T4 input4 = default(T4);
            T5 input5 = default(T5);
            T6 input6 = default(T6);
            bool ignoreException = false;

            // Act
            var result = TaskRunner.RunSynchonously(method, input1, input2, input3, input4, input5, input6, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        // 7 params
        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_7Input_NullMethod_Throws_Test()
        {
            // Arrange
            Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> method = null;
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            T4 input4 = default(T4);
            T5 input5 = default(T5);
            T6 input6 = default(T6);
            T7 input7 = default(T7);
            bool ignoreException = false;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                TaskRunner.RunSynchonously(method, input1, input2, input3, input4, input5, input6, input7, ignoreException);
            });
        }

        [TestMethod]
        public void TaskRunner_RunSynchonously_TaskTResult_7Input_MethodWasCalled_Test()
        {
            // Arrange
            bool wasCalled = false;
            Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> method = (T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) =>
            {
                wasCalled = true;
                return Task.FromResult(new TResult());
            };
            T1 input1 = default(T1);
            T2 input2 = default(T2);
            T3 input3 = default(T3);
            T4 input4 = default(T4);
            T5 input5 = default(T5);
            T6 input6 = default(T6);
            T7 input7 = default(T7);
            bool ignoreException = false;

            // Act
            var result = TaskRunner.RunSynchonously(method, input1, input2, input3, input4, input5, input6, input7, ignoreException);

            // Assert
            Assert.IsTrue(wasCalled);
        }
        #endregion
    }
}