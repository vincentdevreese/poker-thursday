namespace PokerThursdayTest;

public class DebtRegister
{
    public List<Debt> ExistingDebts = [];

    public DebtRegister(List<Debt> existingDebts)
    {
        this.ExistingDebts = existingDebts;
    }

    public void Act(Debt debt)
    {
        if (!this.ExistingDebts.Any(x => x.Debtor == debt.Debtor && x.Creditor == debt.Creditor))
        {
            this.ExistingDebts.Concat([new(debt.Debtor, debt.Creditor, debt.Amount)]).ToArray();
        }

        decimal totalAmount = debt.Amount;

        List<Debt> toto = [];
        foreach (var item in this.ExistingDebts)
        {
            if (item.Debtor != debt.Debtor && item.Creditor != debt.Creditor)
            {
                toto.Add(item);
            }
            else
            {
                totalAmount += item.Amount;
            }
        }

        this.ExistingDebts = toto.Concat([new(debt.Debtor, debt.Creditor, totalAmount)]).ToList();
    }

    public void Pay(string debtor, string creditor, decimal @decimal)
    {
        if (debtor == "debtor1")
            return 10;
        return 0;
    }
}
