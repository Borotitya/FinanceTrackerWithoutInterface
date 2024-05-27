using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FinanceTrackerWithoutClasses
{
    static class Program
    {
        static double income = 0;
        static List<Tuple<string, double>> transactions = new List<Tuple<string, double>>();
        static List<string> categories = new List<string>();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static void SetIncome(double newIncome)
        {
            income = newIncome;
        }

        static void AddTransaction(string category, double amount)
        {
            double totalSpent = CalculateTotal();
            if (totalSpent + amount > income)
            {
                ShowNotification("Сумма транзакции не может превышать доход");
                return;
            }
            transactions.Add(new Tuple<string, double>(category, amount));
        }

        static double CalculateTotal()
        {
            return transactions.Sum(t => t.Item2);
        }

        static List<Tuple<string, double>> GetTransactions()
        {
            return transactions;
        }

        static double GetIncome()
        {
            return income;
        }

        static void AddCategory(string category)
        {
            if (!categories.Contains(category))
            {
                categories.Add(category);
            }
        }

        static List<string> GetCategories()
        {
            return categories;
        }

        static void ShowNotification(string message)
        {
            MessageBox.Show(message, "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        class MainForm : Form
        {
            private ComboBox categoryComboBox;
            private TextBox amountTextBox;
            private TextBox incomeTextBox;
            private Button addTransactionButton;
            private Button showTransactionsButton;
            private Button showSummaryButton;
            private ListBox transactionListBox;
            private Label totalLabel;

            public MainForm()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                Text = "Отслеживание финансов";
                Size = new System.Drawing.Size(400, 400);

                categoryComboBox = new ComboBox
                {
                    Location = new System.Drawing.Point(10, 10),
                    Size = new System.Drawing.Size(200, 25),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                categoryComboBox.Items.AddRange(new string[] { "Развлечения", "Еда", "Транспорт", "Одежда", "Жилье", "Здоровье", "Образование", "Прочее" });
                Controls.Add(categoryComboBox);

                amountTextBox = new TextBox
                {
                    Location = new System.Drawing.Point(10, 45),
                    Size = new System.Drawing.Size(200, 25)
                };
                Controls.Add(amountTextBox);

                incomeTextBox = new TextBox
                {
                    Location = new System.Drawing.Point(10, 80),
                    Size = new System.Drawing.Size(200, 25),
                    Text = "Введите ваш заработок"
                };
                incomeTextBox.Enter += IncomeTextBox_Enter;
                incomeTextBox.Leave += IncomeTextBox_Leave;
                Controls.Add(incomeTextBox);

                addTransactionButton = new Button
                {
                    Location = new System.Drawing.Point(10, 120),
                    Size = new System.Drawing.Size(200, 25),
                    Text = "Добавить транзакцию"
                };
                addTransactionButton.Click += AddTransactionButton_Click;
                Controls.Add(addTransactionButton);

                showTransactionsButton = new Button
                {
                    Location = new System.Drawing.Point(220, 120),
                    Size = new System.Drawing.Size(150, 25),
                    Text = "Показать транзакции"
                };
                showTransactionsButton.Click += ShowTransactionsButton_Click;
                Controls.Add(showTransactionsButton);

                showSummaryButton = new Button
                {
                    Location = new System.Drawing.Point(220, 80),
                    Size = new System.Drawing.Size(150, 25),
                    Text = "Показать сводку"
                };
                showSummaryButton.Click += ShowSummaryButton_Click;
                Controls.Add(showSummaryButton);

                transactionListBox = new ListBox
                {
                    Location = new System.Drawing.Point(10, 160),
                    Size = new System.Drawing.Size(350, 200)
                };
                Controls.Add(transactionListBox);

                totalLabel = new Label
                {
                    Location = new System.Drawing.Point(10, 370),
                    Size = new System.Drawing.Size(350, 20),
                    Text = $"Общая сумма потраченных денег: {CalculateTotal()}",
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };
                Controls.Add(totalLabel);
            }

            private void AddTransactionButton_Click(object sender, EventArgs e)
            {
                string category = categoryComboBox.SelectedItem?.ToString() ?? "";
                if (!double.TryParse(amountTextBox.Text, out double amount))
                {
                    MessageBox.Show("Пожалуйста, введите корректную сумму.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(incomeTextBox.Text, out double income))
                {
                    MessageBox.Show("Пожалуйста, введите корректный заработок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SetIncome(income);
                AddTransaction(category, amount);
                UpdateTransactionsListBox();
                totalLabel.Text = $"Общая сумма потраченных денег: {CalculateTotal()}";
            }

            private void UpdateTransactionsListBox()
            {
                transactionListBox.Items.Clear();
                foreach (var transaction in GetTransactions())
                {
                    transactionListBox.Items.Add($"Категория: {transaction.Item1}, Сумма: {transaction.Item2}");
                }
            }

            private void ShowTransactionsButton_Click(object sender, EventArgs e)
            {
                TransactionForm transactionForm = new TransactionForm();
                transactionForm.Show();
            }

            private void ShowSummaryButton_Click(object sender, EventArgs e)
            {
                SummaryForm summaryForm = new SummaryForm();
                summaryForm.Show();
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
                if (incomeTextBox.Text == "")
                {
                    incomeTextBox.Text = "Введите ваш заработок";
                }
            }
        }

        class TransactionForm : Form
        {
            public TransactionForm()
            {
                Text = "Список транзакций";
                Size = new System.Drawing.Size(400, 400);

                ListBox transactionListBox = new ListBox
                {
                    Location = new System.Drawing.Point(10, 10),
                    Size = new System.Drawing.Size(360, 340)
                };
                foreach (var transaction in GetTransactions())
                {
                    transactionListBox.Items.Add($"Категория: {transaction.Item1}, Сумма: {transaction.Item2}");
                }
                Controls.Add(transactionListBox);
            }
        }

        class SummaryForm : Form
        {
            public SummaryForm()
            {
                Text = "Сводка";
                Size = new System.Drawing.Size(300, 200);

                double totalIncome = GetIncome();
                double totalSpent = CalculateTotal();
                double remaining = totalIncome - totalSpent;

                Label totalIncomeLabel = new Label
                {
                    Location = new System.Drawing.Point(10, 10),
                    Size = new System.Drawing.Size(250, 25),
                    Text = $"Общий доход: {totalIncome}"
                };
                Controls.Add(totalIncomeLabel);

                Label totalSpentLabel = new Label
                {
                    Location = new System.Drawing.Point(10, 40),
                    Size = new System.Drawing.Size(250, 25),
                    Text = $"Общие расходы: {totalSpent}"
                };
                Controls.Add(totalSpentLabel);

                Label remainingLabel = new Label
                {
                    Location = new System.Drawing.Point(10, 70),
                    Size = new System.Drawing.Size(250, 25),
                    Text = $"Остаток: {remaining}"
                };
                Controls.Add(remainingLabel);
            }
        }
    }
}
