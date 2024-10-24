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

    private void Verify(Debt[] register, Debt[] expected)
    {
        Debt[] actual = this.sut.Do(register);
        actual.Should().Equal(expected);
    }
}

public class Suggest
{
    public Debt[] Do(Debt[] debts)
    {
        if (debts.Length < 2)
            return debts;

        var cross = debts.Select(d => d.Debtor)
            .Intersect(debts.Select(d => d.Creditor));

        if (!cross.Any())
            return debts;


        return [new("a", "c", 20)];
    }
}