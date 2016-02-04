using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator {
	class Program {
		public const int MAX_ELEMENTS = 200000;
		static void Main(string[] args) {
			var intArray = new int[MAX_ELEMENTS]; 
			var rand = new Random();
			for (int i = 0; i < intArray.Length; i++) {
				//array of natural numbers
				intArray[i] = rand.Next(Int32.MaxValue / 2); // to avoid an overflow in target sum
			}

			int index1 = rand.Next(intArray.Length);
			int index2 = rand.Next(intArray.Length);

			int targetSum = intArray[index1] + intArray[index2];

			try {
				var start = DateTime.Now;
				Tuple<int, int> indexes = CalculateIndexesForSum_Brute(intArray, targetSum);
				var duration = DateTime.Now - start;
				Console.WriteLine("Time to calculate: {0}", duration.TotalSeconds);
				Console.WriteLine("Expected\tindex1 - {0}\tindex2 - {1}\ttargetSum: {2}", index1, index2, targetSum);
				Console.WriteLine("Brute lookup\tindex1 - {0}\tindex2 - {1}\tconrolSum: {2}",indexes.Item1, indexes.Item2, 
						intArray[indexes.Item1] + intArray[indexes.Item2]);
				Console.ReadLine();
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}

		private static Tuple<int, int> CalculateIndexesForSum_Brute(int[] intArray, int targetSum) {
			for (int i = 0; i < intArray.Length; i++) {
				var remainder = targetSum - intArray[i]; // this replaces two number addition in inner loop with comparison
				for (int j = i; j < intArray.Length; j++) {
					if (intArray[j] == remainder) {
						return Tuple.Create(i, j);
					}
				}
			}
			throw new Exception("No indexes have been found");
		}
	}
}
