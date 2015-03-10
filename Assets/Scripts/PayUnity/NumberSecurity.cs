
public static class NumberSecurity
{
    static int randomNumSecurity;

    public static int RandomNumSecurity
    {
        get { return NumberSecurity.randomNumSecurity; }
    }

    public static NumberSecurity()
    {
        randomNumSecurity = UnityEngine.Random.Range(0, 1000);
    }
}
