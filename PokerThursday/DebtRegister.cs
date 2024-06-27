namespace PokerThursday;

public record DebtSnapshot(string Debtor, string Creditor, decimal Amount);

public record DebtRegisterSnapshot(List<DebtSnapshot> Debts);

public class DebtRegister
{
    private List<Debt> existingDebts = [];

    public DebtRegister(List<Debt> existingDebts)
    {
        this.existingDebts = existingDebts;
    }

    public DebtRegisterSnapshot ToSnapshot() => new(this.existingDebts.Select(d => d.ToSnapshot()).ToList());
    
    public static DebtRegister From(DebtRegisterSnapshot snapshot) =>
        new(snapshot.Debts.Select(d => new Debt(d.Debtor, d.Creditor, d.Amount)).ToList());

    public void Act(Debt debt)
    {
        if (this.existingDebts.Any(x => x.Debtor == debt.Debtor && x.Creditor == debt.Creditor))
        {
            this.existingDebts.Concat([new Debt(debt.Debtor, debt.Creditor, debt.Amount)]).ToArray();
        }

        decimal totalAmount = debt.Amount;

        List<Debt> toto = [];
        foreach (var item in this.existingDebts)
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

        this.existingDebts = toto.Concat([new Debt(debt.Debtor, debt.Creditor, totalAmount)]).ToList();
    }

    public void Pay(Debt debt)
    {
        var found = this.existingDebts.SingleOrDefault(x => x.Debtor == debt.Debtor && x.Creditor == debt.Creditor);

        if (found is not null)
        {
            if (found.Amount - debt.Amount < 0)
                throw new PayDebtAmountOverException();
            this.existingDebts.Remove(found);
            if (found.Amount - debt.Amount != 0)
            {
                found = found with { Amount = found.Amount - debt.Amount };
                this.existingDebts.Add(found);
            }
        }
    }
}
