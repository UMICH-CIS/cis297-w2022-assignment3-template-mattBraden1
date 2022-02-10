using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{
    /// <summary>
    /// Matthew Braden, Winter 2022
    /// Base class for a bank account, gives the account a balance
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// gives the account a balance
        /// </summary>
        /// <param name="balance">decimal value >= 0</param>
        public Account(decimal balance)
        {
            Balance = balance;
        }
        private decimal balance;
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// get and set methods for balance, 
        /// throws an exception if set to a value less than 0
        /// </summary>
        public decimal Balance
        {
            get
            {
                return balance;
            }
            set
            {
                if (value < 0.0M)
                    throw new ArgumentException("The given value is less than 0");
                balance = value;
            }
        }
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// adds the specified amount to the balance
        /// </summary>
        /// <param name="amount">decimal value</param>
        public virtual void Credit(decimal amount)
        {
            balance += amount;
        }
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// debits the amount from the balance if it can
        /// </summary>
        /// <param name="amount">decimal value</param>
        /// <returns>true if balance has been debited</returns>
        public virtual bool Debit(decimal amount)
        {
            if (balance >= amount)
            {
                balance -= amount;
                return true;
            }
            else
            {
                Console.WriteLine("Debit amount exceeded account balance.");
                return false;
            }
        }
    }
    /// <summary>
    /// Matthew Braden, Winter 2022
    /// Savings account derrived from Account,
    /// has interest
    /// </summary>
    public class SavingsAccount:Account
    {
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// contructs Account and gives interest a value
        /// </summary>
        /// <param name="balance">decimal value >= 0</param>
        /// <param name="interest">value denoting % interest in decimal form</param>
        public SavingsAccount(decimal balance, decimal interest):base(balance)
        {
            this.interest = interest;
        }
        private decimal interest;
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// Calculates the interest given by the current balance and the rate
        /// </summary>
        /// <returns>total interest for a transaction</returns>
        public decimal CalculateInterest()
        {
            return Balance * interest;
        }
    }
    /// <summary>
    /// Matthew Braden, Winter 2022
    /// Checking account derrived from Account,
    /// adds transaction fees
    /// </summary>
    public class CheckingAccount:Account
    {
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// constructs Account and gives fee a value
        /// </summary>
        /// <param name="balance">decimal value >= 0</param>
        /// <param name="fee">fee that will be subtracted per transaction</param>
        public CheckingAccount(decimal balance, decimal fee) : base(balance)
        {
            this.fee = fee;
        }
        private decimal fee;
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// Account's credit method with a fee charged
        /// </summary>
        /// <param name="amount">a decimal value</param>
        public override void Credit(decimal amount)
        {
            base.Credit(amount);
            Balance -= fee;
        }
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// Account's debit method with a fee charged
        /// </summary>
        /// <param name="amount">a decimal value</param>
        /// <returns>true if balance has been debited</returns>
        public override bool Debit(decimal amount)
        {
            if (base.Debit(amount))
            {
                Balance -= fee;
                return true;
            }
            return false;
        }
    }
    class Program
    {
        // Processing Accounts polymorphically.
        // given test code
        static void Main(string[] args)
        {
            // create array of accounts
            Account[] accounts = new Account[4];

            // initialize array with Accounts
            accounts[0] = new SavingsAccount(25M, .03M);
            accounts[1] = new CheckingAccount(80M, 1M);
            accounts[2] = new SavingsAccount(200M, .015M);
            accounts[3] = new CheckingAccount(400M, .5M);

            // loop through array, prompting user for debit and credit amounts
            for (int i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine($"Account {i + 1} balance: {accounts[i].Balance:C}");

                Console.Write($"\nEnter an amount to withdraw from Account {i + 1}: ");
                decimal withdrawalAmount = decimal.Parse(Console.ReadLine());

                accounts[i].Debit(withdrawalAmount); // attempt to debit

                Console.Write($"\nEnter an amount to deposit into Account {i + 1}: ");
                decimal depositAmount = decimal.Parse(Console.ReadLine());

                // credit amount to Account
                accounts[i].Credit(depositAmount);

                // if Account is a SavingsAccount, calculate and add interest
                if (accounts[i] is SavingsAccount)
                {
                    // downcast
                    SavingsAccount currentAccount = (SavingsAccount)accounts[i];

                    decimal interestEarned = currentAccount.CalculateInterest();
                    Console.WriteLine($"Adding {interestEarned:C} interest to Account {i + 1} (a SavingsAccount)");
                    currentAccount.Credit(interestEarned);
                }

                Console.WriteLine($"\nUpdated Account {i + 1} balance: {accounts[i].Balance:C}\n\n");
            }
        }
    }
}
