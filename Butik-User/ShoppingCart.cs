using System;
using System.Collections.Generic;
using System.Text;

namespace Butik_User
{

    class ShoppingCart
    {
        /// <summary>
        /// Product: instance of Product-class
        /// int:     amount of Product in ShoppingCart
        /// </summary>
        public Dictionary<Product, int> Products;
        public double TotalSum { get; set; } // TODO(johancz): decimal?
        public DiscountCode ActiveDiscountCode { get; set; }

        public void AddProduct(Product product)
        {

        }

        public void RemoveProduct(Product product)
        {

        }

        public void AddDiscountCode(DiscountCode discountCode)
        {
            if (ActiveDiscountCode == null)
            {
                ActiveDiscountCode = discountCode;
            }
        }

        public void RemoveDiscountCode()
        {

        }

        public void SaveToFile()
        {

        }
        public void LoadFromFile()
        {

        }

        public ShoppingCart()
        {
            Products = new Dictionary<Product, int>();
        }
    }
}
