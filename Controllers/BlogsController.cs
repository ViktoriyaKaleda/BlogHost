using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogH.Models;
using BlogHosting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace BlogHosting.Controllers
{
    public class BlogsController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private IHostingEnvironment _appEnvironment;

		public BlogsController(
				ApplicationDbContext context,
				UserManager<IdentityUser> userManager,
				IHostingEnvironment appEnvironment
			)
		{
			_context = context;
			_userManager = userManager;
			_appEnvironment = appEnvironment;
		}

		// GET: Blogs
		public async Task<IActionResult> Index()
        {
            return View(await _context.Blog.Include(m => m.Author).OrderByDescending(m => m.CreatedDateTime).ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var blog = await _context.Blog
				.Include(m => m.Posts).ThenInclude(m => m.Comments)
				.Include(m => m.Posts).ThenInclude(m => m.Likes)
				.Include(m => m.Posts).ThenInclude(m => m.Author)
				.SingleOrDefaultAsync(m => m.BlogId == id);

			if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,BlogName,Description,CreatedDateTime,UpdatedDateTime")] Blog blog)
        {
            if (ModelState.IsValid)
            {
				blog.Author = await _userManager.GetUserAsync(HttpContext.User);
				blog.CreatedDateTime = DateTime.Now;
				blog.UpdatedDateTime = blog.CreatedDateTime;

				_context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,BlogName,Description,CreatedDateTime,UpdatedDateTime")] Blog blog)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
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
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blog.FindAsync(id);
            _context.Blog.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blog.Any(e => e.BlogId == id);
        }
    }
}
