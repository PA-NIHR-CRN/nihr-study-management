namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IRandomNumberGenerator
    {
        int GetRandomNumber();

        int CurrentYear { get; }
    }
}
