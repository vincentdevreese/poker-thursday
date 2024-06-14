namespace PokerThursdayTest;

public class AddDebt
{
    private IInMemoryDebtRegister inMemoryDebtRegister;

    public AddDebt(IInMemoryDebtRegister inMemoryDebtRegister)
    {
        this.inMemoryDebtRegister = inMemoryDebtRegister;
    }

    public void Act(DebtRegister debtRegister, Debt debt)
    {
        debtRegister.Act(debt);
        this.inMemoryDebtRegister.Save(debtRegister);
    }
}