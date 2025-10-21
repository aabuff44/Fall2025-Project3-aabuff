using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_aabuff.Data;
using Fall2025_Project3_aabuff.Models;

namespace Fall2025_Project3_aabuff.Controllers
{
    public class Actor_MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Actor_MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Actor_Movie
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Actor_Movies.Include(a => a.Actor).Include(a => a.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Actor_Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor_Movie = await _context.Actor_Movies
                .Include(a => a.Actor)
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor_Movie == null)
            {
                return NotFound();
            }

            return View(actor_Movie);
        }

        // GET: Actor_Movie/Create
        public IActionResult Create()
        {
            Console.WriteLine("=== CREATE GET CALLED ===");

            var actors = _context.Actors.ToList();
            var movies = _context.Movies.ToList();

            Console.WriteLine($"Found {actors.Count} actors and {movies.Count} movies");

            ViewBag.ActorId = new SelectList(actors, "Id", "Name");
            ViewBag.MovieId = new SelectList(movies, "Id", "Title");

            return View();
        }

        // POST: Actor_Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)  // ← Changed parameter to IFormCollection
        {
            Console.WriteLine("=== CREATE POST CALLED ===");

            // Let's see what's actually in the form
            foreach (var key in form.Keys)
            {
                Console.WriteLine($"Form key: {key} = {form[key]}");
            }

            // Try to manually create the model
            var actorIdStr = form["ActorId"];
            var movieIdStr = form["MovieId"];

            Console.WriteLine($"Raw ActorId: {actorIdStr}");
            Console.WriteLine($"Raw MovieId: {movieIdStr}");

            if (int.TryParse(actorIdStr, out int actorId) && int.TryParse(movieIdStr, out int movieId))
            {
                Console.WriteLine($"Parsed ActorId: {actorId}, MovieId: {movieId}");

                // Check if relationship exists
                bool exists = await _context.Actor_Movies
                    .AnyAsync(am => am.ActorId == actorId && am.MovieId == movieId);

                if (exists)
                {
                    ModelState.AddModelError("", "This actor is already associated with this movie.");
                }
                else
                {
                    var actorMovie = new Actor_Movie { ActorId = actorId, MovieId = movieId };
                    _context.Actor_Movies.Add(actorMovie);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("SUCCESS: Relationship saved!");
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                Console.WriteLine("ERROR: Could not parse ActorId or MovieId");
                ModelState.AddModelError("", "Please select both an actor and a movie.");
            }

            // Reload dropdowns
            ViewBag.ActorId = new SelectList(_context.Actors, "Id", "Name");
            ViewBag.MovieId = new SelectList(_context.Movies, "Id", "Title");
            return View(new Actor_Movie());
        }

        // GET: Actor_Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor_Movie = await _context.Actor_Movies.FindAsync(id);
            if (actor_Movie == null)
            {
                return NotFound();
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", actor_Movie.ActorId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", actor_Movie.MovieId);
            return View(actor_Movie);
        }

        // POST: Actor_Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActorId,MovieId")] Actor_Movie actor_Movie)
        {
            if (id != actor_Movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor_Movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Actor_MovieExists(actor_Movie.Id))
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
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", actor_Movie.ActorId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", actor_Movie.MovieId);
            return View(actor_Movie);
        }

        // GET: Actor_Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor_Movie = await _context.Actor_Movies
                .Include(a => a.Actor)
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor_Movie == null)
            {
                return NotFound();
            }

            return View(actor_Movie);
        }

        // POST: Actor_Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor_Movie = await _context.Actor_Movies.FindAsync(id);
            if (actor_Movie != null)
            {
                _context.Actor_Movies.Remove(actor_Movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Actor_MovieExists(int id)
        {
            return _context.Actor_Movies.Any(e => e.Id == id);
        }
    }
}