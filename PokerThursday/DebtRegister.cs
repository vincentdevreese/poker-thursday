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
        EnsureIsValid(debt);

        if (ShouldIgnore(debt))
            return;

        decimal totalAmount = debt.Amount;

        List<Debt> toto = [];
        foreach (var item in this.existingDebts)
        {
            if (item.Debtor != debt.Debtor || item.Creditor != debt.Creditor)
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
        EnsureIsValid(debt);

        if (ShouldIgnore(debt))
            return;

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

    private static void EnsureIsValid(Debt debt)
    {
        if (debt.Debtor == "")
            throw new InvalidNameException();

        if (debt.Creditor == "")
            throw new InvalidNameException();

        if (debt.Creditor == debt.Debtor)
            throw new InvalidNameException();
    }

    private static bool ShouldIgnore(Debt debt)
        => debt.Amount <= 0;
}
