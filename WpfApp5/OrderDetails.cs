//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp5
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetails
    {
        public int OrderID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> ClientID { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual ProductCards ProductCards { get; set; }
    }
}