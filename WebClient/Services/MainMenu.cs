using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WebClient.Services
{
    internal static class MainMenu
    {
        internal static void Show()
        {
            Console.WriteLine("Введите:\n" +
                "R - добавить пользователя со случайными данными\n" +
                "N - добавить пользователя вручную\n" +
                "A - получить список всех пользователей\n" +
                "любое число - поиск пользователя по id\n" +
                "другой ввод - завершение работы");
            string option;
            while (true)
            {
                Console.Write("Enter your choice: ");
                option = Console.ReadLine();
                if (int.TryParse(option, out int custId))
                {
                    var result = CustomerServices.GetById(custId);
                    Console.WriteLine(result);
                }
                else
                {
                    switch (option.ToUpper())
                    {
                        case "R":
                            Console.WriteLine(CustomerServices.CreateRandom());
                            break;
                        case "N":
                            Console.WriteLine(CustomerServices.CreateManual());
                            break;
                        case "A":
                            Console.WriteLine(CustomerServices.GetAll());
                            break;
                        default:
                            Console.WriteLine("--завершение работы--");
                            return;
                    }
                }
            }
        }
    }
}
