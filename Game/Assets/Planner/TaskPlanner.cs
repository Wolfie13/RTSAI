using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

public class TaskPlanner : MonoBehaviour {

    private Process ProcessPlanner;
    private string working_directory;

    private string PDDLDomainName = "Domain";
    private string PDDLProblemName = "TestGeneration";
    private string SolutionName = "/ffPSolution.soln";

    private List<string> solution;    
    //private string task = "";

    
    private Map map;


	// Use this for initialization
	void Start () 
    {
        map = GetComponent<Map>();
        working_directory = Application.dataPath;        
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            RunPlanner(1);
            ReadSolution();
            foreach (string task in solution)
            {
                UnityEngine.Debug.Log(task);
            }
            ProcessSolution(1);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {           
            List<Goal> goals = new List<Goal>();           
            CreateProblem(0, goals);
        }
    }

    void AssignTask(Action action, int TeamID)
    {
        PlayerData team_data = map.GetTeamData(TeamID);
        List<Person> people = team_data.GetPeople();

        foreach (Person p in people)
        {
            if (p.ToDoList.Count() == 0)
            {
                p.ToDoList.Add(action);
            }
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
        File.Delete(working_directory + "/Planner" + SolutionName);

        return solution;
    }

    public void ProcessSolution(int TeamID)
    {
        PlayerData team_data = map.GetTeamData(TeamID);
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
            if(task.Contains("PRODUCEIRON"))
            {
                AssignTask(new Smelt(), TeamID);
            }
            if(task.Contains("PRODUCETOOL"))
            {
                //BLACKMSITH ACTION
            }
            if (task.Contains("TRAINTEACHER"))
            {
                AssignTask(new Educate(), TeamID);
            }
            if(task.Contains("TRAINLUMBERJACK"))
            {
                if(availableStructures.ContainsKey(BuildingType.School))
                {
                    AssignTask(new Train(), TeamID);
                }
                else{AssignTask(new Educate(), TeamID);}
            }
            if(task.Contains("TRAINCARPENTER"))
            {
                if (availableStructures.ContainsKey(BuildingType.School))
                {
                    AssignTask(new Train(), TeamID);
                }
                else {AssignTask(new Educate(), TeamID);}
            }
            if(task.Contains("TRAINMINER"))
            {
                if (availableStructures.ContainsKey(BuildingType.School))
                {
                    AssignTask(new Train(), TeamID);
                }
                else {AssignTask(new Educate(), TeamID);}
            }
            if(task.Contains("TRAINBLACKSMITH"))
            {
                if (availableStructures.ContainsKey(BuildingType.School))
                {
                    AssignTask(new Train(), TeamID);
                }
                else { AssignTask(new Educate(), TeamID); }
            }
            if(task.Contains("TRAINRIFLEMAN"))
            {
                //Train Rifleman
            }
        }      
    }

    //To Do: Pass in Desired goal from executive
    public void CreateProblem(int TeamID, List<Goal> goals)
    {
        PlayerData team_data = map.GetTeamData(TeamID);

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

        lines.Add("(ore_resource oreresource)");
        lines.Add("(coal_resource coalresource)");
     
        //Add Initial Amounts Data
        lines.Add("(= (time) 0)");
        lines.Add("(= (wood) 0)");
        lines.Add("(= (iron) 0)");
        lines.Add("(= (timber) 0)");
        lines.Add("(= (stored-ore) 0)");
        lines.Add("(= (stored-coal) 0)");
        lines.Add("(= (rifles) 0)");
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
