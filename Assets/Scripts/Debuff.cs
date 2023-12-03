[System.Serializable]
public class Debuff
{
    public string debuffName;
    public int duration; 

    public Debuff(string name, int turns)
    {
        debuffName = name;
        duration = turns;
    }
}
