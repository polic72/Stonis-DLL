using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Stonis
{
    /// <summary>
    /// A randomly-generated name from the initialized syllables.
    /// </summary>
    public class Name
    {
        private static readonly int[] DEFAULT_SYLLABLE_COUNTS = { 2, 3, 4, 5, 6 };
        private static readonly double[] DEFAULT_SYLLABLE_PROBABILITIES = { .15, .33, .29, .15, .08 };

        private static string[] known_syllables;
        private static Distribution<int> distribution;
        private static Random syllable_chooser;

        private static bool initialized = false;


        private string[] syllables;


        #region Init

        /// <summary>
        /// Determines whether the backend of the Name class is initialized.
        /// <para/>
        /// If false, use any Init method.
        /// </summary>
        /// <returns>Whether the backend of the Name class is initialized.</returns>
        public static bool IsInitialized()
        {
            return initialized;
        }


        /// <summary>
        /// Initializes the backend of the Name class.
        /// </summary>
        /// <param name="path">Path to a plain text file that has a syllable on every line.</param>
        /// <param name="syllable_counts">Array representing every possible number of syllables a name can have.
        /// </param>
        /// <param name="syllable_probabilities">Array representing the probability that every syllable count 
        /// occurs.</param>
        public static void Init(string path, int[] syllable_counts, double[] syllable_probabilities)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                LinkedList<string> syl = new LinkedList<string>();

                while (!reader.EndOfStream)
                {
                    syl.AddLast(reader.ReadLine());
                }

                known_syllables = syl.ToArray();
            }


            distribution = new Distribution<int>(syllable_counts, syllable_probabilities);
            syllable_chooser = new Random();

            initialized = true;
        }


        /// <summary>
        /// Initializes the backend of the Name class.
        /// <para/>
        /// Uses default the syllable counts (2, 3, 4, 5, 6) and syllable probabilities (.15, .33, .29, .15, .08).
        /// </summary>
        /// <param name="path">Path to a plain text file that has a syllable on every line.</param>
        public static void Init(string path)
        {
            Init(path, DEFAULT_SYLLABLE_COUNTS, DEFAULT_SYLLABLE_PROBABILITIES);
        }


        /// <summary>
        /// Initializes the backend of the Name class.
        /// </summary>
        /// <param name="syllables">Every possible syllable.</param>
        /// <param name="syllable_counts">Array representing every possible number of syllables a name can have.
        /// </param>
        /// <param name="syllable_probabilities">Array representing the probability that every syllable count 
        /// occurs.</param>
        public static void Init(string[] syllables, int[] syllable_counts, double[] syllable_probabilities)
        {
            known_syllables = (string[])syllables.Clone();


            distribution = new Distribution<int>(syllable_counts, syllable_probabilities);
            syllable_chooser = new Random();

            initialized = true;
        }


        /// <summary>
        /// Initializes the backend of the Name class.
        /// <para/>
        /// Uses default the syllable counts (2, 3, 4, 5, 6) and syllable probabilities (.15, .33, .29, .15, .08).
        /// </summary>
        /// <param name="syllables">Every possible syllable.</param>
        public static void Init(string[] syllables)
        {
            Init(syllables, DEFAULT_SYLLABLE_COUNTS, DEFAULT_SYLLABLE_PROBABILITIES);
        }

        #endregion Init


        /// <summary>
        /// Constructs a Name object with a random number of syllables (using the distribution given in the Init()) 
        /// and random syllables (uniformly distributed from the known syllables).
        /// </summary>
        public Name()
        {
            if (!initialized)
            {
                throw new InvalidOperationException("The Name class was not initialized.");
            }

            int num_syllables = distribution.ChooseValue();
            syllables = new string[num_syllables];

            int counter = 0;
            for (int i = 0; i < num_syllables; ++i)
            {
                syllables[counter++] = known_syllables[syllable_chooser.Next(known_syllables.Length)];
            }
        }


        /// <summary>
        /// Gets the syllables used in this Name object.
        /// </summary>
        /// <returns>The syllables used in this Name object.</returns>
        public string[] GetSyllables()
        {
            return syllables;
        }


        public static explicit operator string(Name name) => name.ToString();

        public override string ToString()
        {
            return string.Join("", syllables);
        }
    }
}
