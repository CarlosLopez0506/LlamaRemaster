[System.Serializable]
public class Debuff
{
    public string DebuffName { get; private set; }
    public int Cost
    {
        get { return _cost; }
        set { _cost = value; }
    }
    public bool IsActive
    {
        get { return active; }
        private set { active = value; }
    }

    private int _cost;
    private bool active;

    public Debuff(string name, int price)
    {
        DebuffName = name;
        Cost = price;
        IsActive = false;
    }

    public void ActivateDebuff()
    {
        IsActive = true;
    }

    public void DeactivateDebuff()
    {
        IsActive = false;
    }
}