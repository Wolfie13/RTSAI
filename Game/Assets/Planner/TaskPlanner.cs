using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

public class TaskPlanner : MonoBehaviour {

    private Process ProcessPlanner;
    private string working_directory = Application.dataPath;

    private string PDDLDomainName = @"Domain";
    private string PDDLProblemName = @"Problem";
    private string SolutionName = @"/ffSolution.soln";

    private string solution = "";

	// Use this for initialization
	void Start () 
    {
	    
	}

    void RunPlanner()
    {
        ProcessPlanner = new Process();

        ProcessPlanner.StartInfo.WorkingDirectory = working_directory + @"/Planner";
        ProcessPlanner.StartInfo.FileName = working_directory + @"/metric-ff.exe";
        ProcessPlanner.StartInfo.Arguments = string.Format("-o {0}.pddl -f {1}.pddl", PDDLDomainName, PDDLProblemName);
        ProcessPlanner.StartInfo.CreateNoWindow = true;
        ProcessPlanner.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        ProcessPlanner.Start();
        ProcessPlanner.WaitForExit();
    }

    string ReadSolution()
    {
        var result = File.ReadAllLines(SolutionName).Where(s => s.Contains(":"));

        solution = result.ToList()[0].ToString();

        File.Delete(SolutionName);

        return solution;
    }
}
