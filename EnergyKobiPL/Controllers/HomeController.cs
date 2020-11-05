using EnergyKobiPL.DBContext;
using EnergyKobiPL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                //string olarak gelen değeri, Decimal değer tipine dönüştürdük.
                model.AverageElectricityBillDecimal = Convert.ToDecimal(model.AverageElectricityBill);
                var phoneNumber = new String(model.PhoneNumber.Where(Char.IsDigit).ToArray());

                var customerRequest = new CustomerRequest
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = phoneNumber,
                    CompanyName = model.CompanyName,
                    SubscriberGroupId = model.SubscriberGroupId,
                    AverageElectricityBill = model.AverageElectricityBillDecimal
                };

                db.CustomerRequests.Add(customerRequest);
                db.SaveChanges();

                var customerRequestId = customerRequest.Id;

                var fileList = new List<HttpPostedFileBase>();
                fileList.Add(model.Attachment1);
                fileList.Add(model.Attachment2);

                foreach (var file in fileList)
                {
                    byte[] billDocumentBinary = null;

                    if (file != null)
                    {
                        try
                        {
                            using (Stream inputStream = file.InputStream)
                            {
                                MemoryStream memoryStream = inputStream as MemoryStream;
                                if (memoryStream == null)
                                {
                                    memoryStream = new MemoryStream();
                                    inputStream.CopyTo(memoryStream);
                                }
                                billDocumentBinary = memoryStream.ToArray();
                            }

                            db.BillDocuments.Add(new BillDocument { FileName = file.FileName, FileBinary = billDocumentBinary, CustomerRequestId = customerRequestId });
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                } 

                return RedirectToAction("Index", "Home");
            }
        }
    }
}