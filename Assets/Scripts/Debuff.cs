[System.Serializable]
public class Debuff
{
    public string debuffName;
    public int duration; // Number of turns the debuff lasts

    public Debuff(string name, int turns)
    {
        debuffName = name;
        duration = turns;
    }
}
