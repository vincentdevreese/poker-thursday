using FluentAssertions;

namespace PokerThursdayTest;

// TODO: partially pay
public class PayDebtTests
{
    private InMemoryDebtRegister inMemoryDebtRegister = new();

    private PayDebt sut;

    public PayDebtTests()
    {
        sut = new(inMemoryDebtRegister);
    }

    [Fact]
    public void PayShouldDoNothingWhenNoDebtExistsForDebtorAndCreditor()
    {
        var register = new DebtRegister([]);

        inMemoryDebtRegister.Feed(register);

        Verify(new Debt("debtor1", "creditor", 10.0m), []);
    }

    [Fact]
    public void PayShouldEraseDebtFromDebtRegister2()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new("debtor1", "creditor", 70m), new Debt("debtor1", "creditor", 10.0m));
    }

    [Fact]
    public void PayShouldEraseDebtFromDebtRegister3()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new("debtor1", "creditor", 60m), new Debt("debtor1", "creditor", 20.0m));
    }

    [Fact]
    public void PayShouldEraseDebtFromDebtRegister4()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m), new("debtor2", "creditor", 30m)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new("debtor1", "creditor", 60m), new Debt("debtor1", "creditor", 20.0m));
    }

    [Fact]
    public void PayShouldEraseDebtFromDebtRegister5()
    {
        var register = new DebtRegister([new("debtor1", "creditor", 80m), new("debtor1", "creditor1", 30m)]);

        inMemoryDebtRegister.Feed(register);

        Verify(new("debtor1", "creditor", 60m), new Debt("debtor1", "creditor", 20.0m));
    }

    private void Verify(Debt debt, params Debt[] expected)
    {
        sut.Pay(debt);

        DebtRegister actual = inMemoryDebtRegister.Get();

        actual.ExistingDebts.Should().Equal(expected);
    }
}