﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.Models
{
    public partial class TProductOrderDetail
    {
        public int COrderDetailId { get; set; }
        public int COrderId { get; set; }
        public int CProductId { get; set; }
        public int CUnitPrice { get; set; }
        public double CQuantity { get; set; }
        public bool CDetailStatus { get; set; }

        public virtual TProductList COrderDetail { get; set; }
        public virtual TProductOrder TProductOrder { get; set; }
    }
}