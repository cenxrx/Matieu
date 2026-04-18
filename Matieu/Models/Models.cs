using System;

namespace Matieu
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string Category { get; set; } = "";
        public int? CollectionId { get; set; }
        public string CollectionName { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
    }

    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public string FullName { get; set; } = "";
        public decimal Balance { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = "";
    }
}
