using Library.DataAccess.Data;
using Library.DataAccess.Repository;
using Library.DataAccess.Repository.IRepository;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        // private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        //public CategoryController(ApplicationDbContext db)
        //public CategoryController(ICategoryRepository db)
        public CategoryController(IUnitOfWork unitOfWork)
        {
            //db=_db;
            //_categoryRepo = db;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //List<Category> objCategoryList = _db.Categories.ToList();
            //List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //Custom Validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match name");
            }
            if (ModelState.IsValid)
            {
                // _db.Categories.Add(obj);
                //_categoryRepo.Add(obj);
                _unitOfWork.Category.Add(obj);

                // _db.SaveChanges();
                //_categoryRepo.Save();
                _unitOfWork.Save();
                TempData["success"] = "Category Created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View("Index");

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb = _categoryRepo.Get(u=>u.Id==id);

            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View("Edit", categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                //_db.Categories.Update(obj);
                //_categoryRepo.Update(obj);
                _unitOfWork.Category.Update(obj);

                //_db.SaveChanges();
                //_categoryRepo.Save();
                _unitOfWork.Save();
                TempData["success"] = "Category Updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View("Index");

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View("Delete", categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            //Category? obj = _db.Categories.Find(id);
            //Category? obj = _categoryRepo.Get(u => u.Id == id);
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            //_db.Categories.Remove(obj);
            //_categoryRepo.Remove(obj);
            _unitOfWork.Category.Remove(obj);
            //_db.SaveChanges();
            //_categoryRepo.Save();
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index", "Category");
        }

    }
}
