using FluentAssertions;

namespace PokerThursdayTest;

// TODO redondance test, x dettes?
// TODO x a payé sa dette (effacer l'ardoise)
// StrykerMutator
public class AddDebtTests
{
    private InMemoryDebtRegister inMemoryDebtRegister = new();

    private AddDebt sut;

    public AddDebtTests()
    {
        sut = new(inMemoryDebtRegister);
    }

    [Fact]
    public void ActShouldRegisterDebtInRegistry()
    {
        DebtRegister debtRegister = new DebtRegister([]);
        this.inMemoryDebtRegister.Feed(debtRegister);

        string debtor = "user1";
        string creditor = "creditor";

        this.Verify(new Debt(debtor, creditor, 30.0m),
            debtRegister.ToSnapshot() with { Debts = [new DebtSnapshot(debtor, creditor, 30.0m)] }
        );
    }

    [Fact]
    public void ActShouldRegisterAnotherDebt()
    {
        var existingDebts = new List<Debt>();
        existingDebts.Add(new Debt("des trucs", "bidon", 50m));

        DebtRegister debtRegister = new DebtRegister(existingDebts);
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
    public void ActShouldRegisterAnotherDebtOnExistingDebtor()
    {
        var existingDebts = new List<Debt>();
        existingDebts.Add(new("vincent", "dimitri", 30m));
        DebtRegister debtRegister = new DebtRegister(existingDebts);
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
        this.sut.Act(debt);

        DebtRegister actual = this.inMemoryDebtRegister.Get();

        actual.ToSnapshot().Should().BeEquivalentTo(expected);
    }
}