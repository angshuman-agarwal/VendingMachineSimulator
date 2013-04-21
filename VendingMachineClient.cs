#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// The client wrapper class for the Vending Machine context. 
    /// <remarks>
    /// The Vending Machine context could be running over the network on a different server.
    /// </remarks>
    /// </summary>
    public class VendingMachineClient : IVendingMachineClient
    {
        private readonly IVendingMachine _vendingMachine;

        /// <summary>
        /// Creates a vending machine context internally with an initial inventory count
        /// <remarks>
        /// The inventory count can be removed externally into an app.config file later and the 
        /// vending machine context can directly read. For now, let us assume this.
        /// </remarks>
        /// </summary>
        /// <param name="itemCount"></param>
        public VendingMachineClient(int itemCount)
        {
          _vendingMachine  = new VendingMachineContext(itemCount);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="itemName">Name of the product selected by the user</param>
        public void SelectProduct(string itemName)
        {
            _vendingMachine.SelectProduct(itemName);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="coinValue">Value of the coin inserted by the user</param>
        public void InsertCoin(int coinValue)
        {
            _vendingMachine.InsertCoin(coinValue);
        }

        /// <summary>
        /// User action to cancel a transaction and get the refund
        /// </summary>
        public void EjectCoin()
        {
            _vendingMachine.EjectCoin();
        }
    }
}
