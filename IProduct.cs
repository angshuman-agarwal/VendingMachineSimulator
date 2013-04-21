#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// Abstraction for the Vending machine items
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// Price of the item 
        /// <remarks>
        /// Assumption is whole number currently. Can be changed to float
        /// </remarks>
        /// </summary>
        int Price { get; set; }
        
        /// <summary>
        /// Name of the item
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Quantity of the item
        /// </summary>
        int Quantity { get; set; }
    }
}