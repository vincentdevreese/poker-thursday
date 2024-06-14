
namespace PokerThursdayTest
{
    public class InMemoryDebtRegister : IInMemoryDebtRegister
    {
        private DebtRegister register;

        public void Save(DebtRegister register)
        {
            this.register = register;
        }

        internal DebtRegister Get()
        {
            return register;
        }
    }
}