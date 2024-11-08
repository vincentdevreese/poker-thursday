namespace PokerThursday;

public record DebtSnapshot(string Debtor, string Creditor, decimal Amount);

public record DebtRegisterSnapshot(List<DebtSnapshot> Debts);

public class DebtRegister(List<Debt> existingDebts)
{
    private List<Debt> existingDebts = existingDebts;

    public DebtRegisterSnapshot ToSnapshot()
    {
        return new DebtRegisterSnapshot(existingDebts.Select(d => d.ToSnapshot()).ToList());
    }

    public static DebtRegister From(DebtRegisterSnapshot snapshot)
    {
        return new DebtRegister(snapshot.Debts.Select(d => new Debt(d.Debtor, d.Creditor, d.Amount)).ToList());
    }

    public void Act(Debt debt)
    {
        EnsureIsValid(debt);

        if (ShouldIgnore(debt))
            return;

        decimal totalAmount = debt.Amount;

        List<Debt> toto = [];
        foreach (Debt item in existingDebts)
            if (item.Debtor != debt.Debtor || item.Creditor != debt.Creditor)
                toto.Add(item);
            else
                totalAmount += item.Amount;

        existingDebts = toto.Concat([new Debt(debt.Debtor, debt.Creditor, totalAmount)]).ToList();
    }

    public void Pay(Debt debt)
    {
        EnsureIsValid(debt);

        if (ShouldIgnore(debt))
            return;

        Debt? found = existingDebts.SingleOrDefault(x => x.Debtor == debt.Debtor && x.Creditor == debt.Creditor);

        if (found is null) return;

        if (found.Amount - debt.Amount < 0)
            throw new PayDebtAmountOverException();

        existingDebts.Remove(found);
        if (found.Amount - debt.Amount == 0) return;

        found = found with { Amount = found.Amount - debt.Amount };
        existingDebts.Add(found);
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
    {
        return debt.Amount <= 0;
    }
}