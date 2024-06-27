namespace PokerThursdayTest;

public record Debt(string Debtor, string Creditor, decimal Amount)
{
    public DebtSnapshot ToSnapshot() => new(this.Debtor, this.Creditor, this.Amount);
}