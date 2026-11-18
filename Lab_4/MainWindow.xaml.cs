using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LinqPlinqTask
{
    public partial class MainWindow : Window
    {
        private readonly DataProcessor _processor = new DataProcessor();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtN.Text, out int n) || n <= 0)
            {
                MessageBox.Show("Введите корректное положительное число N.");
                return;
            }

            TxtOutput.Text = "Генерация данных и выполнение запросов...";
            BtnRun.IsEnabled = false;

            string result = await Task.Run(() => RunAllQueries(n));

            TxtOutput.Text = result;
            BtnRun.IsEnabled = true;
        }

        private string RunAllQueries(int n)
        {
            StringBuilder sb = new StringBuilder();
            int[] data = _processor.GenerateData(n);

            sb.AppendLine($"Сгенерирован массив из {n} целых чисел.");
            sb.AppendLine(new string('=', 60));

            Stopwatch sw = new Stopwatch();

            // --- ЗАПРОС 1: Четные числа ---
            sw.Restart();
            var q1Linq = _processor.GetEvenNumbers(data, usePlinq: false);
            sw.Stop();
            long q1LinqTime = sw.ElapsedMilliseconds;

            sw.Restart();
            var q1Plinq = _processor.GetEvenNumbers(data, usePlinq: true);
            sw.Stop();
            long q1PlinqTime = sw.ElapsedMilliseconds;
            AppendResult(sb, "1. Все четные числа", q1LinqTime, q1PlinqTime, q1Linq);

            // --- ЗАПРОС 2: Нечетные числа ---
            sw.Restart();
            var q2Linq = _processor.GetOddNumbers(data, usePlinq: false);
            sw.Stop();
            long q2LinqTime = sw.ElapsedMilliseconds;

            sw.Restart();
            var q2Plinq = _processor.GetOddNumbers(data, usePlinq: true);
            sw.Stop();
            long q2PlinqTime = sw.ElapsedMilliseconds;
            AppendResult(sb, "2. Все нечетные числа", q2LinqTime, q2PlinqTime, q2Linq);

            // --- ЗАПРОС 3: Сумма первой и последней = 6 ---
            sw.Restart();
            var q3Linq = _processor.GetFirstAndLastSumEqualsSix(data, usePlinq: false);
            sw.Stop();
            long q3LinqTime = sw.ElapsedMilliseconds;

            sw.Restart();
            var q3Plinq = _processor.GetFirstAndLastSumEqualsSix(data, usePlinq: true);
            sw.Stop();
            long q3PlinqTime = sw.ElapsedMilliseconds;
            AppendResult(sb, "3. Сумма первой и последней цифры = 6", q3LinqTime, q3PlinqTime, q3Linq);

            // --- ЗАПРОС 4: Содержит "666" ---
            sw.Restart();
            var q4Linq = _processor.GetContains666(data, usePlinq: false);
            sw.Stop();
            long q4LinqTime = sw.ElapsedMilliseconds;

            sw.Restart();
            var q4Plinq = _processor.GetContains666(data, usePlinq: true);
            sw.Stop();
            long q4PlinqTime = sw.ElapsedMilliseconds;
            AppendResult(sb, "4. Числа, содержащие '666'", q4LinqTime, q4PlinqTime, q4Linq);

            return sb.ToString();
        }

        private void AppendResult(StringBuilder sb, string title, long linqTime, long plinqTime, System.Collections.Generic.List<int> results)
        {
            sb.AppendLine($"--- {title} ---");
            sb.AppendLine($"Время LINQ:  {linqTime} мс");
            sb.AppendLine($"Время PLINQ: {plinqTime} мс");
            sb.AppendLine($"Найдено чисел: {results.Count}");

            sb.Append("Числа (по возрастанию): ");
            if (results.Count == 0)
            {
                sb.AppendLine("Нет совпадений.");
            }
            else
            {
                int displayCount = Math.Min(results.Count, 30); // Выводим первые 30 для читаемости
                sb.AppendLine(string.Join(", ", results.GetRange(0, displayCount)) + (results.Count > 30 ? " ..." : ""));
            }
            sb.AppendLine(new string('=', 60));
        }
    }
}