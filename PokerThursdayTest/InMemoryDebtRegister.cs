

using PokerThursday;

namespace PokerThursdayTest
{
    public class InMemoryDebtRegister : IInMemoryDebtRegister
    {
        private DebtRegisterSnapshot snapshot;

        public void Save(DebtRegister register)
        {
            this.snapshot = register.ToSnapshot();
        }

        internal void Feed(DebtRegister register)
        {
            this.snapshot = register.ToSnapshot();
        }

        public DebtRegister Get()
        {
            return DebtRegister.From(this.snapshot);
        }
    }
}