namespace PokerThursday;

public class AddDebt
{
    private readonly IInMemoryDebtRegister inMemoryDebtRegister;

    public AddDebt(IInMemoryDebtRegister inMemoryDebtRegister)
    {
        this.inMemoryDebtRegister = inMemoryDebtRegister;
    }

    public void Add(Debt debt)
    {
        var debtRegister = this.inMemoryDebtRegister.Get();
        debtRegister.Act(debt);
        this.inMemoryDebtRegister.Save(debtRegister);
    }
}