using FluentAssertions;
using PokerThursday;
using PokerThursdayTest.AutoFixture;

namespace PokerThursdayTest;

public class SuggestTests
{
    private readonly Suggest sut = new();

    private static Debt ADebtFrom(int amount)
    {
        return Randomizer.Any<Debt>() with { Amount = amount };
    }

    private static Debt ADebt(string debtor, string creditor)
    {
        return Randomizer.Any<Debt>() with { Debtor = debtor, Creditor = creditor };
    }

    [Fact]
    public void Should_DoNothing_WhenDebtRegisterIsEmpty()
    {
        Verify([], []);
    }

    [Theory]
    [RandomData]
    public void Should_DoNothing_WhenDebtRegisterHasOnlyOneDebt(Debt theSingleDebt)
    {
        Verify([theSingleDebt], [theSingleDebt]);
    }

    [Fact]
    public void Should_DoNothing_WhenNoDebtorIsAlsoCreditor()
    {
        Debt debt1 = ADebt("a", "b");
        Debt debt2 = ADebt("c", "d");

        Verify([debt1, debt2], [debt1, debt2]);
    }

    [Fact]
    public void Should_test_name_3()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("b", "c", 20);

        Verify([debt1, debt2], [new Debt("a", "c", 20)]);
    }

    [Fact]
    public void Should_test_name_6()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("b", "c", 10);

        Verify(
            [debt1, debt2],
            [
                new Debt("a", "b", 10),
                new Debt("a", "c", 10)
            ]
        );
    }

    [Fact]
    public void Should_test_name_7()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("b", "c", 30);

        Verify(
            [debt1, debt2],
            [
                new Debt("a", "c", 20),
                new Debt("b", "c", 10)
            ]
        );
    }

    [Fact]
    public void Should_test_name_8()
    {
        Debt debt1 = new("a", "b", 20);
        Debt debt2 = new("b", "c", 30);
        Debt debt3 = new("d", "e", 30);

        Verify(
            [debt1, debt2, debt3],
            [
                new Debt("a", "c", 20),
                new Debt("b", "c", 10),
                new Debt("d", "e", 30)
            ]
        );
    }

    [Fact]
    public void Should_test_name_9()
    {
        Verify(
            [
                new Debt("a", "b", 20),
                new Debt("e", "f", 30),
                new Debt("b", "c", 30),
                new Debt("g", "z", 30),
                new Debt("y", "W", 30),
                new Debt("c", "d", 30)
            ],
            [
                new Debt("y", "W", 30),
                new Debt("e", "f", 30),
                new Debt("g", "z", 30),
                new Debt("a", "d", 20),
                new Debt("b", "d", 10)
            ]
        );
    }

    private void Verify(Debt[] register, Debt[] expected)
    {
        Debt[] actual = sut.Do(register);
        actual.Should().BeEquivalentTo(expected, o => o.WithoutStrictOrdering());
    }
}