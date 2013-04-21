#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

using System;

namespace VendingMachine
{
    /// <summary>
    /// Main class (the context) responsible for delegating the actions to the current state object it is composed with.
    /// </summary>
    public class VendingMachineContext : IVendingMachine
    {
        // instance variable for referring the transaction manager for inventory and calculation purposes
        private readonly TransactionManager _transactionManager;

        #region Constructor
        /// <summary>
        /// The constructor takes an initial inventory of the products. All products are initialised to the same quantity.
        /// </summary>
        /// <param name="count">product quantity</param>
        public VendingMachineContext(int count)
        {
            // initialises an instance of transaction manager with initial inventory count.
            // Transaction manager keeps track of the product inventory and cash calculations
            _transactionManager = new TransactionManager(count);

            // all possible machine states are initialised
            InitializeStates();

            // the constructor takes an initial inventory of the products. If the inventory isn't zero, the machine enters state 
            // where it is ready for the users to select a product. Else, it goes into sold out state.
            State = TotalItemCount > 0 ? GetUserSelectItemState : GetSoldOutState;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes all possible states of the machine
        /// <list type="bullet">
        /// <item>
        ///     <description>
        ///     1. <see cref="T:VendingMachine.WaitUserSelectionState"/>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     2. <see cref="T:VendingMachine.WaitCoinState"/>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     3. <see cref="T:VendingMachine.SoldState"/>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     4. <see cref="T:VendingMachine.DispenseChangeState"/>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///     5. <see cref="T:VendingMachine.SoldOutState"/>
        ///     </description>
        /// </item>        
        /// </list>        
        /// </summary>
        private void InitializeStates()
        {
            GetUserSelectItemState = new WaitUserSelectionState(this);
            GetWaitCoinState = new WaitCoinState(this);
            GetSoldState = new SoldState(this);
            GetDispenseChangeState = new DispenseChangeState(this);
            GetSoldOutState = new SoldOutState(this);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Keeps track of the current state of the system. 
        /// </summary>
        public IState State { get; set; }

        /// <summary>
        /// Returns the <see cref="T:VendingMachine.WaitUserSelectionState"/> of the machine
        /// </summary>
        public IState GetUserSelectItemState { get; private set; }

        /// <summary>
        /// Returns the <see cref="T:VendingMachine.WaitCoinState"/> of the machine
        /// </summary>
        public IState GetWaitCoinState { get; private set; }

        /// <summary>
        /// Returns the <see cref="T:VendingMachine.SoldState"/> of the machine
        /// </summary>
        public IState GetSoldState { get; private set; }

        /// <summary>
        /// Returns the <see cref="T:VendingMachine.SoldOutState"/> of the machine
        /// </summary>
        public IState GetSoldOutState { get; private set; }

        /// <summary>
        /// Returns the <see cref="T:VendingMachine.DispenseChangeState"/> of the machine
        /// </summary>
        public IState GetDispenseChangeState { get; private set; }

        /// <summary>
        /// <remarks>
        /// If balance is negative, it indicates that machine owes a refund to the customer
        /// If the balance is positive, it indicates that the customer needs to insert more coins
        /// The machine delegates this to the <see cref="T:VendingMachine.TransactionManager"/> class
        /// Delegates the call to <see cref="P:VendingMachine.TransactionManager.TransactionBalance"/> property for getting the current transaction balance amount
        /// </remarks>
        /// </summary>
        public int CustomerBalance
        {
            get
            {
                return _transactionManager.TransactionBalance;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// <remarks>
        /// Delegates the call to <see cref="P:VendingMachine.TransactionManager.TotalItemCount"/> property for getting the total inventory count
        /// </remarks>
        /// </summary>
        public int TotalItemCount
        {
            get
            {
                return _transactionManager.TotalItemCount;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// <remarks>Delegates the call to <see cref="P:VendingMachine.TransactionManager.SelectedProduct"/> property for getting the currently selected product inthe transaction.</remarks>
        /// </summary>
        public IProduct SelectedItem
        {
            get
            {
                return _transactionManager.SelectedProduct;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>
        /// As part of the SelectItem action, the state asks the machine to update the inventory. 
        /// The machine in turn delegates this operation to the transaction manager.
        /// </remarks>
        /// </summary>
        /// <param name="productName">Name of the product</param>
        public void AddItem(string productName)
        {
            // update the select product details by delegating to the transaction manager
            _transactionManager.AddSelectedProduct(productName);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="productName">Name of the product</param>
        /// <returns>quantity of the product in the inventory</returns>
        public int GetItemCount(string productName)
        {
            return _transactionManager.GetProductQuantity(productName);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void SelectProduct(string itemName)
        {
            try
            {
                // delegate the current operation's context to the state 
                // such that the subsequent state of the machine can be determined correctly.
                State.SelectItem(itemName);
            }
            catch (Exception ex)
            {
                // currently, handling the exception by showing the exception message on the console.
                // May be we can have custom exceptions and handle each exception type separately and introduce logging.
               DisplayMessage(ex.Message);
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// <remarks>
        /// The action insert coin delegate the amount handling operation to the transaction manager
        /// It then calls the <see cref="M:VendingMachine.IState.InsertCoins"/> action on the currently
        /// set State. The DispenseItem and DispenseChange are internal operations which are handled automatically
        /// based on the state transitions.
        /// </remarks>
        /// </summary>
        /// <param name="coinValue">The denomination value</param>
        public void InsertCoin(int coinValue)
        {
            try
            {
                // update the transaction such the amount can be tracked
                _transactionManager.AddCashToBalance(coinValue);

                // delegate the handling to appropriate state for next processing steps
                State.InsertCoins();

                // Internal to the machine. No user action required for these events.
                State.DispenseItem();
                State.DispenseChange();
            }
            catch (ApplicationException ex)
            {
                // currently, handling the exception by showing the exception message on the console.
                // May be we can have custom exceptions and handle each exception type separately and introduce logging.
                DisplayMessage(ex.Message);
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// <remarks>
        /// This operation is equivalent of cancelling of an operation
        /// </remarks>
        /// </summary>
        public void EjectCoin()
        {
            try
            {
                State.EjectCoin();
            }
            catch (Exception ex)
            {
                // currently, handling the exception by showing the exception message on the console.
                // May be we can have custom exceptions and handle each exception type separately and introduce logging.
                DisplayMessage(ex.Message);
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// This action is performed when the sale has happened and the VendingMachineContext asks the 
        /// TransactionManager to udpate the inventory of the currently selected product
        /// </summary>
        public void ReleaseItem()
        {
            _transactionManager.UpdateTransaction();
        }
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns>Formmated String containing the denomination value and quantity</returns>
        public string RefundMoney()
        {
           return _transactionManager.RefundMoney();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns>Formmated String containing the denomination value and quantity</returns>
        public string TenderChange()
        {
            return _transactionManager.TenderChange();
        }
        
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="message">User Message</param>
        public void DisplayMessage(string message)
        {
            // output to the console
            Console.WriteLine(message);
        }
        #endregion
    }
}
