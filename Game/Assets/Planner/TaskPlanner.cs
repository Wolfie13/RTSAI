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
    private string PDDLProblemName = "TeamProblem";
    private string SolutionName = "/ffSolution.soln";

    private string solution = "";
    
    private Map map;

	// Use this for initialization
	void Start () 
    {
        map = GetComponent<Map>();
        working_directory = Application.dataPath;
        UnityEngine.Debug.Log(working_directory);
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            RunPlanner(1);
            ReadSolution();
            UnityEngine.Debug.Log(solution);
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

    string ReadSolution()
    {
        var result = File.ReadAllLines(working_directory + "/Planner" + SolutionName).Where(s => s.Contains(":"));

        //TO DO: add functionality for reading all actions and assigning tasks to population
        solution = result.ToList()[1].ToString();       
        

        File.Delete(working_directory + "/Planner" + SolutionName);

        return solution;
    }
}
