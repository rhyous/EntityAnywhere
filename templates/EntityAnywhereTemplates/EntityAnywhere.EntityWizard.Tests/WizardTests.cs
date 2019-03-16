using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EntityAnywhere.EntityWizard.Tests
{
    [TestClass]
    public class WizardTests
    {
        [TestMethod]
        public void Wizard_RunStarted_Test()
        {
            // Arrange
            var wizard = new Wizard();
            var mockStarter = new Mock<IStarter>();
            IDictionary<string, string> actualDictionary = null;
            mockStarter.Setup(m=>m.Start(It.IsAny<IDictionary<string,string>>()))
                .Callback((IDictionary<string, string> inDict)=> 
                {
                    actualDictionary = inDict;
                });
            var dictionary = new Dictionary<string, string>();
            wizard.Starter = mockStarter.Object;

            // Act
            wizard.RunStarted(null, dictionary, WizardRunKind.AsNewProject, null);

            // Assert
            mockStarter.Verify(m => m.Start(It.IsAny<IDictionary<string, string>>()), Times.Once);
            Assert.AreEqual(dictionary, actualDictionary);

        }
    }
}
