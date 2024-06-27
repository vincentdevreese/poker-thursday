using FluentAssertions;
using PokerThursdayTest.AutoFixture;
using static PokerThursdayTest.Randomizer;

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
    [RandomData]
    public void PayShouldDoNothingWhenNoDebtExistsForDebtorAndCreditor(Debt existingDebt)
    {
        var register = new DebtRegister([existingDebt]);

        this.Feed(register);

        this.Verify(existingDebt with { Debtor = Another(existingDebt.Debtor) }, register.ToSnapshot());
        this.Verify(existingDebt with { Creditor = Another(existingDebt.Creditor) }, register.ToSnapshot());
    }

    [Theory]
    [RandomData]
    public void PayShouldEraseDebtFromDebtRegister(Debt existingDebt)
    {
        var register = new DebtRegister([existingDebt]);

        this.Feed(register);

        this.Verify(existingDebt, register.ToSnapshot() with { Debts = [] });
    }

    [Theory]
    [RandomData]
    public void PayShouldUpdateDebtFromDebtRegister(Debt existingDebt)
    {
        var register = new DebtRegister([existingDebt with { Amount = 80m }]);

        this.Feed(register);

        Verify(
            existingDebt with { Amount = 60 },
            register.ToSnapshot() with { Debts = [existingDebt.ToSnapshot() with { Amount = 20 }] }
        );
    }


    [Theory]
    [RandomData]
    public void PayShouldFailWhenAmountIsOverDebt(Debt existingDebt)
    {
        var register = new DebtRegister([existingDebt with { Amount = 80 }]);

        this.Feed(register);

        this.Invoking(s => s.Verify(existingDebt with { Amount = 90 }, register.ToSnapshot())).Should()
            .Throw<PayDebtAmountOverException>();
    }

    private void Verify(Debt debt, DebtRegisterSnapshot expected)
    {
        sut.Pay(debt);

        DebtRegister actual = inMemoryDebtRegister.Get();

        actual.ToSnapshot().Should().BeEquivalentTo(expected);
    }

    private void Feed(DebtRegister register)
    {
        this.inMemoryDebtRegister.Feed(register);
    }
}