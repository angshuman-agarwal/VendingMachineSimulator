#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

using System;

namespace VendingMachine
{
    /// <summary>
    /// SoldState confirms that the machine is ready to dispense and release the product. 
    /// It takes care of the appropriate state transitions in the <see cref="M:VendingMachine.IState.DispenseItem"/> action
    /// </summary>
    public class SoldState : IState
    {
        private readonly IVendingMachine _machine;

        /// <summary>
        /// When the state is instantiated we pass it a reference to the Vending machine.
        /// </summary>
        /// <param name="vendingMachine">An instance of the vending machine context</param>
        public SoldState(IVendingMachine vendingMachine)
        {
            _machine = vendingMachine;
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// The current state is the <see cref="T:VendingMachine.SoldState"/>. 
        /// User action of selecting a drink is invalid. See <see cref="M:VendingMachine.SoldState.DispenseItem"/>. 
        /// </remarks>
        /// </summary>
        /// <param name="itemName"></param>
        public void SelectItem(string itemName)
        {
            throw new ApplicationException("Cannot select item now, we are already dispensing the item for you.");
        }

        /// <summary>
        /// <inheritdoc />
        /// <remarks>
        /// The current state is the <see cref="T:VendingMachine.SoldState"/>. 
        /// User action of inserting a coin is invalid. See <see cref="M:VendingMachine.SoldState.DispenseItem"/>.
        /// </remarks>
        /// </summary>
        public void InsertCoins()
        {
            throw new ApplicationException("Please wait, we are already dispensing the item for you.");
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// The current state is the <see cref="T:VendingMachine.SoldState"/>. This means that the user has paid. 
        /// The following scenarios are possible in this case :- 
        /// <list type="bullet">
        /// <item>
        ///     <description>
        ///     Case 1. Machine does not have exact change to tender. Hence, refund the user money and set the transition state to 
        ///     <see cref="T:VendingMachine.WaitUserSelectionState"/>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     Case 2. Everything is fine. Ask the machine to release the drink.
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     Case 3. After releasing the item, check if the machine owes any amount to the user. If yes, then,
        ///     the state is changed to <see cref="T:VendingMachine.DispenseChangeState"/>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     Case 4. After releasing the item, checks if the machine has enough inventory and it owes nothing to the user. 
        ///     Transitions the state to <see cref="T:VendingMachine.WaitUserSelectionState"/>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     Case 5. After releasing the item, check the total inventory count. 
        ///     If its zero, then transition the state to  <see cref="T:VendingMachine.SoldOutState"/>
        ///     </description>
        /// </item>        
        /// </list>
        /// </remarks>
        /// </summary>
        public void DispenseItem()
        {
            // Machine does not have exact change to tender, hence do not dispense the item
            // Change back the state to user selection state, refund the money and show an appropriate message to the user.
            if (String.IsNullOrEmpty(_machine.TenderChange()))
            {
                var refund = _machine.RefundMoney();
                _machine.State = _machine.GetUserSelectItemState;
                throw new ApplicationException(string.Format("Machine does not have sufficient change. Please tender exact change. Please collect your inserted amount{0}", refund));
            }

            _machine.ReleaseItem();
            _machine.DisplayMessage("Item has been dispensed. Please do not forget to collect it.");

            // machine has to refund
            if (_machine.CustomerBalance < 0)
            {
                _machine.State = _machine.GetDispenseChangeState;
            }
            
            // Machine has enough products and has nothing to refund back to the user
            if (_machine.TotalItemCount > 0 && _machine.CustomerBalance == 0)
            {
                _machine.State = _machine.GetUserSelectItemState;
            }
            
            // Machine is empty. Everything is sold out. Hence, change the state to SoldOut
            if(_machine.TotalItemCount == 0)
            {
                _machine.State = _machine.GetSoldOutState;
                throw new ApplicationException("Sorry, the machine is out of stock.");
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// Nothing has to be done in the DispenseChange action for <see cref="T:VendingMachine.SoldState"/>
        /// </remarks>
        /// </summary>
        public void DispenseChange()
        {
            // Do Nothing...
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// User tries to do an EjectCoin operation when the state is  <see cref="T:VendingMachine.SoldState"/>. This is an invalid action. 
        /// Throw an exception saying invalid operation.
        /// </remarks>
        /// </summary>
        public void EjectCoin()
        {
            throw new ApplicationException("Invalid operation. You have already confirmed the purchase.");
        }
    }
}
