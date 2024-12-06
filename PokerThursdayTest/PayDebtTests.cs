using FluentAssertions;
using PokerThursday;
using PokerThursdayTest.AutoFixture;
using static PokerThursdayTest.Randomizer;

namespace PokerThursdayTest;

public class PayDebtTests
{
    private readonly InMemoryDebtRegister inMemoryDebtRegister = new();

    private readonly PayDebt sut;

    public PayDebtTests()
    {
        sut = new PayDebt(inMemoryDebtRegister);
    }

    [Theory]
    [RandomData]
    public void ShouldDoNothingWhenNoDebtExistsForDebtorAndCreditor(Debt existingDebt)
    {
        DebtRegister register = new([existingDebt]);

        Feed(register);

        Verify(existingDebt with { Debtor = Another(existingDebt.Debtor) }, register.ToSnapshot());
        Verify(existingDebt with { Creditor = Another(existingDebt.Creditor) }, register.ToSnapshot());
    }

    [Theory]
    [RandomData]
    public void ShouldEraseDebtFromDebtRegister(Debt existingDebt)
    {
        DebtRegister register = new([existingDebt]);

        Feed(register);

        Verify(existingDebt, register.ToSnapshot() with { Debts = [] });
    }

    [Theory]
    [RandomData]
    public void ShouldUpdateDebtFromDebtRegister(Debt existingDebt)
    {
        DebtRegister register = new([existingDebt with { Amount = 80m }]);

        Feed(register);

        Verify(
            existingDebt with { Amount = 60 },
            register.ToSnapshot() with { Debts = [existingDebt.ToSnapshot() with { Amount = 20 }] }
        );
    }


    [Theory]
    [RandomData]
    public void ShouldFailWhenAmountIsOverDebt(Debt existingDebt)
    {
        DebtRegister register = new([existingDebt with { Amount = 80 }]);

        Feed(register);

        this.Invoking(s => s.Verify(existingDebt with { Amount = 90 }, register.ToSnapshot())).Should()
            .Throw<PayDebtAmountOverException>();
    }


    [Theory]
    [InlineRandomData(0)]
    [InlineRandomData(-10)]
    public void ShouldIgnoreDebtWhenAmountIsInvalid(int amount, Debt existingDebt)
    {
        DebtRegister register = new([existingDebt]);

        Feed(register);

        Verify(existingDebt with { Amount = amount }, register.ToSnapshot());
    }

    [Theory]
    [RandomData]
    public void ShouldFailWhenDebtorInvalid(Debt debt, DebtRegisterSnapshot debtRegister)
    {
        Feed(debtRegister);

        this.Invoking(s => s.Verify(debt with { Debtor = string.Empty }, debtRegister)).Should()
            .Throw<InvalidNameException>();
    }

    [Theory]
    [RandomData]
    public void ShouldFailWhenCreditorIsInvalid(Debt debt, DebtRegisterSnapshot debtRegister)
    {
        Feed(debtRegister);

        this.Invoking(s => s.Verify(debt with { Creditor = string.Empty }, debtRegister)).Should()
            .Throw<InvalidNameException>();
    }

    [Theory]
    [RandomData]
    public void ShouldFailWhenDebtorNameEqualsCreditorName(string someone, Debt debt, DebtRegisterSnapshot debtRegister)
    {
        Feed(debtRegister);

        this.Invoking(s => s.Verify(debt with { Debtor = someone, Creditor = someone }, debtRegister)).Should()
            .Throw<InvalidNameException>();
    }

    private void Verify(Debt debt, DebtRegisterSnapshot expected)
    {
        sut.Pay(debt);

        DebtRegister actual = inMemoryDebtRegister.Get();

        actual.ToSnapshot().Should().BeEquivalentTo(expected);
    }

    private void Feed(DebtRegister register)
    {
        inMemoryDebtRegister.Feed(register);
    }

    private void Feed(DebtRegisterSnapshot debtRegister)
    {
        inMemoryDebtRegister.Feed(debtRegister);
    }
}