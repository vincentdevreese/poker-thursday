using AutoFixture.Xunit2;

namespace PokerThursdayTest.AutoFixture;

public class RandomDataAttribute : AutoDataAttribute
{
    public RandomDataAttribute() : base(RandomData.GetFixture)
    {
    }
}