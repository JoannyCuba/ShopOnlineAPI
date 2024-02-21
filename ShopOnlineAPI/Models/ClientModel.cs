﻿namespace ShopOnlineAPI.Models
{
    public class ClientModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public ICollection<ClientProductModel>? Products { get; set; }

    }
}
