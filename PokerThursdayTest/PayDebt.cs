namespace PokerThursdayTest
{
    public class PayDebt
    {
        private IInMemoryDebtRegister inMemoryDebtRegister;

        public PayDebt(IInMemoryDebtRegister inMemoryDebtRegister)
        {
            this.inMemoryDebtRegister = inMemoryDebtRegister;
        }

        public void Pay(Debt debt)
        {
            var debtRegister = inMemoryDebtRegister.Get();
            debtRegister.Pay(debt);
            this.inMemoryDebtRegister.Save(debtRegister);
        }
    }
}