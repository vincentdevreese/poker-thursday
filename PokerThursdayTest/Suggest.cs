using PokerThursday;

namespace PokerThursdayTest;

public class Suggest
{
    public Debt[] Do(Debt[] debts)
    {
        while (true)
        {
            var optimizedDebts = Optimize(debts);
            if (optimizedDebts.SequenceEqual(debts))
                return debts;

            debts = optimizedDebts;
        }
    }

    private static Debt[] Optimize(Debt[] debts)
    {
        var cross = debts.Select(d => d.Debtor).Intersect(debts.Select(d => d.Creditor)).ToArray();
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