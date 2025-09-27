using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using testingassignment2.Models;
using Xunit;

namespace testingassignment2.Tests
{
    public class CarModelValidationTests
    {
        private static IList<ValidationResult> Validate(object obj)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, ctx, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void ValidCar_PassesValidation()
        {
            var car = new Car
            {
                RegistrationNumber = "ABC123",
                Model = "Corolla",
                Capacity = 5,
                RatePerDay = 50m,
                Status = "Good",
                Available = true
            };

            var results = Validate(car);
            Assert.Empty(results);
        }

        [Fact]
        public void NegativeRate_FailsValidation()
        {
            var car = new Car
            {
                RegistrationNumber = "ABC123",
                Model = "Corolla",
                Capacity = 5,
                RatePerDay = -1m,
                Status = "Good",
                Available = true
            };

            var results = Validate(car);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Car.RatePerDay)));
        }

        [Fact]
        public void ZeroCapacity_FailsValidation()
        {
            var car = new Car
            {
                RegistrationNumber = "ABC123",
                Model = "Corolla",
                Capacity = 0,
                RatePerDay = 50m,
                Status = "Good",
                Available = true
            };

            var results = Validate(car);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Car.Capacity)));
        }

        [Fact]
        public void MissingRegistrationNumber_FailsValidation()
        {
            var car = new Car
            {
                RegistrationNumber = null,
                Model = "Corolla",
                Capacity = 4,
                RatePerDay = 50m,
                Status = "Good",
                Available = false
            };

            var results = Validate(car);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Car.RegistrationNumber)));
        }

        [Fact]
        public void InvalidStatus_FailsValidation()
        {
            var car = new Car
            {
                RegistrationNumber = "ZZZ999",
                Model = "Civic",
                Capacity = 4,
                RatePerDay = 60m,
                Status = "Excellent", // not in Good|Fair|Poor
                Available = true
            };

            var results = Validate(car);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Car.Status)));
        }
    }
}
