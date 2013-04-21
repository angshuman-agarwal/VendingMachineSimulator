#region License
// Copyright (c) Angshuman Agarwal, All rights reserved.
// See License.txt in the project root for license information.
#endregion

using System;

namespace VendingMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            // Case : Machine is initialised with 1 quantity of each product. use the same instance throughout
            var machine = new VendingMachineClient(1);

            Scenario_1(machine);
            
            // buy Coke again. This time error message should be displayed for 2 insert coins actions
            Scenario_1(machine);

            // user buys a Tango worth 163 P and inserts only 50P
            // should get a message saying balance is remaining
            Scenario_2(machine);

            // user continues to buy Tango 
            Scenario_3(machine);

            Console.ReadLine();
        }
        
        /// <summary>
        /// Normal flow. User should get back change of 35P in an optimal way
        /// </summary>
        /// <param name="machine"></param>
        private static void Scenario_1(IVendingMachineClient machine)
        {
            Console.WriteLine("**********Scenario 1**************\n");
            machine.SelectProduct("Coke");
            machine.InsertCoin(50);
            machine.InsertCoin(50);
            Console.WriteLine("\n**********END**************");
        }

        /// <summary>
        /// User buys a costly item but inserts less money at first 
        /// </summary>
        /// <param name="machine"></param>
        private static void Scenario_2(IVendingMachineClient machine)
        {
            Console.WriteLine("**********Scenario 2**************\n");
            machine.SelectProduct("Tango");
            machine.InsertCoin(50);
            Console.WriteLine("\n**********END**************");
        }

        /// <summary>
        /// Continuing from Scenario 2, user inserts rest of the money and gets 7P back
        /// Note that the user enters in 20Ps.
        /// </summary>
        /// <param name="machine"></param>
        private static void Scenario_3(IVendingMachineClient machine)
        {
            Console.WriteLine("\n**********Scenario 3**************\n");
            Console.WriteLine("...continuing from Scenario 2\n");
            machine.InsertCoin(20);
            machine.InsertCoin(20);
            machine.InsertCoin(20);
            machine.InsertCoin(20);
            machine.InsertCoin(20);
            machine.InsertCoin(20);
            Console.WriteLine("\n**********END**************");
        }
    }
}
