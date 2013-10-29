using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apriori.DAL.Gateway;
using Apriori.DAL.DAO;
using Apriori.DAL.Gateway.Interface;

namespace Apriori.Algorithm
{
    public class AprioriAlgorithm
    {
        private IInputDatabaseHelper inputDatabaseHelper;
        private IOutputDatabaseHelper outputDatabaseHelper;

        private float minimumSupport;
        public float MinimumSupport
        {
            get { return minimumSupport; }
            set 
            { 
                minimumSupport = value;
                minimumSupportCount = (int)(minimumSupport * (float)inputDatabaseHelper.TotalTransactionNumber);
            }
        }
        private int minimumSupportCount;

        public int MinimumSupportCount
        {
            get { return minimumSupportCount; }
            set 
            { 
                minimumSupportCount = value;
                minimumSupport = (float)minimumSupportCount / (float)inputDatabaseHelper.TotalTransactionNumber;
            }
        }

        //constructor
        public AprioriAlgorithm(IInputDatabaseHelper _inDatabaseHelper,IOutputDatabaseHelper _outDatabaseHelper)
        {
            inputDatabaseHelper = _inDatabaseHelper;
            outputDatabaseHelper = _outDatabaseHelper;
            MinimumSupport = 0f;
        }
        public AprioriAlgorithm(IInputDatabaseHelper _inDatabaseHelper, IOutputDatabaseHelper _outDatabaseHelper, float _minimumSupport)
            : this(_inDatabaseHelper,_outDatabaseHelper)
        {
            MinimumSupport = _minimumSupport;

        }

        public AprioriAlgorithm(IInputDatabaseHelper _inDatabaseHelper, IOutputDatabaseHelper _outDatabaseHelper, int _minimumSupportCount)
            : this(_inDatabaseHelper,_outDatabaseHelper)
        {
            MinimumSupportCount = _minimumSupportCount;
        }



        //returns n length candidates from n-1 frequent itemsets by cross product
        //let a1a2...a(n-1) and b1b2...b(n-1) are two n-1 length frequent itemsets
        //candidate c1c2...cn is a1a2...a(n-2)a(n-1)b(n-1) if a1a2...a(n-2) = b1b2...b(n-2)
        List<ItemSet> GetNextCandidates(List<ItemSet> itemSets)
        {
            List<ItemSet> candidates = new List<ItemSet>();

            for (int i = 0; i < itemSets.Count; ++i )
            {
                for (int j = i + 1; j < itemSets.Count; ++j )
                {
                    ItemSet aCandidate = CrossTwoItemSet(itemSets[i],itemSets[j]);
                    if (!aCandidate.IsEmpty())
                    {
                        candidates.Add(aCandidate);
                    }
                }
            }
            return candidates;
        }

        private ItemSet CrossTwoItemSet(ItemSet itemSet1, ItemSet itemSet2)
        {
            ItemSet candidate = new ItemSet();
            
            if(itemSet1.GetLength() != itemSet2.GetLength())
                return candidate;

            int itemSetLength = itemSet1.GetLength();

            for (int i = 0; i < itemSetLength - 1; ++i )
            {
                if (itemSet1.GetItem(i).Symbol == itemSet2.GetItem(i).Symbol) // 
                {
                    Item anItem = itemSet1.GetItem(i).Clone();
                    candidate.AddItem(anItem);
                }
                else 
                {
                    return new ItemSet(); //return empty itemset
                }
            }

            candidate.AddItem(itemSet1.GetItem(itemSetLength - 1).Clone());
            candidate.AddItem(itemSet2.GetItem(itemSetLength - 1).Clone());

            return candidate;
        }
        private List<ItemSet> GetFrequentItemSetsFromCandidates(List<ItemSet> candidates)
        { 
            List<ItemSet> frequentItemSets = new List<ItemSet>();
            
            foreach(ItemSet anItemSet in candidates)
            {
                if(inputDatabaseHelper.GetFrequency(anItemSet) >= MinimumSupportCount) // frequent itemset
                {
                    frequentItemSets.Add(anItemSet.Clone());
                }
            }

            return frequentItemSets;
        }

        //generate frequent itemsets
        public int GenerateFrequentItemSets()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<ItemSet> previousItemSets = new List<ItemSet>(); //holds itemsets found in previous calculation

            //insert the frequent 1-itemsets
            List<Item> items = inputDatabaseHelper.CalculateFrequencyAllItems();

            foreach(Item anItem in items)
            {
                if(anItem.SupportCount >= MinimumSupportCount) // if frequent
                {
                    ItemSet anItemSet = new ItemSet();
                    anItemSet.AddItem(anItem);
                    previousItemSets.Add(anItemSet);
                }
            }
            int itemSetLength = 1;
            int totalFrequentItemSets = 0;
            
            Console.WriteLine("generated "+itemSetLength.ToString()+"-itemset total "+previousItemSets.Count + " itemsets");
            totalFrequentItemSets += previousItemSets.Count;
            while(previousItemSets.Count != 0)
            {
                itemSetLength++;
                List<ItemSet> newCandidates = GetNextCandidates(previousItemSets);
                previousItemSets.Clear();
                previousItemSets = GetFrequentItemSetsFromCandidates(newCandidates);
                totalFrequentItemSets += previousItemSets.Count;
                Console.WriteLine("generated " + itemSetLength.ToString() + "-itemset total " + previousItemSets.Count + " itemsets");
                
            }
            watch.Stop();
            outputDatabaseHelper.WriteAggregatedResult(inputDatabaseHelper.DatabaseName, MinimumSupport, totalFrequentItemSets, watch.ElapsedMilliseconds);
            Console.WriteLine("Aggregated Result Written to " + outputDatabaseHelper.DatabasePath);
            return totalFrequentItemSets;
        }
       
        
    }
}
