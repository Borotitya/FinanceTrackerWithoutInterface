using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinanceTrackerWithoutInterface
{
    public class Transaction //
    {
        public string category { get; } //
        public double amount { get; }

        public Transaction(string category, double amount)
        {
            this.category = category;
            this.amount = amount;
        }
    }

    public class FinanceTracker
    {
        private double income = 0; 
        private List<Transaction> transactions = new List<Transaction>();
        private List<string> categories = new List<string>();

        public void setIncome(double newIncome)
        {

        }
    }
}
