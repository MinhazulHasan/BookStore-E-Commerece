using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int id)
        {
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
                // create
                
                return View(productVM);
            }
            else
            {
                // update

            }
            return View(productVM);
        }


        // GET
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        //    if (coverTypeFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(coverTypeFromDb);
        //}

        //// POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(CoverType coverType)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.CoverType.Update(coverType);
        //        _unitOfWork.Save();
        //        TempData["success"] = "CoverType has been updated successfully.";
        //        return RedirectToAction("Index", "CoverType");
        //    }
        //    return View(coverType);
        //}


        //// GET
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        //    if (coverTypeFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(coverTypeFromDb);
        //}

        //// POST
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteCoverType(int? id)
        //{
        //    if (id == 0 || id == null)
        //    {
        //        return NotFound();
        //    }
        //    var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        //    if (coverTypeFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.CoverType.Remove(coverTypeFromDb);
        //    _unitOfWork.Save();
        //    TempData["success"] = "CoverType has been deleted successfully.";
        //    return RedirectToAction("Index", "CoverType");
        //}
    }
}
