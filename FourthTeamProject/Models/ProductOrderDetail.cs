﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.Models
{
    public partial class ProductOrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool DetailStatus { get; set; }
        public string ProductName { get; set; }
        public string ProductContent { get; set; }
        public string ProductCatagoryName { get; set; }

        public virtual ProductOrder Order { get; set; }
    }
}