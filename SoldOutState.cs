#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

using System;

namespace VendingMachine
{
    /// <summary>
    /// SoldOutState confirms that the machine is empty and needs refilling. No state transitions happen if the machine is in this state
    /// </summary>
    public class SoldOutState : IState
    {
        private readonly IVendingMachine _machine;

        /// <summary>
        /// When the state is instantiated we pass it a reference to the Vending machine.
        /// </summary>
        /// <param name="vendingMachine">An instance of the vending machine context</param>
        public SoldOutState(IVendingMachine vendingMachine)
        {
            _machine = vendingMachine;
        }

        /// <summary>
        /// <inheritdoc />
        /// This can only happen when total item count is zero and machine is in sold out state.  
        /// User tries to select an item 
        /// </summary>
        public void SelectItem(string itemName)
        {
            _machine.DisplayMessage("Products are not available. Machine needs a refill.");
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// If the user performs an Insert Coin action in SoldOut state then refund back the money
        /// </remarks>
        /// </summary>
        public void InsertCoins()
        {
            _machine.RefundMoney();
            throw new ApplicationException("Machine is empty. Please take back your money.");
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void DispenseItem()
        {
            // Do nothing
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void DispenseChange()
        {
           // Do Nothing
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void EjectCoin()
        {
            throw new ApplicationException("Machine is empty.");
        }
    }
}
