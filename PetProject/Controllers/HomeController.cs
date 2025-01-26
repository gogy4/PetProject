using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Data;
using PetProject.Models;
 
namespace PetProject.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext db;
        public HomeController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult Filter(string name)
        {
            var users = db.Users.ToList();
            
            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(p => p.Name!.Contains(name)).ToList();
            }

            var viewModel = new UserListModel
            {
                Users = users.ToList(),
                Name = name
            };
            
            return View(viewModel);
        }
 
        public async Task<IActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != null)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            return NotFound();
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != null)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                   return View(user);
                }
            }

            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}