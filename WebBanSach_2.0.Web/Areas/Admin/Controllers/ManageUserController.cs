using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ManageUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IIdentityRoleRepository _identityRoleRepository;

        public ManageUserController(IUnitOfWork unitOfWork, IApplicationUserRepository applicationUserRepository, IIdentityRoleRepository identityRoleRepository)
        {
            this._unitOfWork = unitOfWork;
            this._applicationUserRepository = applicationUserRepository;
            this._identityRoleRepository = identityRoleRepository;
        }

        // GET: Admin/ManageUser
        public async Task<ActionResult> Index()
        {
            var list = await _applicationUserRepository.GetAllAsync();
            
            return View(list.Where(m => m.Id != "ad3b7c5f-fbae-4c8a-a1ea-bc7f89db2860"));
        }

        public async Task<ActionResult> Edit(string Id)
        {
            ApplicationUser model = await _applicationUserRepository.GetSingleByStringIDAsync(Id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser model)
        {
            try
            {
                await _applicationUserRepository.UpdateAsync(model);
                await _unitOfWork.SaveAsync();
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
            ApplicationUser model = await _applicationUserRepository.GetSingleByStringIDAsync(Id);
            var list = await _identityRoleRepository.GetAllAsync();
            ViewBag.RoleId = new SelectList(list.Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddToRole(string UserId, string[] RoleId)
        {
            ApplicationUser model = await _applicationUserRepository.GetSingleByStringIDAsync(UserId);
            if (RoleId != null && RoleId.Count() > 0)
            {
                foreach (string item in RoleId)
                {
                    IdentityRole role = await _identityRoleRepository.GetSingleByStringIDAsync(item);
                    model.Roles.Add(new IdentityUserRole() { UserId = UserId, RoleId = item });
                }
                await _unitOfWork.SaveAsync();
            }
            var list = await _identityRoleRepository.GetAllAsync();
            ViewBag.RoleId = new SelectList(list.Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return RedirectToAction("EditRole", new { Id = UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleFromUser(string UserId, string RoleId)
        {
            ApplicationUser model = await _applicationUserRepository.GetSingleByStringIDAsync(UserId);
            model.Roles.Remove(model.Roles.Single(m => m.RoleId == RoleId));
            await _unitOfWork.SaveAsync();
            var list = await _identityRoleRepository.GetAllAsync();
            ViewBag.RoleId = new SelectList(list.Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return RedirectToAction("EditRole", new { Id = UserId });
        }
        public async Task<ActionResult> Delete(string Id)
        {
            var model = await _applicationUserRepository.GetSingleByStringIDAsync(Id);
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
                _applicationUserRepository.Delete(Id);
                await _unitOfWork.SaveAsync();
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