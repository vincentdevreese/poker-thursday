namespace PokerThursday
{
    public class InMemoryDebtRegister : IInMemoryDebtRegister
    {
        private DebtRegisterSnapshot snapshot;

        public void Save(DebtRegister register)
        {
            this.snapshot = register.ToSnapshot();
        }

        public void Feed(DebtRegister register)
        {
            this.snapshot = register.ToSnapshot();
        }

        public DebtRegister Get()
        {
            return DebtRegister.From(this.snapshot);
        }
    }
}