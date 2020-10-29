using EnergyKobiPL.DBContext;
using EnergyKobiPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
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
                var customer = db.Customers.FirstOrDefault(x => (x.Email == model.UserName || x.UserName == model.UserName));

                if (customer != null && Crypto.VerifyHashedPassword(customer.Password, model.Password))
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
                    var hashPassword = Crypto.HashPassword(model.Password);

                    db.Customers.Add(new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Password = hashPassword,
                        Email = model.Email,
                        ConfirmEmail = model.Email,
                        PhoneNumber = model.PhoneNumber
                    });

                    db.SaveChanges();

                    #region Email Gönderimi

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                    smtpClient.Credentials = new System.Net.NetworkCredential("homeelectrictestuser@gmail.com", "ab12*cd34");
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;

                    MailMessage mail = new MailMessage();

                    mail.Subject = "Sisteme Yeni Üye Katıldı"; // mail konusu yazılır
                    mail.Body = $"{DateTime.Now} tarinde {model.Email} adresine sahip kullanıcı sisteme kaydolmuştur."; //mail içeriği yazılır
                    mail.From = new MailAddress("homeelectrictestuser@gmail.com", "EnergyKobi"); // display ismi yazılır

                    foreach (var email in db.SendEmailAddresses.ToList())
                    {
                        if (email.IsCC.HasValue && !email.IsCC.Value)
                            mail.To.Add(new MailAddress(email.EmailAddress));
                        else
                            mail.CC.Add(new MailAddress(email.EmailAddress));
                    }

                    //Mail gönderimi yapılır
                    smtpClient.Send(mail);

                    #endregion

                    return RedirectToAction("Login", "Login");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}