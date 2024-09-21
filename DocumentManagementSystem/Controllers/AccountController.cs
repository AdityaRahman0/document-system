using DocumentManagementSystem.ViewModels;
using Entities;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        private string connectionString = Helper.ConnectionString.connectionString;

        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel model)
        {
            AccountService accountService = new AccountService();
            if (ModelState.IsValid)
            {
                User user = accountService.getValidUser(connectionString, model.UserName, model.Password);

                if (user != null)
                {
                    Session["UserName"] = user.UserName;
                    Session["Role"] = user.Role;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Login gagal!";
                    return View();
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Requestor", Text = "Requestor" },
                new SelectListItem { Value = "Approver", Text = "Approver" },
                new SelectListItem { Value = "DCC", Text = "DCC" },
                new SelectListItem { Value = "QA Manager", Text = "QA Manager" }
            }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Login");
            }

            // Repopulate roles in case of validation errors
            model.Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "Requestor", Text = "Requestor" },
            new SelectListItem { Value = "Approver", Text = "Approver" },
            new SelectListItem { Value = "DCC", Text = "DCC" },
            new SelectListItem { Value = "QA Manager", Text = "QA Manager" }
        };

            return View(model);
        }
    }
}