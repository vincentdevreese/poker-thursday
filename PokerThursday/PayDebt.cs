namespace PokerThursday
{
    public class PayDebt
    {
        private readonly IInMemoryDebtRegister inMemoryDebtRegister;

        public PayDebt(IInMemoryDebtRegister inMemoryDebtRegister)
        {
            this.inMemoryDebtRegister = inMemoryDebtRegister;
        }

        public void Pay(Debt debt)
        {
            var debtRegister = this.inMemoryDebtRegister.Get();
            debtRegister.Pay(debt);
            this.inMemoryDebtRegister.Save(debtRegister);
        }
    }
}
