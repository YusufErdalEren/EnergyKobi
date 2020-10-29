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
        public decimal AverageElectricityBillDecimal { get; set; }
        public string AverageElectricityBill { get; set; }
        public int SubscriberGroupId { get; set; }
        public HttpPostedFileBase Attachment1 { get; set; }
        public HttpPostedFileBase Attachment2 { get; set; }
    }
}