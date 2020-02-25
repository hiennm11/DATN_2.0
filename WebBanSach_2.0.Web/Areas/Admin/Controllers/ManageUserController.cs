using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUserController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork(new Data.WebBanSach_2_0DbContext());
        // GET: Admin/ManageUser
        public ActionResult Index()
        {
            var list = _unitOfWork.ApplicationUser.GetAll().Where(m => m.Id != "ad3b7c5f-fbae-4c8a-a1ea-bc7f89db2860");
            return View(list);
        }

        public ActionResult Edit(string Id)
        {
            ApplicationUser model = _unitOfWork.ApplicationUser.GetSingleByStringID(Id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser model)
        {
            try
            {
                _unitOfWork.ApplicationUser.Update(model);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public ActionResult EditRole(string Id)
        {
            ApplicationUser model = _unitOfWork.ApplicationUser.GetSingleByStringID(Id);
            ViewBag.RoleId = new SelectList(_unitOfWork.IdentityRole.GetAll().ToList().Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToRole(string UserId, string[] RoleId)
        {
            ApplicationUser model = _unitOfWork.ApplicationUser.GetSingleByStringID(UserId);
            if (RoleId != null && RoleId.Count() > 0)
            {
                foreach (string item in RoleId)
                {
                    IdentityRole role = _unitOfWork.IdentityRole.GetSingleByStringID(item);
                    model.Roles.Add(new IdentityUserRole() { UserId = UserId, RoleId = item });
                }
                _unitOfWork.Save();
            }

            ViewBag.RoleId = new SelectList(_unitOfWork.IdentityRole.GetAll().ToList().Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return RedirectToAction("EditRole", new { Id = UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleFromUser(string UserId, string RoleId)
        {
            ApplicationUser model = _unitOfWork.ApplicationUser.GetSingleByStringID(UserId);
            model.Roles.Remove(model.Roles.Single(m => m.RoleId == RoleId));
            _unitOfWork.Save();
            ViewBag.RoleId = new SelectList(_unitOfWork.IdentityRole.GetAll().ToList().Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return RedirectToAction("EditRole", new { Id = UserId });
        }
        public ActionResult Delete(string Id)
        {
            var model = _unitOfWork.ApplicationUser.GetSingleByStringID(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string Id)
        {
            ApplicationUser model = null;
            try
            {
                _unitOfWork.ApplicationUser.Delete(Id);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Delete", model);
            }
        }
    }
}