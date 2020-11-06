using StoreClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Butik_User
{
    public class ShoppingCart
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
            throw new NotImplementedException();
        }

        public void RemoveProduct(Product product)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void SaveToFile()
        {
            throw new NotImplementedException();
        }
        public void LoadFromFile()
        {
            throw new NotImplementedException();
        }

        public ShoppingCart()
        {
            Products = new Dictionary<Product, int>();
        }
    }
}
