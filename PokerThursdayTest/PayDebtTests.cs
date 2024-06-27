using FluentAssertions;

namespace PokerThursdayTest;

// StrykerMutator
// AutoFixture
// Builder
public class PayDebtTests
{
    private InMemoryDebtRegister inMemoryDebtRegister = new();

    private PayDebt sut;

    public PayDebtTests()
    {
        sut = new(inMemoryDebtRegister);
    }

    [Theory]
    [InlineData("vincent", "dimitri", "vincent", "daniel")]
    [InlineData("vincent", "dimitri", "daniel", "dimitri")]
    public void PayShouldDoNothingWhenNoDebtExistsForDebtorAndCreditor(string existingDebtor, string existingCreditor,
        string targetDebtor, string targetCreditor)
    {
        var register = new DebtRegister([new Debt(existingDebtor, existingCreditor, 10)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new Debt(targetDebtor, targetCreditor, 10.0m), register.ToSnapshot());
    }

    [Fact]
    public void PayShouldEraseDebtFromDebtRegister()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new("debtor1", "creditor", 80m), register.ToSnapshot() with { Debts = [] });
    }

    [Fact]
    public void PayShouldUpdateDebtFromDebtRegister()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new("debtor1", "creditor", 60m),
            register.ToSnapshot() with { Debts = [new DebtSnapshot("debtor1", "creditor", 20.0m)] });
    }

    [Fact]
    public void PayShouldFailWhenAmountIsOverDebt()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m)]);

        inMemoryDebtRegister.Feed(register);

        this.Invoking(s => s.Verify(new("debtor1", "creditor", 90m), register.ToSnapshot())).Should()
            .Throw<PayDebtAmountOverException>();
    }

    private void Verify(Debt debt, DebtRegisterSnapshot expected)
    {
        sut.Pay(debt);

        DebtRegister actual = inMemoryDebtRegister.Get();

        actual.ToSnapshot().Should().BeEquivalentTo(expected);
    }
}