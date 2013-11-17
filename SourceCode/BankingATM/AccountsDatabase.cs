using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BankingATM
{
    public class AccountsDatabase
    {
        private Hashtable m_ht;

        public AccountsDatabase()
        {
            m_ht = new Hashtable();
        }

        public string AddAccount(BankAccount account)
        {
            if (!(m_ht.ContainsKey(account.AccountNumber)))
            {
                // Key-AccountNumber of type string
                // Value-Account of type BankAccount
                m_ht.Add(account.AccountNumber, account);
                return account.AccountNumber;
            }
            return "0";  // If hashtable contains key already
        }

        public string RemoveAccount(string accountNumber)
        {
            if (m_ht.ContainsKey(accountNumber))
            {
                m_ht.Remove(accountNumber);
                return accountNumber;
            }
            return "0"; // If hashtable does not contain key
        }

        public BankAccount LoginAccount(string accountNumber, string pin)
        {
            if (m_ht.ContainsKey(accountNumber))
            {
                BankAccount account = (BankAccount)m_ht[accountNumber];
                if (account.PinNumber == pin)
                {
                    return account;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        // This method retrieves the account if it exist
        public BankAccount RetrieveAccount(string accountNumber)
        {
            if (m_ht.ContainsKey(accountNumber))
            {
                return (BankAccount)m_ht[accountNumber];
            }
            return null;
        }
    }
}
