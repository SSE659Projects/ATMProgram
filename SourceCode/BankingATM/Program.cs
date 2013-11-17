using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingATM
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Account Manager 
            AccountManager accountManager = new AccountManager();

            // Create bank accounts
            BankAccount bankAccount1 = new BankAccount("112233", "1234", 200);
            BankAccount bankAccount2 = new BankAccount("445566", "5566", 500);
            // Add bank accounts to database
            accountManager.AddBankAccount(bankAccount1);
            accountManager.AddBankAccount(bankAccount2);
           
            bool bValidInputs = false;
            string sAccountNumber = "";
            string sPinNumber = "";

            // The ATM program continues indefinitely
            while(true)
            {
                Console.Clear();
                Console.WriteLine("***** Welcome to the Banking ATM Program *****");
                Console.WriteLine();
            do 
            {
                Console.Write("Please enter your account number: ");
                sAccountNumber = Console.ReadLine();

                if (accountManager.IsAccountNumberValid(sAccountNumber))
                {
                    bValidInputs = true;
                }
                else
                {
                    Console.WriteLine("Account number is not valid.");
                }

                if (bValidInputs)
                {
                    Console.Write("Please enter your pin number: ");
                    sPinNumber = Console.ReadLine();
                    if (!accountManager.IsPinNumberValid(sPinNumber))
                    {
                        Console.WriteLine("Pin number is not valid.");
                        bValidInputs = false;
                    }
                }

                if (bValidInputs)
                {
                    if (accountManager.Login(sAccountNumber, sPinNumber))
                    {
                        Console.Clear();
                        Console.WriteLine("Login Valid");
                    }
                    else
                    {
                        Console.WriteLine("Login Invalid");
                        bValidInputs = false;
                    }
                }
            } while (!bValidInputs);

            bool bContinueTransaction = true;
            do 
            {
                string choice = "";
                bool bValidChoice = false;
                do 
                {
                    Console.Clear();
                    Console.WriteLine("Select the transaction to perform:");
                    Console.WriteLine("1- Deposit");
                    Console.WriteLine("2- Withdraw");
                    Console.WriteLine("3- View Balance");

                    choice = Console.ReadLine();

                    if (choice == "1" || choice == "2" || choice == "3")
                    {
                        bValidChoice = true;
                    }
                
                } while (!bValidChoice);

                if (choice == "1")
                {
                    bool bDepositAmountValid = false;
                    do 
                    {
                        Console.WriteLine("Enter the amount to deposit: ");
                        string sDepositAmount = Console.ReadLine();
                        int iDepositAmount = -1;

                        if (accountManager.IsTransactionAmountValid(AccountManager.ETransactionType.E_DEPOSIT, sDepositAmount, out iDepositAmount))
                        {
                            bDepositAmountValid = true;
                            accountManager.Deposit(iDepositAmount);
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid deposit amount.");
                        }
                    } while (!bDepositAmountValid);
                }
                else if (choice == "2")
                {
                    bool bWithdrawAmountValid = false;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Enter the amount to withdraw: ");
                        string sWithdrawAmount = Console.ReadLine();
                        int iWithdrawAmount = -1;

                        if (accountManager.IsTransactionAmountValid(AccountManager.ETransactionType.E_WITHDRAW, sWithdrawAmount, out iWithdrawAmount))
                        {
                            bWithdrawAmountValid = true;
                            if (accountManager.Withdraw(iWithdrawAmount) == -1)
                            {
                                Console.WriteLine("Withdraw amount exceeds account balance.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid withdraw amount.");
                        }
                    } while (!bWithdrawAmountValid);
                }
                else 
                {
                    Console.Clear();
                    Console.WriteLine("Account Balance: $" + accountManager.GetAccountBalance());
                }

                bool bValidResponse = false;
                string sResponse = "";
                do 
                {
                    Console.WriteLine();
                    Console.WriteLine("Do you want to perform another transaction? y/n");
                    sResponse = Console.ReadLine();

                    if(sResponse != "y" && sResponse != "Y" && sResponse != "n" && sResponse != "N")
                    {
                        Console.WriteLine("Enter 'y' or 'n' response");
                    }
                    else 
                    {
                        bValidResponse = true;
                    }
                } while (!bValidResponse);

                if (sResponse == "n" || sResponse == "N")
                {
                    bContinueTransaction = false;
                }

            } while (bContinueTransaction);
            } // end main while loop
        }
    }
}
