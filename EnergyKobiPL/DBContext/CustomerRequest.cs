//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnergyKobiPL.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustomerRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public decimal AverageElectricityBill { get; set; }
        public int SubscriberGroupId { get; set; }
        public string BillDocumentId { get; set; }
    
        public virtual BillDocument BillDocument1 { get; set; }
        public virtual SubscriberGroup SubscriberGroup { get; set; }
    }
}
