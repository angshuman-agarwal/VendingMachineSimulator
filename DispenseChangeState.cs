#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// DispenseChangeState confirms that the item has been sold and if the machine owes any change, then that will be dispensed to the user. 
    /// It then handles the specific events and subsequent state transitions.
    /// </summary>
    public class DispenseChangeState : IState
    {
        private readonly IVendingMachine _machine;

        /// <summary>
        /// When the state is instantiated we pass it a reference to the Vending machine context. 
        /// </summary>
        /// <param name="vendingMachine">An instance of the vending machine context</param>
        public DispenseChangeState(IVendingMachine vendingMachine)
        {
            _machine = vendingMachine;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void SelectItem(string itemName)
        {
             // Do Nothing
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void InsertCoins()
        {
            // Do Nothing
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void DispenseItem()
        {
            // Do Nothing
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void EjectCoin()
        {
            // Do Nothing
        }


        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// Dispenses the changes and sets the state back to user wait selection
        /// </remarks>
        /// </summary>
        public void DispenseChange()
        {
            var change = _machine.TenderChange();
            _machine.DisplayMessage(string.Format("Please collect your change : \n{0}", change));
            _machine.State = _machine.GetUserSelectItemState;
        }
    }
}
