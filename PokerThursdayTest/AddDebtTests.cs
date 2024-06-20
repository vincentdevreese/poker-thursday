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
        this.inMemoryDebtRegister.Feed(new DebtRegister([]));
        
        string debtor = "user1";
        string creditor = "creditor";

        this.Verify(new Debt(debtor, creditor, 30.0m),
            new Debt(debtor, creditor, 30.0m)
        );

    }

    [Fact]
    public void ActShouldRegisterAnotherDebt()
    {
        var existingDebts = new List<Debt>();
        existingDebts.Add(new Debt("des trucs", "bidon", 50m));
        
        this.inMemoryDebtRegister.Feed(new DebtRegister(existingDebts));

        string debtor = "vincent";
        string creditor = "dimitri";

        this.Verify(new Debt(debtor, creditor, 120.0m),
            new Debt("des trucs", "bidon", 50m),
            new Debt(debtor, creditor, 120.0m)
        );
    }

    [Fact]
    public void ActShouldRegisterAnotherDebtOnExistingDebtor()
    {
        var existingDebts = new List<Debt>();
        existingDebts.Add(new("vincent", "dimitri", 30m));
        this.inMemoryDebtRegister.Feed(new DebtRegister(existingDebts));

        this.Verify(new Debt("vincent", "dimitri", 120.0m), new Debt("vincent", "dimitri", 150m));
    }

    private void Verify(Debt debt, params Debt[] expected)
    {
        this.sut.Act(debt);

        DebtRegister actual = this.inMemoryDebtRegister.Get();

        actual.ExistingDebts.Should().Equal(expected);
    }
}