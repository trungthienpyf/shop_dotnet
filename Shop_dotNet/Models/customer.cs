//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shop_dotNet.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public customer()
        {
            this.orders = new HashSet<order>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<byte> gender { get; set; }
        public Nullable<System.DateTime> birthday { get; set; }
        public string email { get; set; }
        public string passsword { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public Nullable<byte> role { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> orders { get; set; }
    }
}