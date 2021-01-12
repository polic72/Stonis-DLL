using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NameGen
{
    /// <summary>
    /// A utility class that customizes a discrete distribution.
    /// </summary>
    /// <typeparam name="T">The type stored and returned.</typeparam>
    public class Distribution<T>
    {
        private T[] possible_values;
        private double[] boundaries;

        private Random random;


        #region Constructors

        /// <summary>
        /// Constructs a distribution with the given possible values and probabilities.
        /// <para/>
        /// Both array parameters need to be the same length, and have at least 1 value. Each possible value will 
        /// line up to its probability by index.
        /// </summary>
        /// <param name="possible_values">The possible values that can be chosen.</param>
        /// <param name="probabilities">The probabilities that each value will be chosen. Must add up to 
        /// 1.0</param>
        /// <param name="random">The random object to use.</param>
        /// <exception cref="ArgumentNullException">Any argument is null.</exception>
        /// <exception cref="ArgumentException">possible_values is empty or array parameter's length do not 
        /// match.</exception>
        /// <exception cref="ArgumentOutOfRangeException">probabilities doesn't add up to 1.0</exception>
        public Distribution(T[] possible_values, double[] probabilities, Random random)
        {
            #region Error Handling

            if (possible_values == null)
            {
                throw new ArgumentNullException("possible_values",
                    "The \"possible_values\" parameter cannot be null.");
            }
            else if (probabilities == null)
            {
                throw new ArgumentNullException("probabilities",
                    "The \"probabilities\" parameter cannot be null.");
            }
            else if (random == null)
            {
                throw new ArgumentNullException("random", "The \"random\" parameter cannot be null.");
            }

            if (possible_values.Length == 0)
            {
                throw new ArgumentException("At least 1 number needs to be able to be chosen.", "possible_values");
            }

            if (possible_values.Length != probabilities.Length)
            {
                throw new ArgumentException("The given arrays are not the same length.", "probabilities");
            }

            if (!probabilities.Sum().IsAlmostEqualTo(1.0))
            {
                throw new ArgumentOutOfRangeException("probabilities",
                    "The given probabilities do not add up to 1.0");
            }

            #endregion Error Handling


            this.possible_values = possible_values;

            boundaries = new double[probabilities.Length + 1];
            for (int i = 0; i < boundaries.Length; ++i)
            {
                boundaries[i] = (i == 0) ? 0 : probabilities[i - 1] + boundaries[i - 1];
            }

            this.random = random;
        }


        /// <summary>
        /// Constructs a distribution with the given possible values and probabilities. Uses a radom seed.
        /// <para/>
        /// Both parameters need to be the same length, and have at least 1 value. Each possible value will line 
        /// up to its probability by index.
        /// </summary>
        /// <param name="possible_values">The possible values that can be chosen.</param>
        /// <param name="probabilities">The probabilities that each value will be chosen. Must add up to 
        /// 1.0</param>
        /// <exception cref="ArgumentNullException">Any argument is null.</exception>
        /// <exception cref="ArgumentException">possible_values is empty or array parameter's length do not 
        /// match.</exception>
        /// <exception cref="ArgumentOutOfRangeException">probabilities doesn't add up to 1.0</exception>
        public Distribution(T[] possible_values, double[] probabilities) :
            this(possible_values, probabilities, new Random())
        {

        }

        #endregion Constructors


        /// <summary>
        /// Chooses a T value from the stored values based the given distribution.
        /// </summary>
        /// <returns>T value from the stored values based the given distribution.</returns>
        public T ChooseValue()
        {
            if (possible_values.Length == 1)    //Edge case.
            {
                return possible_values[0];
            }


            double place = random.NextDouble();

            int left_most = 0, right_most = boundaries.Length - 1;

            while (left_most <= right_most)
            {
                int mid = left_most + ((right_most - 1) / 2);

                if (place < boundaries[mid])
                {
                    if (place >= boundaries[mid - 1])
                    {
                        return possible_values[mid - 1];
                    }
                    else
                    {
                        right_most = mid - 1;
                    }
                }
                else if (place > boundaries[mid])
                {
                    if (place < boundaries[mid + 1])
                    {
                        return possible_values[mid];
                    }
                    else
                    {
                        left_most = mid + 1;
                    }
                }
                else
                {
                    return possible_values[mid];
                }
            }

            return default; //This should never happen.
        }
    }
}
