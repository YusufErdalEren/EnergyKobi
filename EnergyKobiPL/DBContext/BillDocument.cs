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
    
    public partial class BillDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileBinary { get; set; }
        public int CustomerRequestId { get; set; }
    
        public virtual CustomerRequest CustomerRequest { get; set; }
    }
}
