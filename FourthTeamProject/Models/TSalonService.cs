﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.Models
{
    public partial class TSalonService
    {
        public int CService { get; set; }
        public int CSalonId { get; set; }
        public int CPetSizeId { get; set; }
        public int CUnitPrice { get; set; }

        public virtual TSalonType CSalon { get; set; }
        public virtual TPetSize CServiceNavigation { get; set; }
        public virtual TSalonOrderDetail TSalonOrderDetail { get; set; }
    }
}