#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// Interface of the Vending Machine context for handling user actions and current transactions
    /// </summary>
    public interface IVendingMachine
    {
        /// <summary>
        /// Keeps track of the Current <see cref="T:VendingMachine.IState"/> of the machine
        /// <returns>returns the current state of the machine</returns>
        /// </summary>
        IState State { get; set; }

        /// <summary>
        /// Returns the User Selection Wait <see cref="T:VendingMachine.IState"/> instance
        /// <returns>returns the Wait User Selection State</returns>
        /// </summary>
        IState GetUserSelectItemState { get; }

        /// <summary>
        /// Returns the Wait for Coin Insertion <see cref="T:VendingMachine.IState"/> instance
        /// <returns>returns the Coin Wait State</returns>
        /// </summary>
        IState GetWaitCoinState { get; }

        /// <summary>
        /// Returns the Sold <see cref="T:VendingMachine.IState"/> instance
        /// <returns>returns the Sold State</returns>
        /// </summary>
        IState GetSoldState { get; }

        /// <summary>
        /// Returns the Sold Out <see cref="T:VendingMachine.IState"/> instance
        /// </summary>
        /// <returns>returns the Sold Out State</returns>
        IState GetSoldOutState { get; }

        /// <summary>
        /// Returns the Change Dispense <see cref="T:VendingMachine.IState"/> instance
        /// </summary>
        /// <returns>returns the Dispense Change State</returns>
        IState GetDispenseChangeState { get; }

        /// <summary>
        /// Returns the Currently Selected <see cref="T:VendingMachine.IProduct"/> in the transaction 
        /// </summary>
        /// <returns>returns currently selected product in the transaction</returns>
        IProduct SelectedItem { get; }

        /// <summary>
        /// Returns the total inventory count
        /// </summary>
        /// <returns>returns total inventory count</returns>
        int TotalItemCount { get; }

        /// <summary>
        /// Returns the customer's balance for the current transaction
        /// </summary>
        /// <returns>returns the balance for the current transaction. 
        /// Can be negative which indicates that machine has to dispense change
        /// </returns>
        int CustomerBalance { get; }

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

        /// <summary>
        /// Updates the inventory based on the current transaction after dispensing the item
        /// </summary>
        void ReleaseItem();

        /// <summary>
        /// Updates the current transaction in the form of refunding the user's money when the user does an <see cref="M:VendingMachine.VendingMachineContext.EjectCoin"/> operation
        /// Returns a formatted string containing the denominations and the respective count
        /// e.g. If 6P is returned then it will be formatted as like 1x5P\r\n 1x1P\r\n
        /// </summary>
        /// <returns>returns formatted string stating the denomination value and the respective count</returns>
        string RefundMoney();

        /// <summary>
        /// Updates the currently successful transaction in the form of calculating appropriate change to be tendered to the user
        /// Returns a formatted string containing the denominations and the respective count
        /// e.g. If 6P is tendered as change then it will be formatted as 1x5P\r\n 1x1P\r\n
        /// </summary>
        /// <returns>returns formatted string stating the denomination value and the respective count</returns>
        string TenderChange();

        /// <summary>
        /// Adds the item for tracking in the current transaction. 
        /// </summary>
        /// <param name="name"></param>
        void AddItem(string name);

        /// <summary>
        /// Gets the quantity for an item from the inventory
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>returns total count of the given item</returns>
        int GetItemCount(string productName);

        /// <summary>
        /// Displays the message to the user
        /// </summary>
        /// <param name="message">User Message</param>
        void DisplayMessage(string message);
    }
}
