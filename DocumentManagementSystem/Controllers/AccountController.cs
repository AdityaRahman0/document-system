using DocumentManagementSystem.ViewModels;
using Entities;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

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
                User user = accountService.GetValidUser(connectionString, model.UserName, model.Password);

                if (user != null)
                {
                    Session["UserName"] = user.UserName;
                    Session["Role"] = user.Role;
                    Session["Id"] = user.UserId;
                    string jsonString = new JavaScriptSerializer().Serialize(user);
                    FormsAuthentication.SetAuthCookie(jsonString, false);
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
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            AccountService accountService = new AccountService();
            var departments = accountService.GetDepartments(connectionString);
            var model = new RegisterViewModel
            {
                Roles = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Requestor", Text = "Requestor" },
                    new SelectListItem { Value = "Manager", Text = "Manager" }
                },
                Departments = departments
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            AccountService accountService = new AccountService();
            if (ModelState.IsValid)
            {
                User user = new User 
                {
                    dtmUpd = DateTime.Now,
                    Email = model.Email,
                    Password = model.Password,
                    Role = model.Role,
                    UserName = model.UserName,
                    usrUpd = model.UserName,
                    DepartmentId = model.SelectedDepartments
                };
                accountService.RegisterUser(connectionString, user);
                return RedirectToAction("Login");
            }

            // Repopulate roles in case of validation errors
            model.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Requestor", Text = "Requestor" },
                new SelectListItem { Value = "Manager", Text = "Manager" }
            };
            return View(model);
        }
    }
}