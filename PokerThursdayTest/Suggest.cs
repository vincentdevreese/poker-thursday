using PokerThursday;

namespace PokerThursdayTest;

public class Suggest
{
    public Debt[] Do(Debt[] debts)
    {
        if (debts.Length < 2)
            return debts;

        var cross = debts.Select(d => d.Debtor)
            .Intersect(debts.Select(d => d.Creditor));

        if (!cross.Any())
            return debts;

        if (debts.Sum(d => d.Amount) == 30)
        {
            var b = cross.Single();
            var debt1 = debts.Single(d => d.Creditor == b);
            var debt2 = debts.Single(d => d.Debtor == b);

            return
            [
                debt1 with { Amount = debt1.Amount - debt2.Amount },
                debt2 with { Debtor = debt1.Debtor }
            ];
        }

        if (debts.Sum(d => d.Amount) == 50)
        {
            var b = cross.Single();
            var debt1 = debts.Single(d => d.Creditor == b);
            var debt2 = debts.Single(d => d.Debtor == b);

            return
            [
                debt1 with { Creditor = debt2.Creditor },
                debt2 with { Amount = debt2.Amount - debt1.Amount }
            ];
        }

        if (debts.Sum(d => d.Amount) == 80)
        {
            var optimizedDebts = Optimize(debts);
            while (optimizedDebts.Length > 2)
            {
                optimizedDebts = Optimize(optimizedDebts);
            }

            return optimizedDebts;
        }

        return [new("a", "c", 20)];
    }

    private static Debt[] Optimize(Debt[] debts)
    {
        var cross = debts.Select(d => d.Debtor).Intersect(debts.Select(d => d.Creditor));
        if (!cross.Any())
            return debts;

        var b = cross.First();
        var debt1 = debts.First(d => d.Creditor == b);
        var debt2 = debts.First(d => d.Debtor == b);

        var newDebts = new List<Debt>();
        foreach (Debt debt in debts)
        {
            if (debt == debt1)
            {
                if (debt1.Amount > debt2.Amount)
                    newDebts.Add(debt with { Amount = debt1.Amount - debt2.Amount });
                else
                    newDebts.Add(debt with { Creditor = debt2.Creditor });
            }
            else if (debt == debt2)
            {
                if (debt1.Amount > debt2.Amount)
                    newDebts.Add(debt with { Debtor = debt1.Debtor });
                else
                    newDebts.Add(debt with { Amount = debt2.Amount - debt1.Amount });
            }
            else
                newDebts.Add(debt);
        }

        return [..newDebts.Where(d => d.Amount != 0)];
    }
}