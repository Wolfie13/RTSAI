using System;


class CollectResourceGoal : Goal
{
    ResourceType rt;
    int amount;

    public CollectResourceGoal(ResourceType rt, int amount)
    {
        this.rt = rt;
        this.amount = amount;
    }

    public override string WriteGoal()
    {
        return "(>= (" + rt.ToString().ToLower() + ") " + amount.ToString() + ")";
    }
}

