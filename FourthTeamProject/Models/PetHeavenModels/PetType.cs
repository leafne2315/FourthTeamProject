﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FourthTeamProject.PetHeavenModels
{
    public partial class PetType
    {
        public PetType()
        {
            Pet = new HashSet<Pet>();
        }

        public int PetTypeId { get; set; }
        public string PetTypeName { get; set; }

        public virtual ICollection<Pet> Pet { get; set; }
    }
}