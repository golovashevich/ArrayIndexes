using System;
using System.Collections.Generic;

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

			//The worst case for brute
			index1 = intArray.Length - 2;
			index2 = intArray.Length - 1;

			int targetSum = intArray[index1] + intArray[index2];

			Console.WriteLine("Lookup algoritms comparison (array with {0} elements)", intArray.Length);

			EstimateAlgorithm(intArray, index1, index2, targetSum, CalculateIndexesForSum_Brute, "Brute lookup");
			EstimateAlgorithm(intArray, index1, index2, targetSum, CalculateIndexesForSum_Hashes, "Hashes lookup");
			EstimateAlgorithm(intArray, index1, index2, targetSum, CalculateIndexesForSum_Sorted, "Sorted lookup");

			Console.WriteLine("\nPress Enter to continue...");
			Console.ReadLine();
		}

		private static void EstimateAlgorithm(int[] intArray, int index1, int index2, int targetSum,
				Func<int[], int, Tuple<int, int>> calculator, string name) {
			Console.WriteLine("\n{0}...", name);
			var start = DateTime.Now;
			try {
				Tuple<int, int> indexes = calculator(intArray, targetSum);
				var duration = DateTime.Now - start;
				Console.WriteLine("Time to calculate: {0} msec(s)", duration.TotalMilliseconds);
				Console.WriteLine("Expected\tindex1 - {0}\tindex2 - {1}\ttargetSum: {2}", index1, index2, targetSum);
				Console.WriteLine("Calculated\tindex1 - {0}\tindex2 - {1}\tconrolSum: {2}", indexes.Item1, indexes.Item2,
						intArray[indexes.Item1] + intArray[indexes.Item2]);
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Does not use additional memory, the slowest; complexity is n * n / 2
		/// </summary>
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

		/// <summary>
		/// Uses additional memory (about size of the intArray - minus duplicates), but the fastest
		/// </summary>
		private static Tuple<int, int> CalculateIndexesForSum_Hashes(int[] intArray, int targetSum) {
			var hashes = new HashSet<int>(intArray);
			for (int i = 0; i < intArray.Length; i++) {
				var remainder = targetSum - intArray[i];
				if (hashes.Contains(remainder)) {
					//Lookup for an specific element only one time
					for (int j = 0; j < intArray.Length; j++) {
						if (intArray[j] == remainder) {
							return Tuple.Create(i, j);
						}
					}
				}
			}
			throw new Exception("No indexes have been found");
		}

		/// <summary>
		/// Does not use additional memory, slightly slower than with hashes
		/// Warning: modifies (sorts) the original array, so it's better to run this algorithm in the last turn
		/// </summary>
		private static Tuple<int, int> CalculateIndexesForSum_Sorted(int[] intArray, int targetSum) {
			Array.Sort(intArray);
			for (int i = 0; i < intArray.Length; i++) {
				var remainder = targetSum - intArray[i];
				var index = Array.BinarySearch(intArray, remainder);
				if (index >= 0) {
					return Tuple.Create(i, index);
				}
			}
			throw new Exception("No indexes have been found");
		}
	}
}
