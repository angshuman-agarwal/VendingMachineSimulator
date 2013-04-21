#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// The Vending machine interface for the customers
    /// </summary>
    public interface IVendingMachineClient
    {
        /// <summary>
        /// Represents a user action for selecting a product from the machine 
        /// </summary>
        /// <param name="itemName">Product Name</param>
        void SelectProduct(string itemName);

        /// <summary>
        /// Represents a user action to insert coins into the machine
        /// </summary>
        /// <param name="coinValue">Value of the coin</param>
        void InsertCoin(int coinValue);

        /// <summary>
        /// Represents a user action to eject the coin from the machine
        /// </summary>
        void EjectCoin();
    }
}
