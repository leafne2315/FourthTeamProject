﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.Models
{
    public partial class TProductUsage
    {
        public int CProductUsage { get; set; }
        public string CUsageName { get; set; }

        public virtual TProductList TProductList { get; set; }
    }
}