using System;

 class BuildBuildingGoal : Goal
{
     BuildingType bt;

     public BuildBuildingGoal(BuildingType bt)
     {
         this.bt = bt;
     }

     public override string WriteGoal()
     {
         return "(has-" + bt.ToString().ToLower() + ")";
     }
}

