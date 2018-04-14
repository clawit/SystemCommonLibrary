using System;
using System.Security.Cryptography;

namespace SystemCommonLibrary.Math
{
	public class Random
	{
		/// <summary>
		/// get a random value
		/// </summary>
		/// <param name="MinValue"></param>
		/// <param name="MaxValue"></param>
		/// <returns></returns>
		public static int Rnd (int MinValue, int MaxValue)
		{
			// Create a byte array to hold the random value.
			byte [] randomNumber = new byte [1];

			// Create a new instance of the RNGCryptoServiceProvider. 
			RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider ();

			// Fill the array with a random value.
			Gen.GetNonZeroBytes (randomNumber);

			// Convert the byte to an integer value to make the modulus operation easier.
			int rand = Convert.ToInt32 (randomNumber [0]);

			System.Random rnd = new System.Random (DateTime.Now.Millisecond * rand);
			return rnd.Next (MinValue, MaxValue);
		}

	}
}
