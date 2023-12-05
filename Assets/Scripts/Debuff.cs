using System;
using System.Collections.Generic;

[System.Serializable]
public class Debuff
{
    public Dictionary<string, bool> debuffData;
    private int cost;

    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public Debuff(string debuffName, int cost)
    {
        this.cost = cost;
        debuffData = new Dictionary<string, bool>();
        debuffData[debuffName] = false;
    }

    public void ToggleDebuff(string debuffName)
    {
        if (debuffData.ContainsKey(debuffName))
        {
            debuffData[debuffName] = !debuffData[debuffName];
        }
        else
        {
            Console.WriteLine($"Debuff '{debuffName}' not found.");
            // You can handle this case as per your requirement, e.g., throw an exception.
        }
    }
}