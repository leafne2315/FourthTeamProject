﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.Models
{
    public partial class TProductType
    {
        public int CPetTypeId { get; set; }
        public string CPetTypeName { get; set; }

        public virtual TProductList CPetType { get; set; }
    }
}