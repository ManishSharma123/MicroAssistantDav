//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MicroAssistant.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class MicroAssistant_Services
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BlurbText { get; set; }
        public byte[] Photo { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string City { get; set; }
        public long StateId { get; set; }
        public long CountryId { get; set; }
        public string CreatedBy { get; set; }
        public long UserId { get; set; }
        public string remark { get; set; }
    }
}
