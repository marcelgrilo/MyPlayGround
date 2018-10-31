
using System;
using System.Threading.Tasks;

using TableStorageTests.Models;

namespace TableStorageTests
{

    public class Program
    {

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();

            Console.ReadKey();
        }

        public static async Task MainAsync(string[] args)
        {
            var settings =
                new AzureTableSettings(
                    storageAccount: "devstoreaccount1",
                    storageKey: "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==",
                    tableName: "scores",
                    storageConnectionString: "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/evstoreaccount1;"
                    );

            IAzureTableStorage<Score> scoreTableStorage = new AzureTableStorage<Score>(settings);



            Random rand = new Random();

            //inserting
            for (int i = 0; i < 2000; i++)
            {
                await scoreTableStorage.Insert(
                new Score()
                {
                    PartitionKey = rand.Next(1, 3).ToString(),
                    RowKey = new Guid().ToString(),
                    Name = GenerateName(rand.Next(3, 8)),
                    Date = DateTime.Now.AddMinutes(rand.Next(-200, 200)),
                    Timestamp = DateTime.Now,
                    TotalScore = rand.Next(0, 1000)
                }).ConfigureAwait(false);
            }
        }

        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }
            return Name;
        }
    }
}
