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
        private readonly FinanceTracker financeTracker = new FinanceTracker();
        private ComboBox categoryComboBox;
        private TextBox amountTextBox;
        private TextBox incomeTextBox;
        private Button addTransactionButton;
        private Label totalLabel;
        private ListBox transactionListBox;

        public MainForm()
        {
            InitializeComponents();
        }
        private void InitializeComponents()
        {
            Text = "Отслеживание финансов";
            Size = new System.Drawing.Size(800, 500);

            categoryComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            categoryComboBox.Items.AddRange(new string[] { "Развлечения", "Еда", "Транспорт", "Одежда", "Жилье", "Образование", "Прочее" });
            Controls.Add(categoryComboBox);

            amountTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 45),
                Size = new System.Drawing.Size(200, 30)
            };
            Controls.Add(amountTextBox);

            incomeTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 80),
                Size = new System.Drawing.Size(200, 30),
                Text = "Введите ваш заработок"
            };
            incomeTextBox.Enter += IncomeTextBox_Enter;
            incomeTextBox.Leave += IncomeTextBox_Leave;
            Controls.Add(incomeTextBox);

            addTransactionButton = new Button
            {
                Location = new System.Drawing.Point(10, 115),
                Size = new System.Drawing.Size(200, 30),
                Text = "Добавить транзакцию"
            };
            addTransactionButton.Click += AddTransactionButton_Click;
            Controls.Add(addTransactionButton);

            transactionListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 150),
                Size = new System.Drawing.Size(200, 200)
            };
            Controls.Add(transactionListBox);

            totalLabel = new Label
            {
                Location = new System.Drawing.Point(10, 355),
                Size = new System.Drawing.Size(200, 30),
                Text = $"Общая сумма потраченных денег: {financeTracker.calculateTotal()}",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            Controls.Add(totalLabel);
        }
        private void AddTransactionButton_Click(object sender, EventArgs e)
        {
           string category = categoryComboBox.SelectedItem.ToString();
            double amount = 0;

            if (!double.TryParse(amountTextBox.Text, out amount))
            {
                MessageBox.Show("Введите корректную сумму"  , "Ошибка");
                return;
            }
            double income = 0;
            if(!double.TryParse(incomeTextBox.Text, out income))
            {
                MessageBox.Show("Введите корректный заработок", "Ошибка");
                return;
            }
            financeTracker.setIncome(income);
            financeTracker.addTransaction(category, amount);

            updateTransactionListBox();
            totalLabel.Text = $"Общая сумма потраченных денег: {financeTracker.calculateTotal()}";
        }
        private void updateTransactionListBox()
        {
            transactionListBox.Items.Clear();
            foreach (var transaction in financeTracker.getTransaction())
            {
                transactionListBox.Items.Add($"Кфтегория: {transaction.category}, Сумма: {transaction.amount}");
            }
        }

        private void IncomeTextBox_Enter(object sender, EventArgs e)
        {
            if (incomeTextBox.Text == "Введите ваш заработок")
            {
                incomeTextBox.Text = "";
            }
        }
        private void IncomeTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(incomeTextBox.Text))
            {
                incomeTextBox.Text = "Введите ваш заработок";
            }
        }
        
    }
}
