

namespace PokerThursdayTest
{
    public class InMemoryDebtRegister : IInMemoryDebtRegister
    {
        private DebtRegister register;

        public void Save(DebtRegister register)
        {
            this.register = register;
        }

        internal void Feed(DebtRegister register)
        {
            this.register = register;
        }

        public DebtRegister Get()
        {
            return register;
        }
    }
}