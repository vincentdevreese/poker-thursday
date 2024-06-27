using AutoFixture.Xunit2;

namespace PokerThursdayTest.AutoFixture;

public class InlineRandomDataAttribute : InlineAutoDataAttribute
{
    public InlineRandomDataAttribute(params object?[] values)
        : base(new RandomDataAttribute(), values)
    {
    }
}