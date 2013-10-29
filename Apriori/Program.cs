using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Apriori.DAL.Gateway;
using Apriori.DAL.DAO;
using Apriori.Algorithm;
namespace Apriori
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInputDatabaseHelper inputHelper = new FileInputDatabaseHelper("mushroom");
            FileOutputDatabaseHelper outputHelper = new FileOutputDatabaseHelper(@"D:\Data_Mining_Assignment\Apriori\Result\");
            AprioriAlgorithm apriori = new AprioriAlgorithm(inputHelper,outputHelper,0.5f);
            apriori.GenerateFrequentItemSets();
        }
    }
}
