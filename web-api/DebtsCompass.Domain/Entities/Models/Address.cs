﻿namespace DebtsCompass.Domain.Entities.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
    }
}
