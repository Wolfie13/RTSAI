using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

public class TaskPlanner {

    private Process ProcessPlanner;
    private string working_directory;

    private string PDDLDomainName = "Domain";
    private string PDDLProblemName = "TestGeneration";
    private string SolutionName = "/ffSolution.soln";

    private List<string> solution;    
    //private string task = "";


	// Use this for initialization
	public TaskPlanner () 
    {
        working_directory = Application.dataPath;        
	}

	public void PlannerTick()
	{
		List<Goal> staticGoals = new List<Goal> ();
		staticGoals.Add (new BuildBuildingGoal (BuildingType.Forge));
		CreateProblem (0, staticGoals);
		RunPlanner (0);
		ReadSolution ();
		ProcessSolution (0);

		CreateProblem (1, staticGoals);
		RunPlanner (1);
		ReadSolution ();
		ProcessSolution (1);
	}

	bool flipflop = true;
    void AssignTask(Action action, int TeamID)
    {
        PlayerData team_data = Map.CurrentMap.GetTeamData(TeamID);
        List<Person> people = team_data.GetPeople();

		flipflop = !flipflop;
		if (flipflop) {
			people[0].ToDoList.Add(action);
		} else {
			people[1].ToDoList.Add(action);
		}
        
    }

    void RunPlanner(int TeamID)
    {
        ProcessPlanner = new Process();

        ProcessPlanner.StartInfo.WorkingDirectory = working_directory + "/Planner";        
        ProcessPlanner.StartInfo.FileName = working_directory + "/Planner/metric-ff.exe";
        ProcessPlanner.StartInfo.Arguments = string.Format("-o {0}.pddl -f {1}.pddl", PDDLDomainName, PDDLProblemName + TeamID);
        ProcessPlanner.StartInfo.CreateNoWindow = true;
        ProcessPlanner.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        ProcessPlanner.Start();
        ProcessPlanner.WaitForExit();
    }

	List<string> ReadSolution()
    {
        var result = File.ReadAllLines(working_directory + "/Planner" + SolutionName).Where(s => s.Contains(":"));
        //TO DO: add functionality for reading all actions and assigning tasks to population
        solution = result.ToList();        
        //File.Delete(working_directory + "/Planner" + SolutionName);

        return solution;
    }

    public void ProcessSolution(int TeamID)
    {
		PlayerData team_data = Map.CurrentMap.GetTeamData(TeamID);
        List<Person> people = team_data.GetPeople();
        List<Building> buildings = team_data.GetBuildings();

        Dictionary<BuildingType, bool> availableStructures = new Dictionary<BuildingType, bool>();

        foreach (Building building in buildings)
        {
            availableStructures[building.m_buildingtype] = true;
        }   

        //Train is with School
        //Educate in barracks / without school

        foreach(string task in solution)
        {
            //Resource Actions

            if(task.Contains("CUTTREE"))
            {
                AssignTask(new CutTree(), TeamID);
            }
            if(task.Contains("CUTSTONE"))
            {
                AssignTask(new Quarry(), TeamID);
            }
            if(task.Contains("PRODUCEWOOD"))
            {
                AssignTask(new SawWood(), TeamID);
            }
            if(task.Contains("SIMPLEMINEORE"))
            {
                AssignTask(new Mine(), TeamID);
            }
            if(task.Contains("SIMPLEMINECOAL"))
            {
                AssignTask(new Mine(ResourceType.Coal), TeamID);
            }
            if(task.Contains("PRODUCEIRON"))
            {
                AssignTask(new Smelt(), TeamID);
            }
            if(task.Contains("PRODUCETOOL"))
            {
                
            }

            //Training Actions

            if (task.Contains("TRAINTEACHER"))
            {
                AssignTask(new Educate(Skill.Teacher, null), TeamID);
            }
            if(task.Contains("TRAINLUMBERJACK"))
            {
                AssignTask(new Educate(Skill.Lumberjack, null), TeamID);
            }
            if(task.Contains("TRAINCARPENTER"))
            {
                AssignTask(new Educate(Skill.Carpenter, null), TeamID);
            }
            if(task.Contains("TRAINMINER"))
            {
                AssignTask(new Educate(Skill.Miner, null), TeamID);
            }
            if(task.Contains("TRAINBLACKSMITH"))
            {
                AssignTask(new Educate(Skill.Blacksmith, null), TeamID);
            }
            if(task.Contains("TRAINRIFLEMAN"))
            {                
                AssignTask(new Educate(Skill.Rifleman, null), TeamID);
            }

            //Building Actions

            if(task.Contains("BUILDFORGE"))
            {
                AssignTask(new BuildBuilding(BuildingType.Forge), TeamID);
            }
            if(task.Contains("BUILDQUARRY"))
            {
                AssignTask(new BuildBuilding(BuildingType.Quarry), TeamID);
            }
            if(task.Contains("BUILDSMELTER"))
            {
                AssignTask(new BuildBuilding(BuildingType.Smelter), TeamID);
            }
            if(task.Contains("BUILDTURFHUT"))
            {
                AssignTask(new BuildBuilding(BuildingType.turfHut), TeamID);
            }
            if(task.Contains("BUILDHOUSE"))
            {
                AssignTask(new BuildBuilding(BuildingType.House), TeamID);
            }
            if(task.Contains("BUILDSCHOOL"))
            {
                AssignTask(new BuildBuilding(BuildingType.School), TeamID);
            }
            if(task.Contains("BUILDSAWMILL"))
            {
                AssignTask(new BuildBuilding(BuildingType.Sawmill), TeamID);
            }
            if(task.Contains("BUILDBARRACKS"))
            {
                AssignTask(new BuildBuilding(BuildingType.Barracks), TeamID);
            }

            //Reproduce Actions

            if(task.Contains("REPRODUCETURFHUT"))
            {
                //AssignTask (REPRODUCE, HUT)
            }
            if(task.Contains("REPRODUCEHOUSE"))
            {
                //AssignTask (REPRODUCE HOUSE)
            }
        }      
    }

