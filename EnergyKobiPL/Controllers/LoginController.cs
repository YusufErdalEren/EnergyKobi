using EnergyKobiPL.DBContext;
using EnergyKobiPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnergyKobiPL.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            using (TestElectricityKobiEntities db = new TestElectricityKobiEntities())
            {
                var customer = db.Customers.FirstOrDefault(x => (x.Email == model.UserName || x.UserName == model.UserName) && x.Password == model.Password);

                if (customer != null)
                    return RedirectToAction("Index", "Home");
                else
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı. Kullanıcı adı ya da şifre yanlış.");
                    return View(model);
                }

            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            using (TestElectricityKobiEntities db = new TestElectricityKobiEntities())
            {
                try
                {
                    db.Customers.Add(new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Password = model.Password,
                        Email = model.Email,
                        ConfirmEmail = model.Email,
                        PhoneNumber = model.PhoneNumber
                    });

                    db.SaveChanges();

                    return RedirectToAction("Login", "Login");
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}