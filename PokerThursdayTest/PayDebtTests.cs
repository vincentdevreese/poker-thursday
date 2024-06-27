using FluentAssertions;

namespace PokerThursdayTest;

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

        Verify(new Debt(targetDebtor, targetCreditor, 10.0m), [new Debt(existingDebtor, existingCreditor, 10)]);
    }

    [Fact]
    public void PayShouldEraseDebtFromDebtRegister()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new("debtor1", "creditor", 80m), []);
    }

    [Fact]
    public void PayShouldUpdateDebtFromDebtRegister()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new("debtor1", "creditor", 60m), [new Debt("debtor1", "creditor", 20.0m)]);
    }

    [Fact]
    public void PayShouldFailWhenAmountIsOverDebt()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m)]);

        inMemoryDebtRegister.Feed(register);

        this.Invoking(s => s.Verify(new("debtor1", "creditor", 90m), [])).Should().Throw<PayDebtAmountOverException>();
    }

    private void Verify(Debt debt, params Debt[] expected)
    {
        sut.Pay(debt);

        DebtRegister actual = inMemoryDebtRegister.Get();

        actual.ExistingDebts.Should().Equal(expected);
    }
}