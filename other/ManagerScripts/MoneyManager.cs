public class MoneyManager
{
    public static int gems = 0;
    string username;
    
    /*public static void AddGems(int gemCount)
    {
        gems += gemCount;
    }*/

    public static void SetGems(int gemCount)
    {
        gems = gemCount;
    }

    public static void LoadGems()
    {
        /*SaveMoney money = SaveSystem.LoadPlayer(username);

        if (money != null)
        {
            gems = money.gems;
        }
        else
        {
            gems = 0;
        }*/
    }
}
