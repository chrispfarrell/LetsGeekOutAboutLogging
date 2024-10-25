using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SynergisticLogging.Web.Data;
using SynergisticLogging.Web.External;
using SynergisticLogging.Web.Logging.ActionLogger;

namespace SynergisticLogging.Web.Controllers
{
    [LoggingActionFilter]
    public class DadJokesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDadJokesApiService _dadJokesApiService;
        public DadJokesController(ApplicationDbContext context, IDadJokesApiService dadJokesApiService)
        {
            _context = context;
            _dadJokesApiService = dadJokesApiService;
        }

        // GET: DadJokes
        public async Task<IActionResult> Index()
        {
              return _context.DadJokes != null ? 
                          View(await _context.DadJokes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DadJokes'  is null.");
        }

        // GET: DadJokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DadJokes == null)
            {
                return NotFound();
            }

            var dadJoke = await _context.DadJokes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dadJoke == null)
            {
                return NotFound();
            }

            return View(dadJoke);
        }

        // GET: DadJokes/Create
        public IActionResult Create()
        {
            var joke = new DadJoke();
            return View();
        }

        public async Task<IActionResult> GetRandom()
        {
            var joke = await _dadJokesApiService.GetRandom();
            _context.Add(joke);
            _context.SaveChanges();

            return _context.DadJokes != null ?
                          View("Index",await _context.DadJokes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DadJokes'  is null.");
        }
        // POST: DadJokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Setup,Punchline")] DadJoke dadJoke)
        {
            if (ModelState.IsValid)
            {
                dadJoke.CreatedDate = DateTime.Now;
                _context.Add(dadJoke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dadJoke);
        }

        // GET: DadJokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DadJokes == null)
            {
                return NotFound();
            }

            var dadJoke = await _context.DadJokes.FindAsync(id);
            if (dadJoke == null)
            {
                return NotFound();
            }
            return View(dadJoke);
        }

        // POST: DadJokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Setup,Punchline,CreatedDate")] DadJoke dadJoke)
        {
            if (id != dadJoke.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dadJoke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DadJokeExists(dadJoke.Id))
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
            return View(dadJoke);
        }

        // GET: DadJokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DadJokes == null)
            {
                return NotFound();
            }

            var dadJoke = await _context.DadJokes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dadJoke == null)
            {
                return NotFound();
            }

            return View(dadJoke);
        }

        // POST: DadJokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DadJokes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DadJokes'  is null.");
            }
            var dadJoke = await _context.DadJokes.FindAsync(id);
            if (dadJoke != null)
            {
                _context.DadJokes.Remove(dadJoke);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DadJokeExists(int id)
        {
          return (_context.DadJokes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