    //To Do: Pass in Desired goal from executive
    public void CreateProblem(int TeamID, List<Goal> goals)
    {
		PlayerData team_data = Map.CurrentMap.GetTeamData(TeamID);

        List<Person> people = team_data.GetPeople();
        List<Building> buildings = team_data.GetBuildings();

        //Write Start Problem - define domain
        List<string> lines = new List<string>();

        lines.Add("(define (problem team_problem)");
        lines.Add("(:domain ai_game)");

        //Add Lines for (:objects)
        lines.Add("(:objects");

        foreach (Person person in people)
        {
            lines.Add(person.name + " - person");
            //New place
        }

        //Add lines for finding new resources
        lines.Add("oreresource - resource");
        lines.Add("coalresource - resource");
        lines.Add(")");

        //Add Lines for (:init)
        lines.Add("(:init");

        {
            Dictionary<BuildingType, bool> availableStructures = new Dictionary<BuildingType, bool>();

            foreach (Building building in buildings)
            {
                availableStructures[building.m_buildingtype] = true;
            }

            foreach (var structure in availableStructures)
            {
                string problem_string = "(has-" + structure.Key.ToString().ToLower() + ")";
                lines.Add(problem_string);
            }
        }

		
		{
			Dictionary<Skill, bool> availableSkills = new Dictionary<Skill, bool>();
			
			foreach (Person p in people)
			{
				foreach (Skill s in p.Skills)
				{
					availableSkills.Add(s, true);
				}
			}
			
			foreach (var skill in availableSkills)
			{
				string problem_string = "(has-" + skill.Key.ToString().ToLower() + ")";
				lines.Add(problem_string);
			}
		}

        lines.Add("(ore_resource oreresource)");
        lines.Add("(coal_resource coalresource)");

        Dictionary<ResourceType, int> availableResource = team_data.Resources;
        int wood = availableResource[ResourceType.Wood];
        int iron = availableResource[ResourceType.Iron];
        int timber = availableResource[ResourceType.Timber];
        int ore = availableResource[ResourceType.Ore];
        int coal = availableResource[ResourceType.Coal];
        int stone = availableResource[ResourceType.Stone];
        int rifles = availableResource[ResourceType.Rifles];
     
        //Add Initial Amounts Data
        lines.Add("(= (time) 0)");
        lines.Add("(= (wood) " + wood + ")"); 
        lines.Add("(= (iron) " + iron + ")");
        lines.Add("(= (timber) " + timber + ")");
        lines.Add("(= (stored-ore) " + ore + ")");
        lines.Add("(= (stored-coal) " + coal + ")");
        lines.Add("(= (rifles) " + rifles + ")");
        lines.Add("(= (riflemen) 0)");
        lines.Add("(= (stone) 0)");
        lines.Add("(= (population) " + people.Count + ")");
        lines.Add("(= (min_resource) 0)");
        lines.Add("(= (ore oreresource) 5)");
        lines.Add("(= (coal coalresource) 5)");

        lines.Add(")");

        //Add Lines for (:goal)
        lines.Add("(:goal");
        lines.Add("(and");

        //Add Goals
        foreach (Goal g in goals)
        {
            lines.Add(g.WriteGoal());
        }

        lines.Add(")");
        lines.Add(")");

        //Write Problem File
        lines.Add(")");
        System.IO.File.WriteAllLines(working_directory + "/Planner/TestGeneration" + TeamID + ".pddl", lines.ToArray());
    }
}
