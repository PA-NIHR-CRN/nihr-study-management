
using NIHR.StudyManagement.Domain.Abstractions;

namespace NIHR.StudyManagement.Domain.Models
{
    public class GrisId
    {
        private string displayValue = "";
        private int sequence;
        private int year2Digit;
        private int checkDigit;

        /// <summary>
        /// Default constructor taking in fullSequence parameter which should be a numeric
        /// representing the Gris ID along with both the check digit and year elements.
        /// </summary>
        /// <param name="fullSequence">999999YYX where 9999999 is the random sequence, YY is a 2 digit year and X is the check digit.</param>
        /// <exception cref="ArgumentException"></exception>
        public GrisId(long fullSequence)
        {
            // Validate the check digit
            if (fullSequence == 0
                 || !IsValidGrisId(fullSequence))
            {
                throw new ArgumentException(nameof(fullSequence));
            }

            // Extract check digit
            this.checkDigit = (int)(fullSequence % 10);

            // Remove check digit
            var partialSequence = fullSequence / 10;

            // Get 2 digit year
            this.year2Digit = (int)(partialSequence % 100);           

            // Get sequence without year or check digit
            this.sequence = (int)(fullSequence / 1000);

            InitialiseDisplayValue();
        }

        private GrisId(int sequence, int year2Digit, int checkDigit)
        {
            this.year2Digit = year2Digit;
            this.sequence = sequence;
            this.checkDigit = checkDigit;

            InitialiseDisplayValue();
        }

        public string DisplayValue
        {
            get
            {
                return this.displayValue;
            }
        }

        private void InitialiseDisplayValue()
        {
            this.displayValue = $"GRIS-{sequence.ToString("000 000")} {year2Digit.ToString("00")}{checkDigit}";
        }

        public static GrisId GenerateNewGrisId(IRandomNumberGenerator randomNumberGenerator)
        {
            // Get a new random number
            var randomSequence = randomNumberGenerator.GetRandomNumber();

            // Append a 2 digit year value to the number
            var year2Digit = randomNumberGenerator.CurrentYear % 100;
            var randomSequenceWith2DigitYear = (randomSequence * 100) + year2Digit;

            // Calculate a check digit
            var checkDigit = CalculateCheckDigit(randomSequenceWith2DigitYear);

            // Verify the check digit (todo?)

            return new GrisId(randomSequence, year2Digit, checkDigit);
        }

        /// <summary>
        /// Method to validate whether the specified sequenceWithCheckDigit value is a valid
        /// GRIS identifier.
        /// </summary>
        /// <param name="sequenceWithCheckDigit">999999YYX where 9999999 is the random sequence, YY is a 2 digit year and X is the check digit.</param>
        /// <returns>True if valid, false if not valid.</returns>
        public static bool IsValidGrisId(long sequenceWithCheckDigit)
        {
            // Remove check digit from sequence
            var sequenceWithoutCheckDigit = sequenceWithCheckDigit / 10;

            // Remove year from sequence
            var sequence = sequenceWithoutCheckDigit / 100;

            // Validate sequence
            if(!IsValidSequence(sequence))
            {
                return false;
            }

            var checkDigit = sequenceWithCheckDigit % 10;

            var calculatedCheckDigit = CalculateCheckDigit(sequenceWithoutCheckDigit);

            return checkDigit == calculatedCheckDigit;
        }

        /// <summary>
        /// Method to validate whether or not the specified sequence is valid as per
        /// Gris Id rules.
        /// 1: No number should be repeated more than 5 times.
        /// 2: Number must be greater than 0 and less than 999999
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        private static bool IsValidSequence(long sequence)
        {
            if(sequence == 0
                || sequence >= 999999)
            {
                return false;
            }

            var sequenceAsString = sequence.ToString();

            var sequenceAsArray = new int[sequenceAsString.Length];

            // Ensure no digit repeated more than 5 times
            Dictionary<int, int> countOfNumbersByNumber = new Dictionary<int, int>();

            for (int numberIndex = 0; numberIndex < sequenceAsString.Length; numberIndex++)
            {
                var currentDigitValue = int.Parse(sequenceAsString.Substring(numberIndex, 1));

                if (!countOfNumbersByNumber.ContainsKey(currentDigitValue))
                {
                    countOfNumbersByNumber[currentDigitValue] = 0;
                }

                countOfNumbersByNumber[currentDigitValue]++;
            }

            foreach (var countByNumber in countOfNumbersByNumber)
            {
                // If any number is more than 5 times, the sequence is invalid
                if(countByNumber.Value > 5)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method will calculate a check digit for the specified randomSequenceWith2DigitYear value.
        /// Resource reference: https://en.wikipedia.org/wiki/Luhn_algorithm
        /// </summary>
        /// <param name="randomSequenceWith2DigitYear"></param>
        /// <returns></returns>
        private static int CalculateCheckDigit(long randomSequenceWith2DigitYear)
        {
            var sequenceAsString = randomSequenceWith2DigitYear.ToString();

            var sequenceAsArray = new int[sequenceAsString.Length];

            for (int numberIndex = 0; numberIndex < sequenceAsString.Length; numberIndex++)
            {
                sequenceAsArray[numberIndex] = int.Parse(sequenceAsString.Substring(numberIndex, 1));
            }

            /*
             * Below quote from above online reference.
             *  - With the payload, start from the rightmost digit. Moving left, double the value of every second digit (including the rightmost digit).
             *  - Sum the values of the resulting digits.
             *  - The check digit is calculated by (10−(𝑠 mod10)) mod10
             * */

            bool doubleValue = true;
            var totalSum = 0;

            for (int index = sequenceAsArray.Length - 1; index >= 0; index--)
            {
                var numberToOperate = sequenceAsArray[index];

                if (doubleValue)
                {
                    totalSum = totalSum + DoubleAndSum(numberToOperate);
                    doubleValue = false;
                }
                else
                {
                    totalSum = totalSum + numberToOperate;
                    doubleValue = true;
                }

            }

            return (10 - (totalSum % 10)) % 10;
        }

        /// <summary>
        /// Method to double the specified sourceNumber and then add the two individual digits.
        /// </summary>
        /// <param name="sourceNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static int DoubleAndSum(int sourceNumber)
        {
            if (sourceNumber >= 10)
            {
                throw new ArgumentException(nameof(sourceNumber));
            }

            if (sourceNumber <= 4)
            {
                return sourceNumber * 2;
            }

            var doubleSource = sourceNumber * 2;

            var result = 1 + (doubleSource % 10);

            return result;
        }
    }
}
