using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnergyKobiPL.Models
{
    public class RegisterRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public decimal AverageElectricityBill { get; set; }
        public int SubscriberGroupId { get; set; }
        public HttpPostedFileBase ElectricityBillDocument { get; set; }
        public string BillDocumentId { get; set; }
    }
}