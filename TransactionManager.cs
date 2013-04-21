#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine
{
    /// <summary>
    /// TransactionManager class is responsible for keeping track of the product inventory and the cash transaction
    /// This class also contains the algorithm for coin change. Refer the "CalculateChange" function.
    /// </summary>
    public class TransactionManager
    {
        private List<Denomination> _initialBalance;
        private int _totalCoinsInserted;
        private List<IProduct> _inventoryList;

        /// <summary>
        /// The constructor takes the initial product inventory count. For simplicity, all products are initialised to the 
        /// same quantity
        /// </summary>
        /// <param name="count">Initial count of each product</param>
        public TransactionManager(int count)
        {
            // the product inventory is created with an initial count supplied. 
            PrepareInventory(count);

            // the denominations are initialised as 1P, 2P, 5P, 10P and 50P
            AssignDenominations(new[] { 1, 2, 5, 10, 20, 50 });
        }

        /// <summary>
        /// Gets the remaining quantity for the given product
        /// </summary>
        /// <param name="productName">name of the product for which the user is seeking the quantity</param>
        /// <returns>remaining quantity for the given product</returns>
        public int GetProductQuantity(string productName)
        {
            var p = from product in _inventoryList where product.Name == productName select product;
            return p.FirstOrDefault().Quantity;
        }

        /// <summary>
        /// Update the instance with the currently selected product for tracking purpose
        /// </summary>
        /// <param name="productName">name of the product</param>
        public void AddSelectedProduct(string productName)
        {
           var p = from product in _inventoryList where product.Name == productName select product;
           SelectedProduct = p.FirstOrDefault();
        }

        /// <summary>
        /// Updates the quantity for the currently selected product in the transaction alongwith the user coin  values
        /// </summary>
        public void UpdateTransaction()
        {
            _totalCoinsInserted = 0;
            SelectedProduct.Quantity--;
        }

        /// <summary>
        /// Updates the balance amount for the current transaction
        /// </summary>
        /// <param name="coinValue">Value of the coin (between 1p and 50P)</param>
        public void AddCashToBalance(int coinValue)
        {
            var i = from n in _initialBalance where n.DenominationValue == coinValue select n;
            var denomination = i.FirstOrDefault();
            if (denomination != null)
            {
                denomination.DenominationCount++;
            }
            _totalCoinsInserted += coinValue;

            // no point updating the product when user simply enters coins into the machine without selecting the product
            if (SelectedProduct != null)
            {
                TransactionBalance = SelectedProduct.Price - _totalCoinsInserted;
            }
        }

        /// <summary>
        /// Refunds the customer's amount on an <see cref="M:VendingMachine.IState.EjectCoin"/> action. This is refunded in the form of 
        /// minimum number of coins that is required to make change for the inserted amount. It won't necessarily give back the exact 
        /// denominations as inserted by the customer. It will give back an optimal denomination always.
        /// <remarks>
        /// The return value is a formatted string which says something like (No. of coins X CoinValue) i.e. say, 1 x 5P
        /// </remarks>
        /// </summary>
        /// <returns>formatted string</returns>
        public string RefundMoney()
        {
            var change = CalculateChange(_totalCoinsInserted);
            // update the user inserted coins to zero
            _totalCoinsInserted = 0;
            return change;
        }

        /// <summary>
        /// Gets the change amount to be given to the customer (in case he has inserted more than the product price)
        /// Minimum number of coins that is required to make change for the balance amount to be tendered to the customer
        /// <remarks>
        /// The return value is a formatted string which says something like (No. of coins X CoinValue) i.e. say, 1 x 5P
        /// </remarks>
        /// </summary>
        /// <returns>formatted string</returns>
        public string TenderChange()
        {
            // taking an absolute value because the TransactionBalance can be negative
            return CalculateChange(Math.Abs(TransactionBalance));
        }

        /// <summary>
        /// This method is used to calculate the amount of change to be returned (if any) at the end of a successful transaction 
        /// or calculate the coin denomination for the inserted customer money on cancellation of a transaction request from 
        /// the customer.
        /// <remarks>
        /// Algorithm:
        /// Uses the greedy algorithm approach by arranging the denomination in descending order of their values. This ensures
        /// that the customer will always get less no. of coins when bigger denimination values are available with the
        /// machine. Can be done with a more optimal algorithm by applying Dynamic Programming technique.
        /// </remarks>
        /// <returns>formatted string containing the denomination value and quantity</returns>
        /// </summary>
        /// <param name="amount">Amount for which the change needs to be calculated</param>
        /// <returns>Formatted string containing the change information</returns>
        public string CalculateChange(int amount)
        {
            var sb = new StringBuilder();

            // arrange the denomination in decreasing order such that the largest Denomination Value is on the TOP
            var descendingCollection = _initialBalance.OrderByDescending(b=>b.DenominationValue);

            // loop through the descending collection
            foreach (var denomination in descendingCollection)
            {
                if (denomination.DenominationCount > 0 && amount > 0)
                {
                    int numberOfDenominations = (amount / denomination.DenominationValue);
                    
                    // i.e. Denomination Value is greater than the amount, hence go to the next denomination value
                    if (numberOfDenominations == 0)
                    {
                        continue;
                    }

                    // e.g. We have 2x10P(denomination.DenominationCount), but we need 3x10P (numberOfDenominations)
                    if (numberOfDenominations > denomination.DenominationCount)
                    {
                        numberOfDenominations = denomination.DenominationCount;

                        // we have comsumed all the denominations of this value
                        denomination.DenominationCount = 0;
                        amount -= numberOfDenominations * denomination.DenominationValue;
                    }
                    else
                    {
                        // balance amount for which change has to be calculated 
                        amount = (amount % denomination.DenominationValue);

                        // update the denomination count
                        denomination.DenominationCount -= numberOfDenominations;
                    }

                    // keep track of the denomination value and its corresponding count
                    string coinValueCountPairFormattedOutput = string.Format("{0} x {1}P", numberOfDenominations,
                                                                             denomination.DenominationValue);

                    sb.Append(coinValueCountPairFormattedOutput);
                    sb.Append(Environment.NewLine); // \r\n
                }
            }

            // we have reached a point where change could not be tendered because machine does not have the correct
            // combination of denominations to tender the required change. For e.g. change to be given is 15P but the
            // machine contains 2 x 20P.
            if (amount > 0)
            {
                // for now, just clear the string and return an empty string in this case.
                sb.Clear();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the current transaction balance 
        /// <returns>integer denoting the current balance owned by user or machine</returns>
        /// </summary>
        public int TransactionBalance { get; private set; }

        /// <summary>
        /// Returns the total inventory count
        /// <returns>total inventory item count</returns>
        /// </summary>
        public int TotalItemCount
        {
            get
            {
                return _inventoryList.Sum(inv => inv.Quantity);
            }
        }

        /// <summary>
        /// Keeps track the currently selected product in a transaction
        /// <returns>currently selected product</returns>
        /// </summary>
        public IProduct SelectedProduct { get; private set; }

        /// <summary>
        /// Initializes the product inventory with an appropriate count for each product item.
        /// </summary>
        /// <remarks>
        /// The quantity is same for all products. And, the prices and product names are hardcoded in the method 
        /// for simplification purpose.
        /// </remarks>
        /// <param name="count">Inventory count for each product</param>
        private void PrepareInventory(int count)
        {
            _inventoryList = new List<IProduct>
                {
                    new Product(Constants.Coke, 65, count),
                    new Product(Constants.Fanta, 45, count),
                    new Product(Constants.Redbull, 85, count),
                    new Product(Constants.Sprite, 92, count),
                    new Product(Constants.Tango, 163, count)
                };
        }

        /// <summary>
        /// Initializes the denominations. Denominations are restricted to 1P till 50P (assumption for simplification)
        /// </summary>
        /// <param name="denominationValues">List of denomination values</param>
        /// <remarks>Makes the denominations <paramref name="denominationValues"/> out of 1P, 2P, 5P, 10P, 20P and 50P.</remarks>
        private void AssignDenominations(IEnumerable<int> denominationValues)
        {
            var denominations = new List<Denomination>();
            foreach (var denominationValue in denominationValues)
            {
                switch (denominationValue)
                {
                    case 1:
                        denominations.Add(new Denomination(denominationValue, Constants.OnePDenominationCount));
                        break;

                    case 2:
                        denominations.Add(new Denomination(denominationValue, Constants.TwoPDenominationCount));
                        break;

                    case 5:
                        denominations.Add(new Denomination(denominationValue, Constants.FiftyPDenominationCount));
                        break;

                    case 20:
                        denominations.Add(new Denomination(denominationValue, Constants.TwentyPDenominationCount));
                        break;

                    case 10:
                        denominations.Add(new Denomination(denominationValue, Constants.TenPDenominationCount));
                        break;

                    case 50:
                        denominations.Add(new Denomination(denominationValue, Constants.FiftyPDenominationCount));
                        break;
                }
            }

            // update the instance variable
            _initialBalance = denominations;
        }
    }
}
