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
            Console.WriteLine("uset pashodov za den");

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

        }

        // разбор строки название сумма на две части
        static bool TryParseExpense(string line, out string name, out double price)
        {
        }

        // меню
        static void RunMenu()
        {
        }

        // вывод данных
        static void PrintAll()
        {
        }

        // статистика
        static void PrintStatistics()
        {
        }

        // сортировка пузырьком
        static void SortByPrice()
        {
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