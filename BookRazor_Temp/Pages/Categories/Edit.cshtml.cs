using BookRazor_Temp.Data;
using BookRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category? CategoryEdit { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? Id)
        {
            if (Id != null && Id != 0)
            {
                CategoryEdit = _db.Categories.Find(Id);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(CategoryEdit);
                _db.SaveChanges();
                TempData["success"] = "Category Updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
