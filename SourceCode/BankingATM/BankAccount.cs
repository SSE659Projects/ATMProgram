﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingATM
{
    public class BankAccount
    {
        private string m_sAccountNumber;
        private string m_sPinNumber;
        private int m_iBalance;

        public BankAccount()
        {
            AccountNumber = "111111";
            PinNumber = "1111";
            Balance = 0;
        }
        
        public BankAccount(string accountNumber, string pinNumber = "1111", int balance = 0)
        {
            AccountNumber = accountNumber;
            PinNumber = pinNumber;
            Balance = balance;
        }
        
        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public bool Withdraw(int amount)
        {
            if (amount <= m_iBalance)
            {
                Balance -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Balance
        {
            get { return m_iBalance; }
            set { m_iBalance = value; }
        }

        public string AccountNumber
        {
            get { return m_sAccountNumber; }
            set { m_sAccountNumber = value; }
        }

        public string PinNumber
        {
            get { return m_sPinNumber; }
            set { m_sPinNumber = value; }
        }
    }
}
