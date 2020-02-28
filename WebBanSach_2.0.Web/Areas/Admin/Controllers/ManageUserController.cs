using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            var list = await _unitOfWork.ApplicationUser.GetAll();
            
            return View(list.Where(m => m.Id != "ad3b7c5f-fbae-4c8a-a1ea-bc7f89db2860"));
        }

        public async Task<ActionResult> Edit(string Id)
        {
            ApplicationUser model = await _unitOfWork.ApplicationUser.GetSingleByStringID(Id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser model)
        {
            try
            {
                _unitOfWork.ApplicationUser.Update(model);
                await _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<ActionResult> EditRole(string Id)
        {
            ApplicationUser model = await _unitOfWork.ApplicationUser.GetSingleByStringID(Id);
            var list = await _unitOfWork.IdentityRole.GetAll();
            ViewBag.RoleId = new SelectList(list.Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddToRole(string UserId, string[] RoleId)
        {
            ApplicationUser model = await _unitOfWork.ApplicationUser.GetSingleByStringID(UserId);
            if (RoleId != null && RoleId.Count() > 0)
            {
                foreach (string item in RoleId)
                {
                    IdentityRole role = await _unitOfWork.IdentityRole.GetSingleByStringID(item);
                    model.Roles.Add(new IdentityUserRole() { UserId = UserId, RoleId = item });
                }
                await _unitOfWork.Save();
            }
            var list = await _unitOfWork.IdentityRole.GetAll();
            ViewBag.RoleId = new SelectList(list.Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return RedirectToAction("EditRole", new { Id = UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleFromUser(string UserId, string RoleId)
        {
            ApplicationUser model = await _unitOfWork.ApplicationUser.GetSingleByStringID(UserId);
            model.Roles.Remove(model.Roles.Single(m => m.RoleId == RoleId));
            await _unitOfWork.Save();
            var list = await _unitOfWork.IdentityRole.GetAll();
            ViewBag.RoleId = new SelectList(list.Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return RedirectToAction("EditRole", new { Id = UserId });
        }
        public async Task<ActionResult> Delete(string Id)
        {
            var model = await _unitOfWork.ApplicationUser.GetSingleByStringID(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string Id)
        {
            ApplicationUser model = null;
            try
            {
                _unitOfWork.ApplicationUser.Delete(Id);
                await _unitOfWork.Save();
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