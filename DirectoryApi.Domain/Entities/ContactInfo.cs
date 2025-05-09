﻿namespace DirectoryApi.Domain.Entities
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public int DirectoryId { get; set; }
    }
}
