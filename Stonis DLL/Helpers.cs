using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NameGen
{
    public static class Helpers
    {
        #region Random Extension Methods

        #region NextLong

        /// <summary>
        /// Gets a random long number between the given bounds.
        /// </summary>
        /// <param name="random">The random object to use.</param>
        /// <param name="min">The minumum bound, inclusive.</param>
        /// <param name="max">The maximum bound, exclusive.</param>
        /// <returns>The generated long.</returns>
        public static long NextLong(this Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException("max", "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            ulong uRange = (ulong)(max - min);

            //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
            //for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            }
            while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }


        /// <summary>
        /// Gets a random long number between 0 (inclusive) and the given max bound.
        /// </summary>
        /// <param name="random">The random object to use.</param>
        /// <param name="max">The maximum bound, exclusive.</param>
        /// <returns>The generated long.</returns>
        public static long NextLong(this Random random, long max)
        {
            return random.NextLong(0, max);
        }


        /// <summary>
        /// Gets a random long number between long.MinValue (inclusive) and long.MaxValue (exclusive).
        /// </summary>
        /// <param name="random">The random object to use.</param>
        /// <param name="max">The maximum bound, exclusive.</param>
        /// <returns>The generated long.</returns>
        public static long NextLong(this Random random)
        {
            return random.NextLong(long.MinValue, long.MaxValue);
        }

        #endregion NextLong


        /// <summary>
        /// Gets a random distributed value based on this random object and the given parameters.
        /// <para/>
        /// Use a <see cref="Distribution{T}"/> object for better performance.
        /// </summary>
        /// <typeparam name="T">The type being chosen.</typeparam>
        /// <param name="random">The Random object to use.</param>
        /// <param name="possible_values">The values that can be chosen from.</param>
        /// <param name="probabilities">The probabilites that each value could be chosen from.</param>
        /// <returns>The value that was chosen.</returns>
        public static T NextDistributed<T>(this Random random, T[] possible_values, double[] probabilities)
        {
            return new Distribution<T>(possible_values, probabilities, random).ChooseValue();
        }

        #endregion Random Extension Methods


        #region IsAlmostEqualTo

        /// <summary>
        /// Tells whether or not the given values are equal to each other up to the given digit.
        /// </summary>
        /// <param name="val_1">The first value.</param>
        /// <param name="val_2">The second value.</param>
        /// <param name="digit">The digit to round the numbers to before comparing.</param>
        /// <returns>Whether or not the given values are equal to each other up to the given digit.</returns>
        public static bool IsAlmostEqualTo(this double val_1, double val_2, int digit)
        {
            if (Math.Round(val_1, digit) == Math.Round(val_2, digit))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Tells whether or not the given values are equal to each other up to the 12th digit.
        /// </summary>
        /// <param name="val_1">The first value.</param>
        /// <param name="val_2">The second value.</param>
        /// <returns>Whether or not the given values are equal to each other up to the 12th digit.</returns>
        public static bool IsAlmostEqualTo(this double val_1, double val_2)
        {
            return IsAlmostEqualTo(val_1, val_2, 12);
        }

        #endregion IsAlmostEqualTo
    }
}
