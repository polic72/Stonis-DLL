using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Stonis
{
    /// <summary>
    /// A utility class that customizes a discrete distribution.
    /// </summary>
    /// <typeparam name="T">The type stored and returned.</typeparam>
    public class ScoredDistribution<T>
    {
        private List<T> values;
        private List<double> scores;

        private double total_score = 0;

        private Random random;


        #region Constructors

        /// <summary>
        /// Constructs a scored distribution with the given random.
        /// </summary>
        /// <param name="capacity">The initial capacity of the ScoredDistribution.</param>
        /// <param name="random">The random object to use.</param>
        public ScoredDistribution(int capacity, Random random)
        {
            values = new List<T>(capacity);
            scores = new List<double>(capacity);

            this.random = random;
        }


        /// <summary>
        /// Constructs a scored distribution.
        /// </summary>
        public ScoredDistribution()
        {
            values = new List<T>();
            scores = new List<double>();

            random = new Random();
        }

        #endregion Constructors


        /// <summary>
        /// Adds the given T and score to this ScoredDistribution.
        /// </summary>
        /// <param name="obj">The T object to add.</param>
        /// <param name="score">The score of the T object to add.</param>
        public void Add(T obj, double score)
        {
            values.Add(obj);
            scores.Add(score);

            total_score += score;
        }


        /// <summary>
        /// Clears this ScoredDistribution of all T objects and scores.
        /// </summary>
        public void Clear()
        {
            values.Clear();
            scores.Clear();

            total_score = 0;
        }


        /// <summary>
        /// Chooses a T value from the stored values based the given distribution.
        /// </summary>
        /// <returns>T value from the stored values based the given distribution.</returns>
        public T ChooseValue()
        {
            double val = random.NextDouble() * total_score;

            double count = 0;
            for (int i = 0; i < values.Count; ++i)
            {
                count += scores[i];

                if (count > val)
                {
                    return values[i];
                }
            }

            return default; //This should never happen.
        }
    }
}
