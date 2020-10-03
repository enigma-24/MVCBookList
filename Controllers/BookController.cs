using System.Threading.Tasks;
using BookListMVC.Data;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BookController : Controller
    {

        private readonly BookListDbContext context;

        public BookController(BookListDbContext _context) => context = _context;

        [BindProperty]
        public Book Book { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Book book = new Book();
            if (id == null)
            {
                //create
                return View(book);
            }
            //update
            Book = context.Books.Find(id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(){
            if(ModelState.IsValid){
                if(Book.ID == 0){
                    //create
                    context.Books.Add(Book);
                }else{
                    //update
                    context.Books.Update(Book);
                }
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await context.Books.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookToDelete = await context.Books.FirstOrDefaultAsync(b => b.ID == id);
            if (bookToDelete != null)
            {
                context.Books.Remove(bookToDelete);
                await context.SaveChangesAsync();
                return Json(new { success = true, message = "Book Deleted!" });
            }
            else
            {
                return Json(new { success = false, message = "Error while deleting Book!" });
            }
        }
        #endregion
    }
}