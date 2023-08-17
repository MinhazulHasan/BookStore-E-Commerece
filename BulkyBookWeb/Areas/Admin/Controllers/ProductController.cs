using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment; // for accessing wwwroot folder
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem() {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(u => new SelectListItem() {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
                return View(productVM);
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if(obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if(obj.Product.Id != 0)
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product has been created successfully.";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }



        // POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (productFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, productFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productFromDb);
            _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Suyccessfull" });
        }


        #endregion

    }
}

/** 
IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem() {
    Text = u.Name,
    Value = u.Id.ToString()
});
IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(u => new SelectListItem {
    Text = u.Name,
    Value = u.Id.ToString()
});

ViewBag.CategoryList = CategoryList;
ViewData["CoverTypeList"] = CoverTypeList;
*/
