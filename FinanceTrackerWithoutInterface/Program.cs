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
            income = newIncome;
        }

        public void addTransaction(string category, double amount)
        {
            double total = calculateTotal();
            if (total + amount > income)
            {
                showNotification("Сумма транзакции не может превышать доход");
                return;
            }
            transactions.Add(new Transaction(category, amount));
        }

        public double calculateTotal()
        {
            double total = 0;
            foreach (var transaction in transactions)
            {
                total += transaction.amount;
            }
            return total;
        }
        public List<Transaction> getTransaction()
        {
            return transactions;
        }
        public double getIncome()
        {
            return income;
        }

        public void addCategory(string category)
        {
            if(category!= "Общие" && category != "Одиночные")
            {
                categories.Add(category);
            }
        }
        public List<string> getCategories()
        {
            return categories;
        }

        public void showNotification(string message)
        {
            MessageBox.Show(message, "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
    public class MainForm : Form
    {

    }
}
