namespace PokerThursday;

public class InMemoryDebtRegister : IInMemoryDebtRegister
{
    private DebtRegisterSnapshot snapshot = default!;

    public void Save(DebtRegister register)
    {
        snapshot = register.ToSnapshot();
    }

    public DebtRegister Get()
    {
        return DebtRegister.From(snapshot);
    }

    public void Feed(DebtRegister register)
    {
        snapshot = register.ToSnapshot();
    }

    public void Feed(DebtRegisterSnapshot register)
    {
        snapshot = register;
    }
}