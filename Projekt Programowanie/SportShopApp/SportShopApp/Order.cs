using System;

namespace SportShopApp
{
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; }    
        public string ProductName { get; set; } 
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}