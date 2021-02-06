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
        /// <exception cref="ArgumentException">When the score is not a positive non-zero value.</exception>
        public void Add(T obj, double score)
        {
            if (score <= 0)
            {
                throw new ArgumentException("The score must be a positive non-zero value.", "score");
            }


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


        #region ChoosingValue

        /// <summary>
        /// Chooses a T value from the stored values based the given distribution.
        /// </summary>
        /// <returns>T value from the stored values based the given distribution. Default if empty.</returns>
        public T ChooseValue()
        {
            if (values.Count == 0)
            {
                return default;
            }


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

            return default;
        }


        /// <summary>
        /// Chooses a T value from the stored values (excluding the given object) based the given distribution.
        /// </summary>
        /// <param name="excluding_obj">The object to exclude from the search.</param>
        /// <returns>T value from the stored values (excluding the given object) based the given distribution.</returns>
        /// <remarks>
        /// T must implement <see cref="IEquatable{T}"/>.
        /// </remarks>
        public T ChooseValue(T excluding_obj)
        {
            if (values.Count == 0)
            {
                return default;
            }


            int excluding_obj_index = values.IndexOf(excluding_obj);
            double temp_total_score;

            if (excluding_obj_index == -1)
            {
                temp_total_score = total_score;
            }
            else
            {
                temp_total_score = total_score - scores[excluding_obj_index];
            }


            double val = random.NextDouble() * temp_total_score;

            double count = 0;
            for (int i = 0; i < values.Count; ++i)
            {
                count += scores[i];

                if (count > val)
                {
                    return values[i];
                }
            }

            return default;
        }

        #endregion ChoosingValue
    }
}
