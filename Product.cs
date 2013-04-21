#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// Product class for keeping track of items
    /// </summary>
    public class Product : IProduct
    {
        /// <summary>
        /// Initialisea new product with name, price and quantity
        /// </summary>
        /// <param name="name">Product name</param>
        /// <param name="price">Product price</param>
        /// <param name="quantity">Product quantity</param>
        public Product(string name, int price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name { get; set; }
    }
}
