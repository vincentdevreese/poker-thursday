namespace PokerThursday;

public class AddDebt(IInMemoryDebtRegister inMemoryDebtRegister)
{
    public void Add(Debt debt)
    {
        DebtRegister debtRegister = inMemoryDebtRegister.Get();
        debtRegister.Act(debt);
        inMemoryDebtRegister.Save(debtRegister);
    }
}