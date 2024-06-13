using FluentAssertions;

namespace PokerThursdayTest;

// TODO redondance test, x dettes?
public class RecordDebtTests
{
    [Fact]
    public void A()
    {
        string user1 = "user1";
        string user2 = "user2";

        var actual = this.Act(user1, user2, 30.0m);

        actual.Should().Equal(new Debt(user1, user2, 30.0m));
    }

    [Fact]
    public void AddDebt()
    {
        string user1 = "vincent";
        string user2 = "dimitri";

        var actual = this.Act(user1, user2, 120.0m);

        actual.Should().Equal(new Debt(user1, user2, 120.0m));
    }

    [Fact]
    public void AddAnotherDebt()
    {
        // dettes exsistante
        var existingDebts = new List<Debt>();
        existingDebts.Add(new Debt("des trucs", "bidon", 50m));

        string user1 = "vincent";
        string user2 = "dimitri";
        var actual = this.Act(existingDebts, user1, user2, 120.0m);

        actual.Should().Equal(
            new Debt("des trucs", "bidon", 50m),
            new Debt(user1, user2, 120.0m)
        );
    }


    [Fact]
    public void AddAnotherUserDebtOnExistingUser()
    {
        // dettes exsistante

        var existingDebts = new List<Debt>();
        existingDebts.Add(new("vincent", "dimitri", 30m));

        string user1 = "vincent";
        string user2 = "dimitri";
        var actual = this.Act(existingDebts, user1, user2, 120.0m);

        actual.Should().Equal(new Debt("vincent", "dimitri", 150m));
    }

    private Debt[] Act(string user1, string user2, decimal debt)
    {
        return this.Act(new List<Debt>(), user1, user2, debt);
    }

    private Debt[] Act(List<Debt> existingDebts, string user1, string user2, decimal debt)
    {
        if (!existingDebts.Any(x => x.Item1 == user1 && x.Item2 == user2))
        {
            return existingDebts.Concat([new(user1, user2, debt)]).ToArray();
        }

        decimal totalAmount = debt;

        List<Debt> toto = [];
        foreach (var item in existingDebts)
        {
            if (item.Item1 != user1 && item.Item2 != user2)
            {
                toto.Add(item);
            }
            else
            {
                totalAmount += item.Item3;
            }
        }

        return toto.Concat([new(user1, user2, totalAmount)]).ToArray();
    }


}

internal record Debt(string Item1, string Item2, decimal Item3)
{

}