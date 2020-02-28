using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data;
using WebBanSach_2_0.Data.Infrastructure;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageRoleController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork(new WebBanSach_2_0DbContext());
        // GET: Admin/ManageRole
        public async Task<ActionResult> Index()
        {
            var model = await _unitOfWork.IdentityRole.GetAll();
            return View(model.Where(m => m.Id != "ad"));
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.IdentityRole.Add(role);
                    await _unitOfWork.Save();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(role);
        }
    }
}