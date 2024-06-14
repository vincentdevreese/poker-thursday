namespace PokerThursdayTest
{
    public class PayDebt
    {
        private IInMemoryDebtRegister inMemoryDebtRegister;

        public PayDebt(IInMemoryDebtRegister inMemoryDebtRegister)
        {
            this.inMemoryDebtRegister = inMemoryDebtRegister;
        }

        public void Pay(DebtRegister debtRegister, Debt debt)
        {
            debtRegister.Pay(debt);
            this.inMemoryDebtRegister.Save(debtRegister);
        }
    }
}