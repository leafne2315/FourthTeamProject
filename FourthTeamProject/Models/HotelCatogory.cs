﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.Models
{
    public partial class HotelCatogory
    {
        public HotelCatogory()
        {
            Hotel = new HashSet<Hotel>();
        }

        public int HotelCatogoryId { get; set; }
        public string HotelCatogoryName { get; set; }

        public virtual ICollection<Hotel> Hotel { get; set; }
    }
}