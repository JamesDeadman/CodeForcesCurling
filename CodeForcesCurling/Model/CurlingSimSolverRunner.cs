using System;
using CurlingSim;
using System.Collections.Generic;

namespace CodeForcesCurling.Model
{
    /// <summary>
    /// A wrapper to allow the UI to select and run a solver
    /// </summary>
    public class CurlingSimSolverRunner
    {
        public string Description { get; private set; }
        public Func<int, List<int>, Solver> SolverFactory { get; private set; }

        public CurlingSimSolverRunner(string description, Func<int, List<int>, Solver> solverFactory)
        {
            Description = description;
            SolverFactory = solverFactory;
        }

        public Solver Run(int Radius, List<int> StartingX)
        {
            Solver solver = SolverFactory.Invoke(Radius, StartingX);
            solver.Execute();
            return solver;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
