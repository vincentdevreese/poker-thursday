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
        string debtor = "user1";
        string creditor = "creditor";

        this.sut.Act(new DebtRegister([]), new Debt(debtor, creditor, 30.0m));

        DebtRegister actual = inMemoryDebtRegister.Get();

        actual.ExistingDebts.Should().Equal(new Debt(debtor, creditor, 30.0m));
    }

    [Fact]
    public void ActShouldRegisterAnotherDebt()
    {
        var existingDebts = new List<Debt>();
        existingDebts.Add(new Debt("des trucs", "bidon", 50m));

        string debtor = "vincent";
        string creditor = "dimitri";
        sut.Act(new DebtRegister(existingDebts), new Debt(debtor, creditor, 120.0m));

        DebtRegister actual = inMemoryDebtRegister.Get();

        actual.ExistingDebts.Should().Equal(
            new Debt("des trucs", "bidon", 50m),
            new Debt(debtor, creditor, 120.0m)
        );
    }

    [Fact]
    public void ActShouldRegisterAnotherDebtOnExistingDebtor()
    {
        var existingDebts = new List<Debt>();
        existingDebts.Add(new("vincent", "dimitri", 30m));

        sut.Act(new DebtRegister(existingDebts), new Debt("vincent", "dimitri", 120.0m));

        DebtRegister actual = inMemoryDebtRegister.Get();

        actual.ExistingDebts.Should().Equal(new Debt("vincent", "dimitri", 150m));
    }
}