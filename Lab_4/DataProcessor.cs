using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqPlinqTask
{
    public class DataProcessor
    {
        // Генерация массива целых чисел
        public int[] GenerateData(int n)
        {
            if (n <= 0) return Array.Empty<int>();

            Random rnd = new Random();
            int[] data = new int[n];
            for (int i = 0; i < n; i++)
            {
                // Генерируем числа от 0 до 10 000 000, чтобы с шансом выпало "666"
                data[i] = rnd.Next(0, 10000000);
            }
            return data;
        }

        // 1. Четные числа (по возрастанию)
        public List<int> GetEvenNumbers(int[] data, bool usePlinq = false)
        {
            var query = usePlinq ? data.AsParallel() : data.AsEnumerable();
            return query.Where(x => x % 2 == 0)
                        .OrderBy(x => x) // Сортировка по возрастанию
                        .ToList();
        }

        // 2. Нечетные числа (по возрастанию)
        public List<int> GetOddNumbers(int[] data, bool usePlinq = false)
        {
            var query = usePlinq ? data.AsParallel() : data.AsEnumerable();
            return query.Where(x => x % 2 != 0)
                        .OrderBy(x => x)
                        .ToList();
        }

        // 3. Сумма первой и последней цифры = 6 (по возрастанию)
        public List<int> GetFirstAndLastSumEqualsSix(int[] data, bool usePlinq = false)
        {
            var query = usePlinq ? data.AsParallel() : data.AsEnumerable();
            return query.Where(x =>
            {
                string s = x.ToString();
                int firstDigit = s[0] - '0';
                int lastDigit = s[s.Length - 1] - '0';
                return (firstDigit + lastDigit) == 6;
            })
                        .OrderBy(x => x)
                        .ToList();
        }

        // 4. Содержит комбинацию "666" (по возрастанию)
        public List<int> GetContains666(int[] data, bool usePlinq = false)
        {
            var query = usePlinq ? data.AsParallel() : data.AsEnumerable();
            return query.Where(x => x.ToString().Contains("666"))
                        .OrderBy(x => x)
                        .ToList();
        }
    }
}