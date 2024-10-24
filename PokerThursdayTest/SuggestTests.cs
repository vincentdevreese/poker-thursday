using FluentAssertions;

using PokerThursday;

namespace PokerThursdayTest;

public class SuggestTests
{
    private readonly Suggest sut = new();

    private static Debt ADebtFrom(int amount) =>
        Randomizer.Any<Debt>() with { Amount = amount };

    [Fact]
    public void Should_test_name()
    {
        Debt[] a = [];
        this.Verify(a, a);
    }

    [Fact]
    public void Should_test_name_2()
    {
        Debt[] a = [ADebtFrom(1)];
        this.Verify(a, a);
    }

    [Fact]
    public void Should_test_name_3()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("b", "c", 20);

        this.Verify([debt1, debt2], [new("a", "c", 20)]);
    }


    [Fact]
    public void Should_test_name_4()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("a", "c", 20);

        this.Verify([debt1, debt2], [debt1, debt2]);
    }

    [Fact]
    public void Should_test_name_5()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("c", "d", 20);
        Debt debt3 = new("e", "f", 20);

        this.Verify([debt1, debt2, debt3], [debt1, debt2, debt3]);
    }

    [Fact]
    public void Should_test_name_6()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("b", "c", 10);

        this.Verify(
            [debt1, debt2],
            [
                new("a", "b", 10),
                new("a", "c", 10)
            ]
        );
    }

    [Fact]
    public void Should_test_name_7()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("b", "c", 30);

        this.Verify(
            [debt1, debt2],
            [
                new("a", "c", 20),
                new("b", "c", 10)
            ]
        );
    }

    [Fact]
    public void Should_test_name_8()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("b", "c", 30);
        Debt debt3 = new("d", "e", 30);

        this.Verify(
            [debt1, debt2, debt3],
            [
                new("a", "c", 20),
                new("b", "c", 10),
                new("d", "e", 30),
            ]
        );
    }

    private void Verify(Debt[] register, Debt[] expected)
    {
        Debt[] actual = this.sut.Do(register);
        actual.Should().Equal(expected);
    }
}