#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

namespace VendingMachine
{
    /// <summary>
    /// A constant class for maintaining the "magic" strings and numbers
    /// </summary>
    public sealed class Constants
    {
        /// <summary>
        /// Constant to denote the item Coke
        /// </summary>
        public static readonly string Coke = "Coke";
        /// <summary>
        /// Constant to denote the item Fanta
        /// </summary>
        public static readonly string Fanta = "Fanta";
        /// <summary>
        /// Constant to denote the item Redbull
        /// </summary>
        public static readonly string Redbull = "Redbull";
        /// <summary>
        /// Constant to denote the item Sprite
        /// </summary>
        public static readonly string Sprite = "Sprite";
        /// <summary>
        /// Constant to denote the item Tango
        /// </summary>
        public static readonly string Tango = "Tango";


        /// <summary>
        /// Constant for storing number of 1P denominations
        /// <remarks>Value is hardcoded to 100</remarks>
        /// </summary>
        public static readonly int OnePDenominationCount = 100;
        /// <summary>
        /// Constant for storing number of 2P denominations
        /// <remarks>Value is hardcoded to 100</remarks>
        /// </summary>
        public static readonly int TwoPDenominationCount = 100;
        /// <summary>
        /// Constant for storing number of 5P denominations
        /// <remarks>Value is hardcoded to 50</remarks>
        /// </summary>
        public static readonly int FivePDenominationCount = 50;
        /// <summary>
        /// Constant for storing number of 10P denominations
        /// <remarks>Value is hardcoded to 5</remarks>
        /// </summary>
        public static readonly int TenPDenominationCount = 5;
        /// <summary>
        /// Constant for storing number of 20P denominations
        /// <remarks>Value is hardcoded to 5</remarks>
        /// </summary>
        public static readonly int TwentyPDenominationCount = 5;
        /// <summary>
        /// Constant for storing number of 50P denominations
        /// <remarks>Value is hardcoded to 2</remarks>
        /// </summary>
        public static readonly int FiftyPDenominationCount = 2;
    }
}
