﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.Models
{
    public partial class TDiscount
    {
        public int CDiscountId { get; set; }
        public string CDiscountName { get; set; }
        public double CDisPercentage { get; set; }
        public DateTime CStatTime { get; set; }
        public DateTime CEndTime { get; set; }

        public virtual TMemberBenefits TMemberBenefits { get; set; }
    }
}