using FluentAssertions;

using PokerThursday;

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

    private void Verify(Debt debt, DebtRegisterSnapshot expected)
    {
        this.sut.Add(debt);

        DebtRegister actual = this.inMemoryDebtRegister.Get();

        actual.ToSnapshot().Should().BeEquivalentTo(expected);
    }
}
