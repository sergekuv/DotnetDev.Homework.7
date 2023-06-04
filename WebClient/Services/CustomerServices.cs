using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebClient.Services
{
    internal static class CustomerServices
    {
        private static Random random = new ();
        private static string serverUri = "https://localhost:5001/customers/";
        internal static string CreateManual()
        {
            Customer customer = GetCustomerFromConsole();
            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                Id = customer.Id,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname
            }),
            Encoding.UTF8, "application/json");

            return "Id=" + customer.Id + ": " + PostCustomer(jsonContent);
        }

        internal static string PostCustomer(StringContent jsonContent)
        {
            using HttpClient client = new();
            try
            {
                using HttpResponseMessage response = client.PostAsync(serverUri, jsonContent).Result;
                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static Customer GetCustomerFromConsole()
        {
            Console.Write("Введите Id (натуральное число): ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("Id будет присвоен автоматически");
                id = random.Next(0, 100);
            }
            Console.Write("Введите имя: ");
            string fName = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            string lName = Console.ReadLine();

            if(String.IsNullOrWhiteSpace(fName) || String.IsNullOrWhiteSpace(lName))
            {
                Console.WriteLine("имя и фамилия не должны быть пустыми - попробуйте еще раз");
                return GetCustomerFromConsole();
            }
            return new Customer() { Id = id, Firstname = fName, Lastname = lName};

        }

        internal static string CreateRandom()
        {
            int id = random.Next(1, 100);
            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                Id = id,
                Firstname = RandomString(random.Next(5,10)),
                Lastname = RandomString(random.Next(5, 10))
            }),
            Encoding.UTF8, "application/json");

            return "Id=" + id + ": " + PostCustomer(jsonContent);
        }

        internal static string GetById(int id)
        {
            var options = new JsonSerializerOptions 
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                using HttpClient client = new();
                using HttpResponseMessage response = client.GetAsync(serverUri + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    Customer? customer = JsonSerializer.Deserialize<Customer>(response.Content.ReadAsStringAsync().Result, options);
                    return "Found: " + customer?.Id + ": " + customer?.Firstname + " " + customer?.Lastname;
                }
                return "Failed to get customer by Id: " + response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal static string GetAll()
        {
            using HttpClient client = new();
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            try
            {
                using HttpResponseMessage response = client.GetAsync(serverUri).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
