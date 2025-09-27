using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testingassignment2.Controllers;
using testingassignment2.Models;
using Xunit;

namespace testingassignment2.Tests
{
    public class CarsControllerTests
    {
        private AppDbContext GetDbContext()
        {
            
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var ctx = new AppDbContext(options);
            ctx.Database.EnsureCreated();
            return ctx;
        }

        private Car MakeValidCar(string reg = "ABC123") => new Car
        {
            RegistrationNumber = reg,
            Model = "Corolla",
            Capacity = 4,
            RatePerDay = 80m,
            Status = "Good",
            Available = true
        };

        [Fact]
        public async Task Index_Returns_View_With_ListOfCars()
        {
            using var ctx = GetDbContext();
            ctx.Cars.AddRange(MakeValidCar("AAA111"), MakeValidCar("BBB222"));
            await ctx.SaveChangesAsync();

            var controller = new CarsController(ctx);

            var result = await controller.Index();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Car>>(view.Model);
            Assert.Equal(2, model.Count()); 
        }

        [Fact]
        public async Task Create_Post_Valid_Redirects_To_Index_And_Saves()
        {
            using var ctx = GetDbContext();
            var controller = new CarsController(ctx);
            var car = MakeValidCar("NEW123");

            var result = await controller.Create(car);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Single(ctx.Cars); 
        }

        [Fact]
        public async Task Create_Post_Invalid_Returns_View()
        {
            using var ctx = GetDbContext();
            var controller = new CarsController(ctx);

            
            controller.ModelState.AddModelError("RatePerDay", "Invalid");

            var car = MakeValidCar("BAD123");

            var result = await controller.Create(car);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(car, view.Model);
            Assert.Empty(ctx.Cars); 
        }
    }
}
