using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testingassignment2.Models;

namespace testingassignment2.Controllers
{
    public class CarRentalController : Controller
    {
        private readonly AppDbContext _context;

        public CarRentalController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rentals = _context.CarRentals
                .Include(r => r.Car)
                .OrderBy(r => r.StartDate)
                .ToList();

            return View(rentals);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateCars();

            var model = new CarRental
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CarRental rental)
        {
            if (rental.EndDate < rental.StartDate)
            {
                ModelState.AddModelError(nameof(rental.EndDate), "Return date must be after the pickup date.");
            }

            if (!ModelState.IsValid)
            {
                PopulateCars(rental.CarId);
                return View(rental);
            }

            _context.CarRentals.Add(rental);
            _context.SaveChanges();

            var selectedCar = _context.Cars.FirstOrDefault(c => c.Id == rental.CarId);
            ViewBag.Car = selectedCar;

            return View("Confirmation", rental);
        }

        private void PopulateCars(int? selectedId = null)
        {
            var carOptions = _context.Cars
                .Where(c => c.Available)
                .OrderBy(c => c.Model)
                .Select(c => new
                {
                    c.Id,
                    Name = $"{c.Model} ({c.RegistrationNumber})"
                })
                .ToList();

            ViewBag.Cars = new SelectList(carOptions, "Id", "Name", selectedId);
        }
    }
}
