﻿namespace FourthTeamProject.Models.DTO
{
    public class createDto
    {
        public string petName { get; set; }
        public int petGender { get; set; }
        public string petVariety { get; set; }
        public DateTime? petBirthday { get; set; }
        public int? petTypeId { get; set; }
    }
}
