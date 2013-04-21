#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

using System;

namespace VendingMachine
{
    /// <summary>
    /// WaitCoinState confirms that the machine is ready to accept coins for the currently selected product. It then handles the specific events and 
    /// subsequent state transitions based on the user actions
    /// </summary>
    public class WaitCoinState : IState
    {
        private readonly IVendingMachine _machine;

        /// <summary>
        /// When the state is instantiated we pass it a reference to the Vending machine. This is used to transition the machine
        /// to a different state.
        /// </summary>
        /// <param name="vendingMachine">An instance of the vending machine</param>
        public WaitCoinState(IVendingMachine vendingMachine)
        {
            _machine = vendingMachine;
        }

        /// <summary>
        /// <inheritdoc />
        /// User may try to select an item after inserting the coin.
        /// </summary>
        public void SelectItem(string itemName)
        {
            throw new ApplicationException(string.Format("Processing an item already. Cannot select {0}", itemName));
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// The operation checks if the customer has tendered the correct amount for the selected product.
        /// If the customer owes more then it propmpts to enter further amount else the state is changed to 
        /// <see cref="T:VendingMachine.SoldState"/>
        /// </remarks>
        /// </summary>
        public void InsertCoins()
        {
            // if balance is negative that means machine needs to return the amount to the customer
            if (_machine.CustomerBalance <= 0)
            {
                _machine.State = _machine.GetSoldState;
            }
            else
            {
                _machine.DisplayMessage(string.Format("Balance amount remaining {0}", _machine.CustomerBalance));
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void DispenseItem()
        {
            // Do Nothing ...
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void DispenseChange()
        {
           // Do Nothing...
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// User can wish to do an <see cref="M:VendingMachine.IState.EjectCoin"/> action in the <see cref="T:VendingMachine.WaitCoinState"/>. For ex: User inserted 10P but total price of the item is 20P.
        /// So, instead of continuing, he may as well decide to cancel the transaction and get the refund back.
        /// </remarks>
        /// </summary>
        public void EjectCoin()
        {
            var refund = _machine.RefundMoney();
            _machine.DisplayMessage(string.Format("Transaction has been cancelled. Please collect the refund{0}.", refund));
            _machine.State = _machine.GetUserSelectItemState;
        }
    }
}
