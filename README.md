Apriori
=======

Apriori(frequent pattern mining from databases) algorithm implementation in C# .Net

For inputs and outputs File System is used here. Other kind of databases can be used by implementing IInputDatabaseHelper.cs and IOutputDatabaseHelper.cs interfaces.


Path for input file is given in App.config file (change this value for other input files)

Path for output file is given in FileOutputDatabaseHelper constructor (change this value for other output paths)

Program.cs file shows a sample usage

Sample Usage

```
static void Main(string[] args)
        {
            FileInputDatabaseHelper inputHelper = new FileInputDatabaseHelper("mushroom");
            FileOutputDatabaseHelper outputHelper = new FileOutputDatabaseHelper(@"D:\Data_Mining_Assignment\Apriori\Result\");
            AprioriAlgorithm apriori = new AprioriAlgorithm(inputHelper,outputHelper,0.5f);
            apriori.GenerateFrequentItemSets();
        }
``` 

Unit Test Project

Unit Test Project Apriori Tests is also included with a few unit tests implemented.




