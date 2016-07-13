using System;
using System.Collections.Generic;

namespace TapUp
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			NumberGen numGen = new NumberGen();
			numGen.print ();
		}
	}

	class NumberGen
	{
		private static int seed = Guid.NewGuid ().GetHashCode ();
		private Random rand = new Random(seed);
		private int[] nNums = new int[4];
		private int[] cNums = {-1, -1, -1, -1};
		private int pNum;
		private int top = 9;

		public int lives = 3;

		public NumberGen ()
		{
			// Set random numbers for the starting current numbers
			for (int i = 0; i < cNums.Length; i++) 
			{
				while (cNums[i] == -1) {
					int num = rand.Next (top + 1);
					if (Array.IndexOf (cNums, num) == -1) {
						cNums [i] = num;
					}
				}
			}
			// Set the previous number
			int setPnum = cNums[rand.Next (cNums.Length)];
			if (setPnum < top) {
				pNum = setPnum + 1;
			} else {
				pNum = setPnum - 1;
			}
			ProcessNextNums ();
		}

		public void print()
		{
			string hand = "";
			string nextHand = "";
			for (int i = 0; i < cNums.Length; i++) 
			{
				hand += cNums [i].ToString ();
				nextHand += nNums [i].ToString ();
			}
			Console.WriteLine ("\n");
			Console.WriteLine ("Next: {0}", nextHand);
			Console.WriteLine ("This: {0}", hand);
			Console.WriteLine ("Prev: {0}", pNum);
			WaitForInput ();
		}

		private void WaitForInput ()
		{
			int num;
			int input = Console.Read ();
			if (input != -1) {
				char chrInput = (char)input;
				if (int.TryParse (chrInput.ToString (), out num)){
					ChoseNumber (num);
				}else{
					Console.WriteLine("Please enter an integer.");
				} 

			}
		}

		private void ChoseNumber(int n)
		{
			if (pNum + 1 == n || pNum - 1 == n) {
				// Timer + someTime
				nNums.CopyTo(cNums, 0);
				pNum = n;
				ProcessNextNums ();
			} else {
				// Timer - someTime
				if (lives > 0) {
					lives -= 1;
					Console.WriteLine("\nLives Left: {0}", lives);
				} else {
					EndGame ();
				}
			}
			print ();
		}

		private void ProcessNextNums()
		{	
			List<int> indexes = new List<int>(){0, 1, 2, 3};
			nNums = new int[] {-1, -1, -1, -1};
			for (int i = 0; i < cNums.Length; i++) 
			{
				while (nNums[i] == -1){
					// Pick one of the avilable indexes.
					int index = rand.Next (indexes.Count);
					// Get the value of the index
					int nNumI = indexes [index];

					if ((rand.Next (2) == 0 || cNums[i] == top) && cNums[i] > 0) {
						nNums [nNumI] = cNums [i] - 1;
					} else {
						nNums [nNumI] = cNums [i] + 1;
					}
					
					// Remove the index from the list of indexes.
					if (nNums[nNumI] != -1){
						indexes.RemoveAt (index);
					}
				}
			}
		}

		private void EndGame()
		{
			Console.WriteLine("Game Over");
			System.Environment.Exit(1);
		}

	}
}
