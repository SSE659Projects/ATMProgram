using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingATM.tests
{
    using NUnit.Framework;

    [TestFixture]
    public class BankAccountTests
    {
        public BankAccount account;
        public AccountsDatabase accountsDatabase;

        [SetUp]
        public void Setup()
        {
            accountsDatabase = new AccountsDatabase();
            account = new BankAccount();
            account.Balance = 100;
        }

        [Test]
        public void TestDeposit()
        {
            account.Deposit(50);
            account.Deposit(75);
            Assert.AreEqual(225, account.Balance);
        }

        [Test]
        public void TestWithdraw()
        {
            account.Withdraw(75);
            account.Withdraw(20);
            Assert.AreEqual(5, account.Balance);
        }

        [TestCase(50, Result = true)]
        [TestCase(99, Result = true)]
        [TestCase(100, Result = true)]
        [TestCase(101, Result = false)]
        [TestCase(200, Result = false)]
        public bool TestWithdrawInsufficientFund(int amount)
        {
            return account.Withdraw(amount);
        }

        // Account Number must have 6 digits
        [TestCase("12", Result = false)]          // 2 digit account number test case
        [TestCase("12345", Result = false)]       // 5 digit account number test case
        [TestCase("123456", Result = true)]       // 6 digit account number test case
        [TestCase("1234567", Result = false)]     // 7 digit account number test case
        public bool IsAccountNumberValid(string sAccountNumber)
        {
            AccountManager accountManager = new AccountManager();

            return accountManager.IsAccountNumberValid(sAccountNumber);
        }

        // Pin Number must have 4 digits
        [TestCase("123", Result = false)]     // 3 digit pin number test case
        [TestCase("1234", Result = true)]     // 4 digit pin number test case
        [TestCase("12345", Result = false)]   // 5 digit pin number test case
        public bool IsPinNumberValid(string sPinNumber)
        {
            AccountManager accountManager = new AccountManager();
            return accountManager.IsPinNumberValid(sPinNumber);
        }
   
        // Test creating a bank account with just the account number
        [Test]
        public void TestCreateBankAccountWithAccountNumber()
        {
            BankAccount bankAccount = new BankAccount("245897");
            Assert.AreEqual("245897", bankAccount.AccountNumber);
        }

        // Test creating a bank account with pin number
        [Test]
        public void TestCreateBankAccountWithPinNumber()
        {
            BankAccount bankAccount = new BankAccount("245897", "1234");
            Assert.AreEqual("1234", bankAccount.PinNumber);
        }

        // Test creating a bank account with initial balance
        [Test]
        public void TestCreateBankAccountWithInitialBalance()
        {
            BankAccount bankAccount = new BankAccount("245897", "1234", 200);
            Assert.AreEqual(200, bankAccount.Balance);
        }

        [Test]
        public void TestAddAccountToDatabase()
        {
            BankAccount bankAccount = new BankAccount("245897", "1234", 200);
            // AddAccount() Method takes in as argument: object of type BankAccount
            string accountNumberAdded = accountsDatabase.AddAccount(bankAccount);
            Assert.AreEqual("245897", accountNumberAdded);
        }

        [Test]
        public void TestAddDuplicateAccountsToDatabase()
        {
            BankAccount bankAccount1 = new BankAccount("245897", "1234", 200);
            BankAccount bankAccount2 = new BankAccount("245897", "1234", 200);
            // AddAccount() Method takes in as argument: object of type BankAccount
            accountsDatabase.AddAccount(bankAccount1);
            string accountNumberAdded = accountsDatabase.AddAccount(bankAccount2);
            Assert.AreEqual("0", accountNumberAdded);
        }

        [Test]
        public void TestRemoveAccountFromDatabase()
        {
            BankAccount bankAccount = new BankAccount("145896");
            accountsDatabase.AddAccount(bankAccount);
            // RemoveAccount() Method takes in as argument: account number to remove
            string accountNumberRemoved = accountsDatabase.RemoveAccount("145896");
            Assert.AreEqual("145896", accountNumberRemoved);
        }

        [Test]
        public void TestRemoveNonExistenceAccountFromDatabase()
        {
            // RemoveAccount() Method takes in as argument: account number to remove
            string accountNumberRemoved = accountsDatabase.RemoveAccount("554453");
            Assert.AreEqual("0", accountNumberRemoved);
        }

        [TestCase("112233", "1234", Result = "112233")]
        [TestCase("445566", "5555", Result = "445566")]
        public string TestLoginAccountFromDatabase(string accountNumber, string pin)
        {
            BankAccount bankAccount = new BankAccount(accountNumber, pin);
            accountsDatabase.AddAccount(bankAccount);
            // LoginAccount() Method takes in as argument: account number, pin number
            BankAccount loggedInBankAccount = accountsDatabase.LoginAccount(accountNumber, pin);
            return loggedInBankAccount.AccountNumber;
        }

        [Test]
        public void TestUnsuccessfulLoginAccountFromDatabase()
        {
            // LoginAccount() Method takes in as argument: account number, pin number
            BankAccount loggedInBankAccount = accountsDatabase.LoginAccount("223344", "5678");
            Assert.AreEqual(null, loggedInBankAccount);
        }

        [Test]
        public void TestAddAccountByManager()
        {
            BankAccount bankAccount = new BankAccount("445566", "1234", 500);
            AccountManager accountManager = new AccountManager();
            string sAccountNumberAdded = accountManager.AddBankAccount(bankAccount);
            Assert.AreEqual("445566", sAccountNumberAdded);
        }

        [Test]
        public void TestRemoveAccountByManager()
        {
            BankAccount bankAccount = new BankAccount("012456", "4444", 600);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount);
            string sAccountNumberRemoved = accountManager.RemoveBankAccount("012456");
            Assert.AreEqual("012456", sAccountNumberRemoved);
        }

        [Test]
        public void TestLoginAccountByManager()
        {
            BankAccount bankAccount = new BankAccount("789456", "7777", 700);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount);
            bool bResult = accountManager.Login("789456", "7777");
            Assert.AreEqual(true, bResult);
        }

        [Test]
        public void TestViewAccountBalanceByManager()
        {
            BankAccount bankAccount = new BankAccount("789456", "7777", 700);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount);
            bool bResult = accountManager.Login("789456", "7777");
            int balance = -1;
            if(bResult)
                balance = accountManager.GetAccountBalance();
            Assert.AreEqual(700, balance);
        }

        //*[TestCase("1500", Result = false)]
        [TestCase("1001", Result = false)]
        [TestCase("1000", Result = true)]
        [TestCase("999", Result = true)]
        //[TestCase("500", Result = true)]
        [TestCase("1", Result = true)]
        [TestCase("0", Result = true)]
        [TestCase("-1", Result = false)]
        //[TestCase("-500", Result = false)]
        //[TestCase("abc", Result = false)]   // Garbage Testing
        //[TestCase("", Result = false)]      // Null Testing
        public bool TestValidDepositAmountByManager(string sAmount)
        {
            int iAmount = 0;
            AccountManager accountManager = new AccountManager();
            return accountManager.IsTransactionAmountValid(AccountManager.ETransactionType.E_DEPOSIT, sAmount, out iAmount);
        }


        [TestCase("501", Result = false)]
        [TestCase("500", Result = true)]
        [TestCase("499", Result = true)]
        [TestCase("1", Result = true)]
        [TestCase("0", Result = true)]
        [TestCase("-1", Result = false)]
        public bool TestValidWithdrawAmountByManager(string sAmount)
        {
            int iAmount = 0;
            AccountManager accountManager = new AccountManager();
            return accountManager.IsTransactionAmountValid(AccountManager.ETransactionType.E_WITHDRAW, sAmount, out iAmount);
        }

        [Test]
        public void TestDepositByManager()
        {
            BankAccount bankAccount = new BankAccount("789456", "7777", 700);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount);
            bool bResult = accountManager.Login("789456", "7777");
            int balance = -1;
            if (bResult)
            {
                balance = accountManager.Deposit(50);
            }
            Assert.AreEqual(750, balance);
        }

        [Test]
        public void TestWithdrawByManager()
        {
            BankAccount bankAccount = new BankAccount("789456", "7777", 700);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount);
            bool bResult = accountManager.Login("789456", "7777");
            int balance = -1;
            if (bResult)
            {
                balance = accountManager.Withdraw(100);
            }
            Assert.AreEqual(600, balance);
        }

        [TestCase("789456", Result = true)]
        [TestCase("012456", Result = true)]
        [TestCase("112233", Result = false)]
        public bool TestAccountExists(string sAccountNumber)
        {
            BankAccount bankAccount1 = new BankAccount("789456", "7777", 100);
            BankAccount bankAccount2 = new BankAccount("012456", "4444", 200);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount1);
            accountManager.AddBankAccount(bankAccount2);
            //accountManager.Login("789456", "7777");
            return accountManager.AccountExists(sAccountNumber);
        }

        // Testing for valid transfer amount when current balance is not a factor
        //[TestCase("500", Result = false)]
        [TestCase("201", Result = false)]
        [TestCase("200", Result = true)]
        [TestCase("199", Result = true)]
        //[TestCase("100", Result = true)]
        [TestCase("1", Result = true)]
        [TestCase("0", Result = true)]
        [TestCase("-1", Result = false)]
        //[TestCase("-500", Result = false)]
        //[TestCase("abc", Result = false)]   // Garbage Testing
        //[TestCase("", Result = false)]      // Null Testing
        public bool TestValidTransferAmountByManager(string sAmount)
        {
            int iAmount = 0;
            BankAccount bankAccount = new BankAccount("789456", "7777", 700);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount);
            if (accountManager.Login("789456", "7777"))
                return accountManager.IsTransactionAmountValid(AccountManager.ETransactionType.E_TRANSFER, sAmount, out iAmount);
            else
                return false;
        }

        // Testing for valid transfer amount when current balance is a factor
        //[TestCase("201", Result = false)]
        [TestCase("101", Result = false)]
        [TestCase("100", Result = true)]
        [TestCase("99", Result = true)]
        public bool TestValidTransferAmountDependentCurrentBalanceByManager(string sAmount)
        {
            int iAmount = 0;
            BankAccount bankAccount = new BankAccount("789456", "7777", 100);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount);
            if (accountManager.Login("789456", "7777"))
                return accountManager.IsTransactionAmountValid(AccountManager.ETransactionType.E_TRANSFER, sAmount, out iAmount);
            else
                return false;
        }

        [Test]
        public void TestTransferByManager()
        {
            int iAmount = 50;
            BankAccount bankAccount1 = new BankAccount("789456", "7777", 100);
            BankAccount bankAccount2 = new BankAccount("012456", "4444", 200);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount1);
            accountManager.AddBankAccount(bankAccount2);
            if (accountManager.Login("789456", "7777"))
            {
                accountManager.Transfer(bankAccount2.AccountNumber, iAmount);
            }
            Assert.AreEqual(50, bankAccount1.Balance);  // Logged in account balance
            Assert.AreEqual(250, bankAccount2.Balance); // Target account balance
        }

        // Test if account is an administrator
        [TestCase(true, Result = true)]
        [TestCase(false, Result = false)]
        public bool TestIsAdministratorAccountByManager(bool bIsAdministrator)
        {

            BankAccount bankAccount = new BankAccount("223344", "1234", 0, bIsAdministrator);
            AccountManager accountManager = new AccountManager();
            accountManager.AddBankAccount(bankAccount);
            accountManager.Login("223344", "1234");
            return accountManager.IsAdministrator();
        }
         
    } // end BankAccountTests
}
