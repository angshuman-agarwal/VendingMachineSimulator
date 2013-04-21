#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// Defines a common interface for all concrete states.
    /// This is for encapsulating the behaviour associated with a particular state of the context (in this case, its the Vending machine)
    /// Whenever a request is made on the context (Vending Machine), it is delegated to the appropriate current state to handle
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Action on the state to dispense an item
        /// </summary>
        void DispenseItem();

        /// <summary>
        /// Action on the state to dispense the change
        /// </summary>
        void DispenseChange();

        /// <summary>
        /// Action on the state to cancel the transaction in the form of ejecting the coin
        /// </summary>
        void EjectCoin();

        /// <summary>
        /// Action on the state to insert coins
        /// </summary>
        void InsertCoins();

        /// <summary>
        /// Action on the state to select an item
        /// </summary>
        void SelectItem(string itemName);
    }
}
