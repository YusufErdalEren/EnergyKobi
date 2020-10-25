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

                byte[] billDocumentBinary;
                string guidID = Guid.NewGuid().ToString();

                if (model != null && model.ElectricityBillDocument != null)
                {
                    try
                    {
                        using (Stream inputStream = model.ElectricityBillDocument.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            billDocumentBinary = memoryStream.ToArray();
                        }

                        db.BillDocuments.Add(new BillDocument { Id = guidID, FileName = model.ElectricityBillDocument.FileName, FileBinary = billDocumentBinary });
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    db.CustomerRequests.Add(new CustomerRequest
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        CompanyName = model.CompanyName,
                        SubscriberGroupId = model.SubscriberGroupId,
                        BillDocumentId = guidID
                    });
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");
            }
        }
    }
}