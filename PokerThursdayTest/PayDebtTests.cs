using FluentAssertions;

namespace PokerThursdayTest;

// TODO: partially pay
public class PayDebtTests
{
    private InMemoryDebtRegister inMemoryDebtRegister = new();

    private PayDebt sut;

    [Fact]
    public void PayShouldEraseDebtFromDebtRegister()
    {
        var sut = new DebtRegister([]);

        decimal debtResult = sut.Pay("debtor", "creditor", 80m);
        debtResult.Should().Be(0m);
    }

    [Fact]
    public void PayShouldEraseDebtFromDebtRegister2()
    {
        var sut = new DebtRegister([]);

        decimal debtResult = sut.Pay("debtor1", "creditor", 70m);
        debtResult.Should().Be(10m);
    }
}