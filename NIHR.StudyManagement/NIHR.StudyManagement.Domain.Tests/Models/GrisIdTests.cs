using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Models;
using System;

namespace NIHR.StudyManagement.Domain.Tests.Models
{
    [TestClass]
    public class GrisIdTests
    {
        [TestMethod]
        [DataRow(123456246, "GRIS-123 456 246")]
        [DataRow(377925334, "GRIS-377 925 334")]
        [DataRow(717774392, "GRIS-717 774 392")]
        [DataRow(675770416, "GRIS-675 770 416")]
        public void ConstructorSuccessScenarios(long sequence,
            string expectedDisplayValue)
        {
            var grisId = new GrisId(sequence);

            Assert.AreEqual(expectedDisplayValue, grisId.DisplayValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow(123456247)]
        [DataRow(377925333)]
        [DataRow(717774391)]
        [DataRow(675770415)]
        public void ConstructorFailScenarios(long sequence)
        {
            var grisId = new GrisId(sequence);
        }

        [TestMethod]
        [DataRow(123456, 24, "GRIS-123 456 246")]
        [DataRow(377925, 33, "GRIS-377 925 334")]
        [DataRow(717774, 39, "GRIS-717 774 392")]
        [DataRow(675770, 41, "GRIS-675 770 416")]
        public void GenerateNewGrisIdScenarios(int randomSequence, int currentYear, string expectedDisplayValue)
        {
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            randomNumberGeneratorMock.SetupGet(mock => mock.CurrentYear).Returns(currentYear);
            randomNumberGeneratorMock.Setup(mock => mock.GetRandomNumber()).Returns(randomSequence);

            var grisId = GrisId.GenerateNewGrisId(randomNumberGeneratorMock.Object);

            Assert.AreEqual(expectedDisplayValue, grisId.DisplayValue);
        }

        [TestMethod]
        [DataRow(123456246, true)]
        [DataRow(938100, true)]
        [DataRow(377925334, true)]
        [DataRow(717774392, true)]
        [DataRow(234241883, true)]
        [DataRow(909226763, true)]
        [DataRow(748665551, true)]
        [DataRow(675770416, true)]
        [DataRow(149084253, true)]
        [DataRow(881465207, true)]
        [DataRow(661289850, true)]
        [DataRow(377925333, false)]
        [DataRow(717774391, false)]
        [DataRow(234241882, false)]
        [DataRow(909226762, false)]
        [DataRow(748665550, false)]
        [DataRow(675770415, false)]
        [DataRow(149084252, false)]
        [DataRow(881465206, false)]
        [DataRow(661289859, false)]
        [DataRow(0, false)]
        [DataRow(999999, false)]
        [DataRow(999999221, false)]
        [DataRow(999999220, false)]
        [DataRow(899999247, true)]
        public void IsValidGrisId(long sequence,
            bool expectedIsValid)
        {
            Assert.AreEqual(expectedIsValid, GrisId.IsValidGrisId(sequence));
        }
    }
}
