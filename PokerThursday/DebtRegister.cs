namespace PokerThursday;

public record DebtSnapshot(string Debtor, string Creditor, decimal Amount);

public record DebtRegisterSnapshot(List<DebtSnapshot> Debts);

public class DebtRegister(List<Debt> existingDebts)
{
    public DebtRegisterSnapshot ToSnapshot() => new(existingDebts.Select(d => d.ToSnapshot()).ToList());

    public static DebtRegister From(DebtRegisterSnapshot snapshot) =>
        new(snapshot.Debts.Select(d => new Debt(d.Debtor, d.Creditor, d.Amount)).ToList());

    public void Act(Debt debt)
    {
        EnsureIsValid(debt);

        if (ShouldIgnore(debt))
            return;

        Debt? creditorExistingDebt =
            existingDebts.SingleOrDefault(x =>
                (x.Debtor == debt.Creditor && x.Creditor == debt.Debtor)
                ||
                (x.Debtor == debt.Debtor && x.Creditor == debt.Creditor));

        if (creditorExistingDebt is not null)
        {
            existingDebts.Remove(creditorExistingDebt);

            if (creditorExistingDebt.Debtor == debt.Debtor)
                debt = debt with { Amount = debt.Amount + creditorExistingDebt.Amount };
            else
            {
                if (debt.Amount > creditorExistingDebt.Amount)
                    debt = new Debt(debt.Debtor, debt.Creditor, debt.Amount - creditorExistingDebt.Amount);
                else if (debt.Amount < creditorExistingDebt.Amount)
                    debt = new Debt(debt.Creditor, debt.Debtor, creditorExistingDebt.Amount - debt.Amount);
                else return;
            }
        }

        existingDebts.Add(debt);
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