using System;

namespace TableStorageTests.Models
{
    public class Score : AzureTableEntity
    {
        public string Name { get; set; }
        public int TotalScore { get; set; }
        public DateTime Date { get; set; }
    }
}
