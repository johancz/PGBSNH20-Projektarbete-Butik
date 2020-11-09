using StoreClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreClassLibrary
{
    public class ProductList
    {
        /// <summary>
<<<<<<< HEAD:StoreClassLibrary/ProductList.cs
=======
        /// Collection of "KeyValuePair"s where:
        ///     Key (Product): instance of Product-class
        ///     Value (int):   itemcount of (Key)"Product"
>>>>>>> johancz/junk/convert-ShoppingCart-class-into-generic-list-of-products:Butik-User/ProductList.cs
        /// </summary>
        ///  Collection of "KeyValuePair"s where:
        ///  Key (Product): instance of Product-class
        ///  Value (int):   itemcount of (Key)"Product"
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

        public ProductList()
        {
            Products = new Dictionary<Product, int>();
        }

        //Add LoadShoppingcart
    }
}
