//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Text.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Invoince
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoince()
        {
            this.InvoinceDetails = new HashSet<InvoinceDetail>();
        }
    
        public string invoinceNo { get; set; }
        public Nullable<System.DateTime> dateOrder { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<bool> deliveryStatus { get; set; }
        public Nullable<System.DateTime> deliveryDate { get; set; }
        public int totalMoney { get; set; }
        public string userName { get; set; }
        public Nullable<int> customerId { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Member Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoinceDetail> InvoinceDetails { get; set; }
    }
}
