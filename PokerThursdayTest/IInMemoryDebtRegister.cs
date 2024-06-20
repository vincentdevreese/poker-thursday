namespace PokerThursdayTest
{
    public interface IInMemoryDebtRegister
    {
        DebtRegister Get();
        void Save(DebtRegister register);
    }
}