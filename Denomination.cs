#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// Class for keeping track of the denomination value and quantity
    /// </summary>
    public class Denomination
    {
        /// <summary>
        /// Initialises a denomination object with a value and quantity
        /// </summary>
        /// <param name="value">Denomination Value</param>
        /// <param name="count">Quantity of denomination value</param>
        public Denomination(int value, int count)
        {
            DenominationValue = value;
            DenominationCount = count;
        }

        /// <summary>
        /// Accessor for the Denomination Value
        /// </summary>
        public int DenominationValue { get; set; }

        /// <summary>
        /// Accessor for the Denomination Count
        /// </summary>
        public int DenominationCount { get; set; }
    }
}
