using StoreClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StoreClassLibrary
{
    public class ProductList
    {
        /// <summary>
        /// </summary>
        public Dictionary<Product, int> Products { get; private set; }
        public double TotalSum { get; set; } = 0;
        public DiscountCode ActiveDiscountCode { get; set; }

        public void AddProduct(Product product)
        {
            if (product == null)
            {
                // TODO(johancz): this silently handles cases where product is null, should it be handled otherwise?
                return;
            }

            Products.TryAdd(product, 0);
            Products[product]++;
            TotalSum += product.Price;
        }

        public void RemoveProduct(Product product)
        {
            if (product == null)
            {
                // TODO(johancz): this silently handles cases where product is null, should it be handled otherwise?
                return;
            }

            if (Products.ContainsKey(product))
            {
                if (Products[product] > 1)
                {
                    Products[product]--;
                }
                else
                {
                    Products.Remove(product);
                }

                TotalSum -= product.Price;
            }
        }

        public void SetDiscountCode(DiscountCode discountCode)
        {
            if (ActiveDiscountCode == null)
            {
                ActiveDiscountCode = discountCode;
            }
        }

        public void RemoveDiscountCode()
        {
            if (ActiveDiscountCode != null)
            {
                ActiveDiscountCode = null;
            }
        }

        public void SaveToFile()
        {
            // Copy files in .csproj instead?
            // ..\..\..\.. takes us to the solution root folder
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\StoreData\ShoppingCart.csv");
            var fileExists = File.Exists(filePath);
            throw new NotImplementedException();
        }

        public void LoadFromFile()
        {
            // Copy files in .csproj instead?
            // ..\..\..\.. takes us to the solution root folder
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\StoreData\ShoppingCart.csv");
            var fileExists = File.Exists(filePath);
            throw new NotImplementedException();
        }

        public ProductList()
        {
            Products = new Dictionary<Product, int>();
        }

        //Add LoadShoppingcart
    }
}
