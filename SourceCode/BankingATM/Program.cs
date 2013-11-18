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
            // Create administrator account
            BankAccount adminAccount = new BankAccount("123456", "1111", 0, true);
            // Add bank accounts to database
            accountManager.AddBankAccount(bankAccount1);
            accountManager.AddBankAccount(bankAccount2);
            accountManager.AddBankAccount(adminAccount);

            bool bValidInputs = false;
            string sAccountNumber = "";
            string sPinNumber = "";

            // The ATM program continues indefinitely
            while (true)
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

                if (accountManager.IsAdministrator())
                {
                    bool bExitAdministratorTask = false;
                    do
                    {
                        string choice = "";
                        bool bValidChoice = false;
                        do
                        {
                            //Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Select the task to perform:");
                            Console.WriteLine("1- Add Account");
                            Console.WriteLine("2- Delete Account");
                            Console.WriteLine("3- View Account Numbers");
                            Console.WriteLine("4- Exit");

                            choice = Console.ReadLine();

                            if (choice == "1" || choice == "2" || choice == "3" || choice == "4")
                            {
                                bValidChoice = true;
                            }

                        } while (!bValidChoice);

                        if (choice == "1")
                        {
                            bool bAccountValid = false;
                            do
                            {
                                Console.WriteLine("Please enter account information.");
                                Console.WriteLine("Enter the account number: ");
                                string sAccNumber = Console.ReadLine();

                                Console.WriteLine("Enter the pin number: ");
                                string sPin = Console.ReadLine();

                                Console.WriteLine("Enter the balance: ");
                                string sBalance = Console.ReadLine();
                                int iBalance = 0;
                                bool bResult = Int32.TryParse(sBalance, out iBalance);

                                if (!(accountManager.IsAccountNumberValid(sAccNumber)))
                                {
                                    Console.WriteLine("Account number is invalid.");
                                }
                                else if (accountManager.AccountExists(sAccNumber))
                                {
                                    Console.WriteLine("Account number already exists.");
                                }
                                else if (!(accountManager.IsPinNumberValid(sPin)))
                                {
                                    Console.WriteLine("Pin number is invalid.");
                                }
                                else if (bResult && iBalance < 0)
                                {
                                    Console.WriteLine("Balance must be greater or equal to zero.");
                                }
                                else if (!bResult)
                                {
                                    Console.WriteLine("Balance has invalid format.");
                                }
                                else
                                {
                                    bAccountValid = true;
                                    BankAccount account = new BankAccount(sAccNumber, sPin, iBalance);
                                    accountManager.AddBankAccount(account);
                                }

                            } while (!bAccountValid);
                        }
                        else if (choice == "2")
                        {
                            bool bAccountToDeleteValid = false;
                            do
                            {
                                Console.WriteLine("Enter the account number of the account to delete: ");
                                string sAccountNumberToDelete = Console.ReadLine();

                                if (!(accountManager.IsAccountNumberValid(sAccountNumberToDelete)))
                                {
                                    Console.WriteLine("Account number is invalid.");
                                }
                                else if (accountManager.GetAccountNumber() == sAccountNumberToDelete)
                                {
                                    Console.WriteLine("Account number cannot be deleted because it is currently logged in.");
                                }
                                else if (!(accountManager.AccountExists(sAccountNumberToDelete)))
                                {
                                    Console.WriteLine("Account number does not exists.");
                                }
                                else
                                {
                                    bAccountToDeleteValid = true;
                                    accountManager.RemoveBankAccount(sAccountNumberToDelete);
                                }
                            } while (!bAccountToDeleteValid);
                        }
                        else if (choice == "3")
                        {
                            Console.Clear();
                            string sAccountNumbers = "";
                            accountManager.RetrieveAccountNumbers(out sAccountNumbers);
                            Console.WriteLine(sAccountNumbers);
                        }
                        else
                        {
                            bExitAdministratorTask = true;
                        }
                    } while (!bExitAdministratorTask);
                }
                else
                {
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
                            Console.WriteLine("3- Transfer to another account");
                            Console.WriteLine("4- View Balance");

                            choice = Console.ReadLine();

                            if (choice == "1" || choice == "2" || choice == "3" || choice == "4")
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
                                    if (iDepositAmount > AccountManager.DEPOSIT_LIMIT)
                                        Console.WriteLine("Deposit exceeds $" + AccountManager.DEPOSIT_LIMIT.ToString() + " limit.");
                                    else
                                        Console.WriteLine("Deposit amount is invalid.");
                                }
                            } while (!bDepositAmountValid);
                        }
                        else if (choice == "2")
                        {
                            bool bWithdrawAmountValid = false;
                            do
                            {
                                Console.WriteLine("Enter the amount to withdraw: ");
                                string sWithdrawAmount = Console.ReadLine();
                                int iWithdrawAmount = -1;

                                if (accountManager.IsTransactionAmountValid(AccountManager.ETransactionType.E_WITHDRAW, sWithdrawAmount, out iWithdrawAmount))
                                {
                                    if (accountManager.Withdraw(iWithdrawAmount) == -1)
                                    {
                                        Console.WriteLine("Withdraw amount exceeds account balance.");
                                    }
                                    else
                                    {
                                        bWithdrawAmountValid = true;
                                    }
                                }
                                else
                                {
                                    if (iWithdrawAmount > AccountManager.WITHDRAW_LIMIT)
                                        Console.WriteLine("Withdraw amount exceeds $" + AccountManager.WITHDRAW_LIMIT.ToString() + " limit.");
                                    else
                                        Console.WriteLine("Withdraw amount is invalid.");
                                }
                            } while (!bWithdrawAmountValid);
                        }
                        else if (choice == "3")
                        {
                            bool bAccountExists = false;
                            bool bTransferAmountValid = false;

                            Console.WriteLine("Enter the account number to transfer to: ");
                            string sAccount = Console.ReadLine();

                            if (!(accountManager.IsAccountNumberValid(sAccount)) || accountManager.GetAccountNumber() == sAccount)
                            {
                                Console.WriteLine("Account number is invalid.");
                            }
                            else
                            {
                                if (accountManager.AccountExists(sAccount))
                                {
                                    bAccountExists = true;
                                }
                                else
                                {
                                    Console.WriteLine("Account number does not exist.");
                                }
                            }

                            if (bAccountExists)
                            {
                                do
                                {
                                    Console.WriteLine("Enter the amount to transfer: ");
                                    string sTransferAmount = Console.ReadLine();
                                    int iTransferAmount = -1;

                                    if (accountManager.IsTransactionAmountValid(AccountManager.ETransactionType.E_TRANSFER,
                                                                                                    sTransferAmount, out iTransferAmount))
                                    {
                                        bTransferAmountValid = true;
                                        accountManager.Transfer(sAccount, iTransferAmount);
                                    }
                                    else
                                    {
                                        if (accountManager.GetAccountBalance() < iTransferAmount)
                                            Console.WriteLine("Transfer amount exceeds account balance.");
                                        else if (iTransferAmount > AccountManager.TRANSFER_LIMIT)
                                            Console.WriteLine("Transfer amount exceeds $" + AccountManager.TRANSFER_LIMIT.ToString() + " limit.");
                                        else
                                            Console.WriteLine("Transfer amount is invalid.");
                                    }
                                } while (!bTransferAmountValid);
                            }
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

                            if (sResponse != "y" && sResponse != "Y" && sResponse != "n" && sResponse != "N")
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
                }
            } // end main while loop
        }
    }
}
