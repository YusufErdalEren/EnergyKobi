using EnergyKobiPL.DBContext;
using EnergyKobiPL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EnergyKobiPL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerRequest(RegisterRequestModel model)
        {
            using (TestElectricityKobiEntities db = new TestElectricityKobiEntities())
            {
                //ViewBag.IsModalOpen = "false";
                TempData["IsModalOpen"] = "false";
                string phoneNumber = string.Empty;
                //string olarak gelen değeri, Decimal değer tipine dönüştürdük.
                //model.AverageElectricityBillDecimal = Convert.ToDecimal(model.AverageElectricityBill);
                if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                    phoneNumber = new String(model.PhoneNumber.Where(Char.IsDigit).ToArray());

                var customerRequest = new CustomerRequest
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    PhoneNumber = phoneNumber,
                    CompanyName = string.Empty,
                    SubscriberGroupId = 4,
                    AverageElectricityBill = default(decimal),
                    Email = string.IsNullOrWhiteSpace(model.Email) ? string.Empty : model.Email
                };

                db.CustomerRequests.Add(customerRequest);
                db.SaveChanges();
                TempData["IsModalOpen"] = "true";

                //var customerRequestId = customerRequest.Id;

                //var fileList = new List<HttpPostedFileBase>();
                //fileList.Add(model.Attachment1);
                //fileList.Add(model.Attachment2);

                //foreach (var file in fileList)
                //{
                //    byte[] billDocumentBinary = null;

                //    if (file != null)
                //    {
                //        try
                //        {
                //            using (Stream inputStream = file.InputStream)
                //            {
                //                MemoryStream memoryStream = inputStream as MemoryStream;
                //                if (memoryStream == null)
                //                {
                //                    memoryStream = new MemoryStream();
                //                    inputStream.CopyTo(memoryStream);
                //                }
                //                billDocumentBinary = memoryStream.ToArray();
                //            }

                //            db.BillDocuments.Add(new BillDocument { FileName = file.FileName, FileBinary = billDocumentBinary, CustomerRequestId = customerRequestId });
                //            db.SaveChanges();
                //            TempData["IsModalOpen"] = "true";
                //        }
                //        catch (Exception ex)
                //        {
                //            return RedirectToAction("Index", "Home");
                //        }
                //    }
                //}

                SendMail(phoneNumber, model.Email, null, string.Empty, null, string.Empty);

                return RedirectToAction("Index", "Home");
            }
        }

        private void SendMail(string phoneNumber, string emailAddress, Stream file1 = null, string file1Name = "", Stream file2 = null, string file2Name = "")
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential("homeelectrictestuser@gmail.com", "ab12*cd34");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage mail = new MailMessage();

            mail.Subject = "Yeni Müşteri Talebi"; // mail konusu yazılır
            var bodyString = new StringBuilder();
            bodyString.Append(string.IsNullOrWhiteSpace(phoneNumber) ? string.Empty : $"{DateTime.Now} tarihinde iletişim bilgisi Phone Number: {phoneNumber} ");
            bodyString.Append(string.IsNullOrWhiteSpace(emailAddress) ? string.Empty : string.IsNullOrWhiteSpace(phoneNumber) ? $"{DateTime.Now} tarihinde iletişim bilgisi EmailAddress: {emailAddress} " : $"EmailAddress: { emailAddress} ");
            bodyString.Append("olan müşteri talebi oluşmuştur.");
            //bodyString.Append("Fatura bilgileri ek'te yer almaktadır.");
            mail.Body = bodyString.ToString();
            //mail.Attachments.Add(new Attachment(file1, file1Name));
            //mail.Attachments.Add(new Attachment(file2, file2Name));
            mail.From = new MailAddress("homeelectrictestuser@gmail.com", "EnergyKobi"); // display ismi yazılır

            TestElectricityKobiEntities db = new TestElectricityKobiEntities();

            foreach (var email in db.SendEmailAddresses.ToList())
            {
                if (email.IsCC.HasValue && !email.IsCC.Value)
                    mail.To.Add(new MailAddress(email.EmailAddress));
                else
                    mail.CC.Add(new MailAddress(email.EmailAddress));
            }

            //Mail gönderimi yapılır
            smtpClient.Send(mail);
        }
    }
}