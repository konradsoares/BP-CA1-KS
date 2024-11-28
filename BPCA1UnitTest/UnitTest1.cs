using NUnit.Framework;
using BPCalculator;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BPCA1UnitTest
{
    public class BloodPressureTests
    {
        private BloodPressure _bloodPressure = null!;   

        [SetUp]
        public void Setup()
        {
            _bloodPressure = new BloodPressure();
        }

        [TestCase(85, 55, BPCategory.Low, TestName = "BP Category - Low")]
        [TestCase(115, 75, BPCategory.Ideal, TestName = "BP Category - Ideal")]
        [TestCase(130, 85, BPCategory.PreHigh, TestName = "BP Category - Pre-High")]
        [TestCase(145, 95, BPCategory.High, TestName = "BP Category - High")]
        public void BPCategory_ShouldReturnCorrectCategory(int systolic, int diastolic, BPCategory expectedCategory)
        {
            // Arrange
            _bloodPressure.Systolic = systolic;
            _bloodPressure.Diastolic = diastolic;

            // Act
            var category = _bloodPressure.Category;

            // Assert
            Assert.That(category, Is.EqualTo(expectedCategory));
        }

        [Test]
        public void Systolic_OutOfRange_ShouldThrowValidationError()
        {
            // Arrange
            _bloodPressure.Systolic = 200; // Outside valid range
            _bloodPressure.Diastolic = 80;

            var context = new ValidationContext(_bloodPressure) { MemberName = nameof(_bloodPressure.Systolic) };
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(_bloodPressure.Systolic, context, validationResults);

            // Assert
            Assert.IsFalse(isValid, "Expected systolic out-of-range validation to fail.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("Invalid Systolic Value"));
        }

        [Test]
        public void Diastolic_OutOfRange_ShouldThrowValidationError()
        {
            // Arrange
            _bloodPressure.Systolic = 120;
            _bloodPressure.Diastolic = 110; // Outside valid range

            var context = new ValidationContext(_bloodPressure) { MemberName = nameof(_bloodPressure.Diastolic) };
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(_bloodPressure.Diastolic, context, validationResults);

            // Assert
            Assert.IsFalse(isValid, "Expected diastolic out-of-range validation to fail.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("Invalid Diastolic Value"));
        }
    }
}
