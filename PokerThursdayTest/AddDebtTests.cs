using FluentAssertions;

using PokerThursday;

using PokerThursdayTest.AutoFixture;

namespace PokerThursdayTest;

// TODO redondance test, x dettes?
// TODO x a payé sa dette (effacer l'ardoise)
// StrykerMutator
public class AddDebtTests
{
    private readonly InMemoryDebtRegister inMemoryDebtRegister = new();

    private readonly AddDebt sut;

    public AddDebtTests()
    {
        sut = new AddDebt(this.inMemoryDebtRegister);
    }

    [Fact]
    public void AddShouldRegisterDebtInRegistry()
    {
        DebtRegister debtRegister = new([]);
        this.inMemoryDebtRegister.Feed(debtRegister);

        string debtor = "user1";
        string creditor = "creditor";

        this.Verify(new Debt(debtor, creditor, 30.0m),
            debtRegister.ToSnapshot() with { Debts = [new DebtSnapshot(debtor, creditor, 30.0m)] }
        );
    }

    [Fact]
    public void AddShouldRegisterAnotherDebt()
    {
        var existingDebts = new List<Debt>();
        existingDebts.Add(new Debt("des trucs", "bidon", 50m));

        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        string debtor = "vincent";
        string creditor = "dimitri";

        this.Verify(new Debt(debtor, creditor, 120.0m),
            debtRegister.ToSnapshot() with
            {
                Debts =
                [
                    new DebtSnapshot("des trucs", "bidon", 50m),
                    new DebtSnapshot(debtor, creditor, 120.0m)
                ]
            });
    }

    [Fact]
    public void AddShouldRegisterAnotherDebt2()
    {
        var existingDebts = new List<Debt>();
        existingDebts.Add(new Debt("des trucs", "bidon", 50m));

        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        string debtor = "des trucs";
        string creditor = "des choses";

        this.Verify(new Debt(debtor, creditor, 120.0m),
            debtRegister.ToSnapshot() with
            {
                Debts =
                [
                    new DebtSnapshot("des trucs", "bidon", 50m),
                    new DebtSnapshot(debtor, creditor, 120.0m)
                ]
            });
    }

    [Fact]
    public void AddShouldRegisterAnotherDebtOnExistingDebtor()
    {
        var existingDebts = new List<Debt>
        {
            new Debt("vincent", "dimitri", 30m)
        };
        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        this.Verify(new Debt("vincent", "dimitri", 120.0m), debtRegister.ToSnapshot() with
        {
            Debts =
            [
                new DebtSnapshot("vincent", "dimitri", 150m)
            ]
        });
    }

    [Theory]
    [InlineRandomData(0)]
    [InlineRandomData(-10)]
    public void AddShouldIgnoreDebtWhenAmountIsInvalid(int amount, Debt debt, DebtRegisterSnapshot debtRegister)
    {
        Feed(debtRegister);

        this.Verify(debt with { Amount = amount }, debtRegister);
    }

    [Theory]
    [RandomData]
    public void AddShouldFailWhenDebtorInvalid(Debt debt, DebtRegisterSnapshot debtRegister)
    {
        Feed(debtRegister);

        this.Invoking(s => s.Verify(debt with { Debtor = string.Empty }, debtRegister)).Should()
            .Throw<InvalidNameException>();
    }

    [Theory]
    [RandomData]
    public void AddShouldFailWhenCreditorIsInvalid(Debt debt, DebtRegisterSnapshot debtRegister)
    {
        Feed(debtRegister);

        this.Invoking(s => s.Verify(debt with { Creditor = string.Empty }, debtRegister)).Should()
            .Throw<InvalidNameException>();
    }

    [Theory]
    [RandomData]
    public void AddShouldFailWhenDebtorNameEqualsCreditorName(string someone, Debt debt, DebtRegisterSnapshot debtRegister)
    {
        Feed(debtRegister);

        this.Invoking(s => s.Verify(debt with { Debtor = someone, Creditor = someone }, debtRegister)).Should()
            .Throw<InvalidNameException>();
    }

    private void Feed(DebtRegisterSnapshot debtRegister)
    {
        this.inMemoryDebtRegister.Feed(debtRegister);
    }

    private void Verify(Debt debt, DebtRegisterSnapshot expected)
    {
        this.sut.Add(debt);

        DebtRegister actual = this.inMemoryDebtRegister.Get();

        actual.ToSnapshot().Should().BeEquivalentTo(expected);
    }
}
