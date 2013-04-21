#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

using System;

namespace VendingMachine
{
    /// <summary>
    /// WaitUserSelectionState confirms that the machine is ready for product selection. it then handles the specific events and subsequent state transitions 
    /// based on the user actions
    /// </summary>
    public class WaitUserSelectionState : IState
    {
        private readonly IVendingMachine _machine;

        /// <summary>
        /// When the state is instantiated we pass it a reference to the Vending machine. This is used to transition the machine
        /// to a different state.
        /// </summary>
        /// <param name="vendingMachine">An instance of the vending machine</param>
        public WaitUserSelectionState(IVendingMachine vendingMachine)
        {
            _machine = vendingMachine;
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// This is the appropriate action for <see cref="T:VendingMachine.WaitUserSelectionState"/> i.e. when the machine is ready for the user to select an item.
        /// <list type="bullet">
        /// <item>
        ///     <description>
        ///     Case 1. Checks the total machine inventory and if it is empty, it transitions back to <see cref="T:VendingMachine.SoldOutState"/>. 
        ///     Machine needs a refill.
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     Case 2. Checks if there is sufficient stock of the selected item and transitions the state to <see cref="T:VendingMachine.WaitCoinState"/>. 
        ///     User is in a position to insert the coins.
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     Case 3. If there is no stock left for the selected item it transitions back to <see cref="T:VendingMachine.WaitUserSelectionState"/>. 
        ///     This allows the user to reselect a different item.
        ///     </description>
        /// </item>
        /// </list>
        /// </remarks>
        /// </summary>
        /// <param name="itemName">Name of the product</param>
        public void SelectItem(string itemName)
        {
            // Machine's inventory is Zero. Everything is sold out. Make the state transition to the sold out state
            if (_machine.TotalItemCount == 0)
            {
                _machine.State = _machine.GetSoldOutState;
                return;
            }

            // check if the machine has enough stock of the selected item
            if (_machine.GetItemCount(itemName) > 0) 
            {
                // tell the machine to update the selected product in transaction
                _machine.AddItem(itemName);
                _machine.DisplayMessage(string.Format("You selected {0} with a value of {1} ",_machine.SelectedItem.Name, _machine.SelectedItem.Price));

                // change the state to "wait for coins". 
                _machine.State = _machine.GetWaitCoinState;
 
                // display an appropriate message to the user
                _machine.DisplayMessage(string.Format("Please insert coins to buy '{0}'", _machine.SelectedItem.Name));
            }
            else
            {
                // Sufficient stock is not available. Hence, change the state to wait selection state 
                // such that the user can select a different drink. Notify this to the user.
                _machine.State = _machine.GetUserSelectItemState;
                throw new ApplicationException(string.Format("Sorry, '{0}' is not available. Please select a different drink.",
                                    _machine.SelectedItem.Name));
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// When the machine is in <see cref="T:VendingMachine.WaitUserSelectionState"/>, user may try to perform the <see cref="M:VendingMachine.IState.InsertCoins"/> action. 
        /// In such a situation, the state will signal the machine to refund the coins.
        /// No state transition will happen. The state of the machine remains unaltered.
        /// User is presented with a message saying the product selection needs to happen first.
        /// Alternatively,
        /// </remarks>
        /// </summary>
        public void InsertCoins()
        {
            // return the inserted money from the user as the state is not appropriate to accept coins
            _machine.RefundMoney();
            throw new ApplicationException("Please select a product first. Collect back your money.");
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// <see cref="M:VendingMachine.IState.DispenseItem"/> is an internal action specific to the vending machine. 
        /// This action does not have to be handled in the <see cref="T:VendingMachine.WaitUserSelectionState"/>
        /// </remarks>
        /// </summary>
        public void DispenseItem()
        {
            // Do Nothing...
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// <see cref="M:VendingMachine.IState.DispenseChange"/> is an internal action specific to the vending machine. 
        /// This action does not have to be handled in the <see cref="T:VendingMachine.WaitUserSelectionState"/>
        /// </remarks>
        /// </summary>
        public void DispenseChange()
        {
            // Do Nothing...
        }

        /// <summary>
        /// The <see cref="M:VendingMachine.IState.EjectCoin"/> user action can be performed in the <see cref="T:VendingMachine.WaitUserSelectionState"/>
        /// In such situation there is no state transition. 
        /// </summary>
        public void EjectCoin()
        {
            throw new ApplicationException("Cannot Eject any coin. Please select a product first.");
        }
    }
}
