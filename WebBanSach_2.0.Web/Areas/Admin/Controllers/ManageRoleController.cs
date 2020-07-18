using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Interfaces;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ManageRoleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityRoleRepository _identityRoleRepository;

        public ManageRoleController(IUnitOfWork unitOfWork, IIdentityRoleRepository identityRoleRepository)
        {
            this._unitOfWork = unitOfWork;
            this._identityRoleRepository = identityRoleRepository;
        }

        // GET: Admin/ManageRole
        public async Task<ActionResult> Index()
        {
            var model = await _identityRoleRepository.GetAllAsync();
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
                    await _identityRoleRepository.AddAsync(role);
                    await _unitOfWork.SaveAsync();
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