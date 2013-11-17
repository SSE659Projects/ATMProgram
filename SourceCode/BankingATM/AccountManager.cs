using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingATM
{
    public class AccountManager
    {
        // Use for determining transaction limit
        public enum ETransactionType
        {
            E_DEPOSIT,
            E_WITHDRAW
            //E_TRANSFER
        };

        public const int DEPOSIT_LIMIT = 1000;
        public const int WITHDRAW_LIMIT = 500;
        //public const int TRANSFER_LIMIT = 200;
        const int ACCOUNT_NUMBER_LENGTH = 6;
        const int PIN_NUMBER_LENGTH = 4;

        private AccountsDatabase m_AccountsDatabase;
        private BankAccount m_BankAccount;

        public AccountManager()
        {
            m_AccountsDatabase = new AccountsDatabase();
        }

        public bool IsAccountNumberValid(string sAccountNumber)
        {
            bool bValid = true;

            if (sAccountNumber.Length == ACCOUNT_NUMBER_LENGTH)
            {
                // Test to see that each character is is between 0-9 
                for (int n = 0; n < sAccountNumber.Length; n++)
                {
                    // 0 decimal is ASCII 48; 9 decimal is ASCII 57
                    if (sAccountNumber[n] < 48 || sAccountNumber[n] > 57)
                    {
                        bValid = false;
                    }
                }
            }
            else
            {
                // Account number is not 6 digits in length
                bValid = false;
            }

            return bValid;
        }

        public bool IsPinNumberValid(string sPinNumber)
        {
            bool bValid = true;

            if (sPinNumber.Length == PIN_NUMBER_LENGTH)
            {
                // Test to see that each character is is between 0-9 
                for (int n = 0; n < sPinNumber.Length; n++)
                {
                    // 0 decimal is ASCII 48; 9 decimal is ASCII 57
                    if (sPinNumber[n] < 48 || sPinNumber[n] > 57)
                    {
                        bValid = false;
                    }
                }
            }
            else
            {
                // Pin number is not 4 digits in length
                bValid = false;
            }

            return bValid;
        }
        
        public string AddBankAccount(BankAccount account)
        {
            return m_AccountsDatabase.AddAccount(account);
        }

        public string RemoveBankAccount(string sAccountNumber)
        {
            return m_AccountsDatabase.RemoveAccount(sAccountNumber);
        }

        public bool Login(string sAccountNumber, string sPin)
        {
            m_BankAccount = m_AccountsDatabase.LoginAccount(sAccountNumber, sPin);

            if (m_BankAccount != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetAccountBalance()
        {
            if (m_BankAccount != null)
            {
                return m_BankAccount.Balance;
            }
            else
            {
                return -1;
            }
        }

        // This method only returns true if the transaction amount is valid for the transaction type.
        public bool IsTransactionAmountValid(ETransactionType transactionType, string sAmount, out int iAmount)
        {
            int limit = 0;
            iAmount = 0;
            bool bResult = Int32.TryParse(sAmount, out iAmount);

            // Set limit based on transaction type
            switch (transactionType)
            {
                case ETransactionType.E_DEPOSIT:
                    limit = DEPOSIT_LIMIT;
                    break;
                case ETransactionType.E_WITHDRAW:
                    limit = WITHDRAW_LIMIT;
                    break;
                //case ETransactionType.E_TRANSFER:
                    //limit = TRANSFER_LIMIT;
                    //break;
                default:
                    break;
            }

            // Transfer amount is invalid if account balance is less than the transfer amount.
            //if (bResult && transactionType == ETransactionType.E_TRANSFER && m_BankAccount.Balance < iAmount)
            //    return false;

            if (bResult && iAmount >= 0 && iAmount <= limit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
        public bool IsDepositAmountValid(string sAmount, out int iAmount)
        {
            iAmount = 0;
            bool bResult = Int32.TryParse(sAmount, out iAmount);

            if (bResult && iAmount >= 0 && iAmount <= DEPOSIT_LIMIT)
            {
                return true;
            }
            else
            {
                return false;
            }    
        }

        public bool IsWithdrawAmountValid(string sAmount, out int iAmount)
        {
            iAmount = 0;
            bool bResult = Int32.TryParse(sAmount, out iAmount);

            if (bResult && iAmount >= 0 && iAmount <= WITHDRAW_LIMIT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
         * */
        
        public int Deposit(int iAmount)
        {
            if (m_BankAccount != null)
            {
                m_BankAccount.Deposit(iAmount);
                return m_BankAccount.Balance;
            }
            else
            {
                return -1;
            }
        }

        public int Withdraw(int iAmount)
        {
            if (m_BankAccount != null)
            {
                if (m_BankAccount.Withdraw(iAmount))
                    return m_BankAccount.Balance;
                else
                    return -1;
            }
            else
            {
                return -1;
            }
        }

        public bool AccountExists(string sAccountNumber)
        {
            BankAccount targetAccount = m_AccountsDatabase.RetrieveAccount(sAccountNumber);
            if (targetAccount != null)
            {
                return true;
            }
            else
            {
                return false;
            }
             
            /*
            if (sAccountNumber == "789456")
            {
                return true;
            }
            else
            {
                return false;
            }
             * */
        }
    }
}
