using System;
using System.Threading.Tasks;
using WebClient.Services;

namespace WebClient
{
    static class Program
    {
        static void Main(string[] args)
        {
            Services.MainMenu.Show();
            Console.ReadLine();
        }
    }
}