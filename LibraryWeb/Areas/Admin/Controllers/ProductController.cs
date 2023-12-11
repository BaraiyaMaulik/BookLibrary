using Library.DataAccess.Data;
using Library.DataAccess.Repository;
using Library.DataAccess.Repository.IRepository;
using Library.Models;
using Library.Models.ViewModels;
using Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        /*private readonly ApplicationDbContext _db;
         private readonly ICategoryRepository _categoryRepo;*/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /*public CategoryController(ApplicationDbContext db)
        public CategoryController(ICategoryRepository db)*/
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            //db=_db;
            //_categoryRepo = db;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            /*List<Category> objCategoryList = _db.Categories.ToList();
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();*/
            /* Below two lines is commented because this data is fetch through GetAll api and in product.js file its is mapped
              List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProductList);*/
            return View();
        }

        //public IActionResult Create(int? id)
        public IActionResult Upsert(int? id)
        {
            /*Using projection in EF
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            //ViewBag.CategoryList = CategoryList;
            ViewData["CategoryList"] = CategoryList;
            return View("Create");*/

            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }

        [HttpPost]
        // public IActionResult Create(ProductVM productVm)
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            /*Custom Validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match name");
            }*/
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images/product");

                    if (!String.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    /* _db.Categories.Add(obj);
                       _categoryRepo.Add(obj);*/
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                /* _db.SaveChanges();
                _categoryRepo.Save();*/
                _unitOfWork.Save();
                TempData["success"] = "Product Created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        /*Also remove Delete View*/
        /*public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View("Delete", productFromDb);
        }*/

        /* [HttpPost, ActionName("Delete")]
         public IActionResult DeletePOST(int? id)
         {
             //Category? obj = _db.Categories.Find(id);
             //Category? obj = _categoryRepo.Get(u => u.Id == id);
             Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
             if (obj == null)
             {
                 return NotFound();
             }
             //_db.Categories.Remove(obj);
             //_categoryRepo.Remove(obj);
             _unitOfWork.Product.Remove(obj);
             //_db.SaveChanges();
             //_categoryRepo.Save();
             _unitOfWork.Save();
             TempData["success"] = "Product Deleted successfully";
             return RedirectToAction("Index", "Product");
         }*/

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { Data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            //delete the old image
            var oldImagePath =
                Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }
}
