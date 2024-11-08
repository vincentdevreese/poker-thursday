namespace PokerThursday;

public class PayDebt(IInMemoryDebtRegister inMemoryDebtRegister)
{
    public void Pay(Debt debt)
    {
        DebtRegister debtRegister = inMemoryDebtRegister.Get();
        debtRegister.Pay(debt);
        inMemoryDebtRegister.Save(debtRegister);
    }
}