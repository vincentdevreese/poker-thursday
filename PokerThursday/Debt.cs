namespace PokerThursday;

public record Debt(string Debtor, string Creditor, decimal Amount)
{
    public DebtSnapshot ToSnapshot() => new(Debtor, Creditor, Amount);
}