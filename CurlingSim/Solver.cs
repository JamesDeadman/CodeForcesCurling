using System;
using System.Collections.Generic;
using System.Linq;

namespace CurlingSim
{
    /// <summary>
    /// Generic implementation of curling sim solver, must be extended
    /// </summary>
    public abstract class Solver
    {
        private static double StartingYValue = 10e100d;

        public int DiskRadius { get; private set; }
        public List<Disk> Disks { get; private set; }
        public TimeSpan ExecutionTime { get; private set; }

        public Solver(int radius, List<int> startingXPositions)
        {
            int i = 1;
            DiskRadius = radius;
            Disks = startingXPositions.Select(x => new Disk((i++).ToString(), x, StartingYValue, radius)).ToList();
        }

        public void Execute()
        {
            DateTime startTime = DateTime.Now;
            Solve();
            ExecutionTime = DateTime.Now - startTime;
        }

        protected abstract void Solve();
    }
}
