namespace PokerThursdayTest;

public class AddDebt
{
    private IInMemoryDebtRegister inMemoryDebtRegister;

    public AddDebt(IInMemoryDebtRegister inMemoryDebtRegister)
    {
        this.inMemoryDebtRegister = inMemoryDebtRegister;
    }

    public void Act(Debt debt)
    {
        var debtRegister = this.inMemoryDebtRegister.Get();
        debtRegister.Act(debt);
        this.inMemoryDebtRegister.Save(debtRegister);
    }
}