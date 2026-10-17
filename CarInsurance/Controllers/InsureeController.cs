
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarInsurance.Models;
using CarInsurance.Data;
namespace CarInsurance.Controllers;

public class InsureeController : Controller
{
    private readonly ApplicationDbContext _context;

    public InsureeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: INSUREES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Insurees.ToListAsync());
    }

    // GET: INSUREES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (insuree == null)
        {
            return NotFound();
        }

        return View(insuree);
    }

    // GET: INSUREES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: INSUREES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")]
    Insuree insuree)
    {
        // Quote starts at $50
        decimal quote = 50m;

        // Calculate customer's age
        DateTime today = DateTime.Today;
        int age = today.Year - insuree.DateOfBirth.Year;

        if (insuree.DateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        // Add price based on age
        if (age <= 18)
        {
            quote += 100m;
        }
        else if (age <= 25)
        {
            quote += 50m;
        }
        else
        {
            quote += 25m;
        }

        // Add price based on car year
        if (insuree.CarYear < 2000)
        {
            quote += 25m;
        }

        if (insuree.CarYear > 2015)
        {
            quote += 25m;
        }

        // Add price for Porsche
        if (string.Equals(insuree.CarMake?.Trim(), "Porsche",
            StringComparison.OrdinalIgnoreCase))
        {
            quote += 25m;

            // Extra charge for Porsche 911 Carrera
            if (string.Equals(insuree.CarModel?.Trim(), "911 Carrera",
                StringComparison.OrdinalIgnoreCase))
            {
                quote += 25m;
            }
        }

        // Add $10 for every speeding ticket
        quote += insuree.SpeedingTickets * 10m;

        // Add 25% for DUI
        if (insuree.DUI)
        {
            quote *= 1.25m;
        }

        // Add 50% for full coverage
        if (insuree.CoverageType)
        {
            quote *= 1.50m;
        }

        // Save the calculated quote
        insuree.Quote = Math.Round(quote, 2);

        if (ModelState.IsValid)
        {
            _context.Insurees.Add(insuree);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(insuree);
    }


    // GET: INSUREES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees.FindAsync(id);
        if (insuree == null)
        {
            return NotFound();
        }
        return View(insuree);
    }

    // POST: INSUREES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
    {
        if (id != insuree.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(insuree);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsureeExists(insuree.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(insuree);
    }

    // GET: INSUREES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (insuree == null)
        {
            return NotFound();
        }

        return View(insuree);
    }

    // POST: INSUREES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var insuree = await _context.Insurees.FindAsync(id);
        if (insuree != null)
        {
            _context.Insurees.Remove(insuree);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Show all insurance quotes to the admin
    public async Task<IActionResult> Admin()
    {
        // Get all customers from the database
        var insurees = await _context.Insurees.ToListAsync();

        // Send the customers to the Admin page
        return View(insurees);
    }

    private bool InsureeExists(int? id)
    {
        return _context.Insurees.Any(e => e.Id == id);
    }
}
