using NIHR.StudyManagement.Domain.Abstractions;

namespace NIHR.StudyManagement.Domain.Helpers
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        const int lowerLimit = 10;
        const int upperLimit = 999999;

        private Random _random;

        public RandomNumberGenerator()
        {
            _random = new Random();
        }

        public int CurrentYear => DateTime.Now.Year;

        public int GetRandomNumber()
        {
            return _random.Next(lowerLimit, upperLimit);
        }
    }
}
