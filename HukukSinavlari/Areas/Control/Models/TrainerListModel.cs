﻿namespace HukukSinavlari.Areas.Control.Models
{
    public class TrainerListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool State { get; set; }
        public string[] Role { get; set; }
    }
}
