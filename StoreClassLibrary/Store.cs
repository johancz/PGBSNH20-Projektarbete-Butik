using System;
using System.Collections.Generic;
using System.Text;

namespace StoreClassLibrary
{
        public static class Store
        {
            public static List<Product> Products { get; set; } = new List<Product>();
            public static ShoppingCart ActiveShoppingCart { get; set; } = new ShoppingCart();
            public static List<DiscountCode> DiscountCodes { get; set; }

            public static void Init()
            {
                Products = Product.LoadAll();
                LoadShoppingCart();
                LoadDiscountCodes();
            }

            private static void LoadShoppingCart()
            {
                
            }

            private static void LoadDiscountCodes()
            {
                //DiscountCodes = ...
            }
        }
    
}
