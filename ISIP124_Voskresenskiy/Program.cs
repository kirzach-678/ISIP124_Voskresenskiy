using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ISIP124_Voskresenskiy
{
    internal class Program
    {
        const int MIN_OPERATIONS = 2;
        const int MAX_OPERATIONS = 40;

        static string[] names;
        static double[] prices;
        static int count;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("учет расходов за день");

            int n = ReadOperationsCount();  // спрашиваем количество операций
            ReadExpenses(n);                // вводим сами траты
            RunMenu();                      // работаем с меню

            Console.WriteLine();
            Console.WriteLine("Работа завершена, нажмите любую клавишу");
            Console.ReadKey();
        }

        // ввод количества операций 
        static int ReadOperationsCount()
        {
            while (true)
            {
                Console.Write("Сколько операций хотите внести (от " + MIN_OPERATIONS + " до " + MAX_OPERATIONS + ")? ");
                string input = Console.ReadLine();

                int value;
                // TryParse пытается превратить текст в число и НЕ падает при ошибке,
                if (!int.TryParse(input, out value))
                {
                    Console.WriteLine("Ошибка: нужно ввести целое число.");
                    continue;
                }

                if (value < MIN_OPERATIONS || value > MAX_OPERATIONS)
                {
                    Console.WriteLine("Ошибка: число должно быть от " + MIN_OPERATIONS +
                                      " до " + MAX_OPERATIONS + ".");
                    continue;
                }

                return value;
            }
        }

        // ввод трат
        static void ReadExpenses(int n)
        {
            names = new string[n];
            prices = new double[n];
            count = 0;

            Console.WriteLine("Вводите траты по шаблону:  Название; Сумма");
            Console.WriteLine("Пример:  Влажные салфетки \"Лента\"; 235");
            Console.WriteLine("Валюта - рубли, пробную часть можно писать через запятую или точку.");

            while (count < n) // пока не набрали нужное количество трат
            {
                Console.Write("Трата №" + (count + 1) + ": ");
                string line = Console.ReadLine();

                string name;
                double price;

                if (!TryParseExpense(line, out name, out price))
                {
                    Console.WriteLine("Не понял ввод. Нужен формат: Название; Сумма  (сумма - неотрицательное число)");
                    continue; // не увеличиваем count - строку просят ввести заново
                }

                names[count] = name;
                prices[count] = price;
                count++;
            }

            Console.WriteLine();
            Console.WriteLine("Все траты записаны!");
        }

        // разбор строки название сумма на две части
        static bool TryParseExpense(string line, out string name, out double price)
        {
            name = "";
            price = 0;

            if (string.IsNullOrWhiteSpace(line))
                return false;

            // ищем последнюю точку с запятой
            int separator = line.LastIndexOf(';');
            if (separator < 0)
                return false; // разделителя нет вообще

            // substring вырезает кусок строки, trim убирает лишние пробелы по краям.
            name = line.Substring(0, separator).Trim();
            string priceText = line.Substring(separator + 1).Trim();

            if (name.Length == 0)
                return false;

            priceText = priceText.Replace(" ", "").Replace(',', '.');

            // InvariantCulture = считать точку десятичным разделителем
            if (!double.TryParse(priceText, NumberStyles.Float, CultureInfo.InvariantCulture, out price))
                return false;

            if (price < 0)
                return false; // отрицательных трат не бывает

            return true;
        }

        // меню
        static void RunMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("меню");
                Console.WriteLine("1. вывод данных");
                Console.WriteLine("2. статистика");
                Console.WriteLine("3. сортировка по цене");
                Console.WriteLine("4. конвертация валюты");
                Console.WriteLine("5. поиск по названию");
                Console.WriteLine("0. выход");
                Console.Write("ваш выбор: ");

                // защита от null, если ввод внезапно закончился
                string choice = (Console.ReadLine() ?? "").Trim();

                switch (choice)
                {
                    case "1":
                        PrintAll();
                        break;
                    case "2":
                        PrintStatistics();
                        break;
                    case "3":
                        SortByPrice();
                        break;
                    case "4":
                        ConvertCurrency();
                        break;
                    case "5":
                        SearchByName();
                        break;
                    case "0":
                        return; 
                    default:
                        Console.WriteLine("нет такого пункта,введите цифру от 0 до 5.");
                        break;
                }
            }
        }

        // вывод данных
        static void PrintAll()
        {
            Console.WriteLine();
            // {0,-4}    = значение 0, выровнять по левому краю в поле шириной 4
            // {2,14:F2} = значение 2, по правому краю в поле 14, с 2 знаками после запятой
            Console.WriteLine("{0,-4} {1,-40} {2,14}", "№", "Название", "Сумма, руб.");
            Console.WriteLine(new string('-', 60)); // строка из 60 дефисов

            double total = 0;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("{0,-4} {1,-40} {2,14:F2}", i + 1, names[i], prices[i]);
                total += prices[i];
            }

            Console.WriteLine(new string('-', 60));
            Console.WriteLine("{0,-4} {1,-40} {2,14:F2}", "", "ИТОГО:", total);
        }

        // статистика
        static void PrintStatistics()
        {
            double sum = 0;
            // за самую большую и самую маленькую сначала принимаем первый элемент,
            // а потом сравниваем с ним все остальные.
            int maxIndex = 0;
            int minIndex = 0;

            for (int i = 0; i < count; i++)
            {
                sum += prices[i];

                if (prices[i] > prices[maxIndex])
                    maxIndex = i;

                if (prices[i] < prices[minIndex])
                    minIndex = i;
            }

            double average = sum / count;

            Console.WriteLine();
            Console.WriteLine("статистика");
            Console.WriteLine("Количество операций: " + count);
            Console.WriteLine("Сумма всех трат:     {0:F2} руб.", sum);
            Console.WriteLine("Средняя трата:       {0:F2} руб.", average);
            Console.WriteLine("Максимальная трата:  {0:F2} руб.  ({1})", prices[maxIndex], names[maxIndex]);
            Console.WriteLine("Минимальная трата:   {0:F2} руб.  ({1})", prices[minIndex], names[minIndex]);
        }

        // сортировка пузырьком
        static void SortByPrice()
        {
            Console.WriteLine();
            Console.WriteLine("1 - по возрастанию цены");
            Console.WriteLine("2 - по убыванию цены");
            Console.Write("Ваш выбор: ");
            string mode = (Console.ReadLine() ?? "").Trim();

            bool ascending;
            if (mode == "1") ascending = true;
            else if (mode == "2") ascending = false;
            else
            {
                Console.WriteLine("Нужно ввести 1 или 2. Сортировка отменена.");
                return;
            }

            // пузырьковая сортировка
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - 1 - i; j++)
                {
                    bool needSwap;
                    if (ascending)
                        needSwap = prices[j] > prices[j + 1];
                    else
                        needSwap = prices[j] < prices[j + 1];

                    if (needSwap)
                    {
                        // меняем местами суммы через временную переменную
                        double tempPrice = prices[j];
                        prices[j] = prices[j + 1];
                        prices[j + 1] = tempPrice;

                        // и обязательно названия, иначе цены уедут от своих товаров
                        string tempName = names[j];
                        names[j] = names[j + 1];
                        names[j + 1] = tempName;
                    }
                }
            }

            Console.WriteLine("Сортировка выполнена");
            PrintAll();
        }

        // конвертация валют
        static void ConvertCurrency()
        {

        }

        // поиск по названию
        static void SearchByName()
        {
        }
    }
}