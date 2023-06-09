﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.PetHeavenModels
{
    public partial class ProductOrder
    {
        public ProductOrder()
        {
            ProductOrderDetail = new HashSet<ProductOrderDetail>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int MemberId { get; set; }
        public int InvoiceId { get; set; }
        public int PayId { get; set; }
        public bool OrderStatus { get; set; }
        public string OrderAddress { get; set; }
        public string OrderMemberName { get; set; }
        public string OrderMemberPhone { get; set; }
        public string OrderMemberEmail { get; set; }
        public string OrderNo { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Member Member { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<ProductOrderDetail> ProductOrderDetail { get; set; }
    }
}