namespace PokerThursday;

public class Suggest
{
    public Debt[] Do(Debt[] debts)
    {
        while (true)
        {
            Debt[] optimizedDebts = Optimize(debts);
            if (optimizedDebts.SequenceEqual(debts))
                return debts;

            debts = optimizedDebts;
        }
    }

    private static Debt[] Optimize(Debt[] debts)
    {
        (Debt Creditor, Debt Debtor)? result = FindCandidate(debts);
        if (result is null)
            return debts;

        Debt debt1 = result.Value.Creditor;
        Debt debt2 = result.Value.Debtor;

        List<Debt> newDebts = debts.Except([debt1, debt2]).ToList();

        newDebts.Add(OptimizeFirstDebt(debt1, debt2));

        newDebts.Add(OptimizeSecondDebt(debt1, debt2));

        return [.. newDebts.Where(d => d.Amount != 0)];
    }

    private static Debt OptimizeSecondDebt(Debt debt1, Debt debt2)
    {
        if (debt1.Amount > debt2.Amount)
        {
            return debt2 with { Debtor = debt1.Debtor };
        }

        return debt2 with { Amount = debt2.Amount - debt1.Amount };
    }

    private static Debt OptimizeFirstDebt(Debt debt1, Debt debt2)
    {
        if (debt1.Amount > debt2.Amount)
        {
            return debt1 with { Amount = debt1.Amount - debt2.Amount };
        }

        return debt1 with { Creditor = debt2.Creditor };
    }

    private static (Debt Creditor, Debt Debtor)? FindCandidate(Debt[] debts)
    {
        IEnumerable<(Debt d1, Debt d2)> candidates =
            from d1 in debts
            from d2 in debts
            where d1.Creditor == d2.Debtor
            select (d1, d2);

        if (!candidates.Any()) return null;

        return candidates.ToArray()[0];
    }
}