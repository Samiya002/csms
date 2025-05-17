using csms.Data;
using csms.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace csms.Controllers
{
    public class CoffeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoffeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Coffee
        public async Task<IActionResult> Index()
        {
            var coffees = await _context.Coffees.ToListAsync();
            return View(coffees);
        }

        // GET: Coffee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coffee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Quantity")] Coffee coffee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coffee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coffee);
        }

        // GET: Coffee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.Coffees.FindAsync(id);
            if (coffee == null)
            {
                return NotFound();
            }

            return View(coffee);
        }

        // POST: Coffee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Coffee coffee)
        {
            if (id != coffee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coffee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoffeeExists(coffee.Id))
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
            return View(coffee);
        }

        // GET: Coffee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.Coffees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coffee == null)
            {
                return NotFound();
            }

            return View(coffee);
        }

        // POST: Coffee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coffee = await _context.Coffees.FindAsync(id);
            if (coffee != null)
            {
                _context.Coffees.Remove(coffee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Menu()
        {
            var coffees = await _context.Coffees
                .Take(200)
                .ToListAsync();
            return View(coffees);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Order(int CoffeeId, int Quantity)
        {
            var coffee = await _context.Coffees.FindAsync(CoffeeId);
            if (coffee == null || Quantity > coffee.Quantity)
            {
                return NotFound();
            }

            var userId = User.Identity?.Name ?? "Guest";

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        CoffeeId = CoffeeId,
                        Quantity = Quantity
                    }
                }
            };

            coffee.Quantity -= Quantity;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Menu));
        }

        [AllowAnonymous]
        public async Task<IActionResult> OrderProgress()
        {
            var userId = User.Identity?.Name;
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Coffee)
                .ToListAsync();

            return View(orders);
        }

        private bool CoffeeExists(int id)
        {
            return _context.Coffees.Any(e => e.Id == id);
        }
    }
}