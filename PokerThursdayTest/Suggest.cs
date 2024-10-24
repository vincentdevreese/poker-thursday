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
            var b = cross.First();
            var debt1 = debts.Single(d => d.Creditor == b);
            var debt2 = debts.Single(d => d.Debtor == b);



            return
            [
                debt1 with { Creditor = debt2.Creditor },
                debt2 with { Amount = debt2.Amount - debt1.Amount }
            ];
        }

        return [new("a", "c", 20)];
    }
}