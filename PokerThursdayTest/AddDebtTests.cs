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
        this.sut = new AddDebt(this.inMemoryDebtRegister);
    }

    [Fact]
    public void AddShouldRegisterAnotherDebt2()
    {
        List<Debt> existingDebts = [new("des trucs", "bidon", 50m)];

        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        const string debtor = "des trucs";
        const string creditor = "des choses";

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
        List<Debt> existingDebts = [new("vincent", "dimitri", 30m)];
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


    [Fact]
    public void AddShouldRegisterAnotherDebtOnExistingDebtor100()
    {
        List<Debt> existingDebts =
        [
            new("vincent", "dimitri", 30m),
            new("claude", "francois", 50m)
        ];
        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        this.Verify(new Debt("vincent", "dimitri", 120.0m), debtRegister.ToSnapshot() with
        {
            Debts =
            [
                new DebtSnapshot("claude", "francois", 50m),
                new DebtSnapshot("vincent", "dimitri", 150m),
            ]
        });
    }


    [Fact]
    public void AddShouldRegisterAnotherDebtOnExistingDebtor1()
    {
        List<Debt> existingDebts = [new("vincent", "dimitri", 30m)];
        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        this.Verify(new Debt("dimitri", "vincent", 55.0m), debtRegister.ToSnapshot() with
        {
            Debts =
            [
                new DebtSnapshot("dimitri", "vincent", 25m)
            ]
        });
    }

    [Fact]
    public void AddShouldRegisterAnotherDebtOnExistingDebtor2()
    {
        List<Debt> existingDebts = [new("vincent", "dimitri", 130m)];
        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        this.Verify(new Debt("dimitri", "vincent", 55.0m), debtRegister.ToSnapshot() with
        {
            Debts =
            [
                new DebtSnapshot("vincent", "dimitri", 75m)
            ]
        });
    }

    [Fact]
    public void AddShouldRegisterAnotherDebtOnExistingDebtor3()
    {
        List<Debt> existingDebts =
        [
            new("vincent", "dimitri", 130m),
            new("claude", "hervé", 10m)
        ];
        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        this.Verify(new Debt("dimitri", "vincent", 55.0m), debtRegister.ToSnapshot() with
        {
            Debts =
            [
                new DebtSnapshot("claude", "hervé", 10m),
                new DebtSnapshot("vincent", "dimitri", 75m),
            ]
        });
    }

    [Fact]
    public void AddShouldRegisterAnotherDebtOnExistingDebtor4()
    {
        List<Debt> existingDebts =
        [
            new("vincent", "dimitri", 130m),
            new("vincent", "hervé", 10m),
            new("dimitri", "cladue", 20m)
        ];
        DebtRegister debtRegister = new(existingDebts);
        this.inMemoryDebtRegister.Feed(debtRegister);

        this.Verify(new Debt("dimitri", "vincent", 130.0m), debtRegister.ToSnapshot() with
        {
            Debts =
            [
                new DebtSnapshot("vincent", "hervé", 10m),
                new DebtSnapshot("dimitri", "cladue", 20m)
            ]
        });
    }


    [Theory]
    [InlineRandomData(0)]
    [InlineRandomData(-10)]
    public void AddShouldIgnoreDebtWhenAmountIsInvalid(int amount, Debt debt, DebtRegisterSnapshot debtRegister)
    {
        this.Feed(debtRegister);

        this.Verify(debt with { Amount = amount }, debtRegister);
    }

    [Theory]
    [RandomData]
    public void AddShouldFailWhenDebtorInvalid(Debt debt, DebtRegisterSnapshot debtRegister)
    {
        this.Feed(debtRegister);

        this.Invoking(s => s.Verify(debt with { Debtor = string.Empty }, debtRegister)).Should()
            .Throw<InvalidNameException>();
    }

    [Theory]
    [RandomData]
    public void AddShouldFailWhenCreditorIsInvalid(Debt debt, DebtRegisterSnapshot debtRegister)
    {
        this.Feed(debtRegister);

        this.Invoking(s => s.Verify(debt with { Creditor = string.Empty }, debtRegister)).Should()
            .Throw<InvalidNameException>();
    }

    [Theory]
    [RandomData]
    public void AddShouldFailWhenDebtorNameEqualsCreditorName(string someone, Debt debt,
        DebtRegisterSnapshot debtRegister)
    {
        this.Feed(debtRegister);

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

        actual.ToSnapshot().Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }
}