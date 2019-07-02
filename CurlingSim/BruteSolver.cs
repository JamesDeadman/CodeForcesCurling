using System;
using System.Collections.Generic;
using System.Linq; 

namespace CurlingSim
{
    /// <summary>
    /// This solver works by checking each previously placed disk; brute force method.
    /// </summary>
    public class BruteSolver : Solver
    {
        public List<Disk> PlacedDisks { get; private set; } = new List<Disk>();

        public BruteSolver(int radius, List<int> startingXPositions) : base(radius, startingXPositions)
        {
        }

        protected override void Solve()
        {
            foreach (Disk disk in Disks)
            {
                PlaceDisk(disk);
            }
        }

        private void PlaceDisk(Disk disk)
        {
            double maxY = DiskRadius;
            double rr4 = DiskRadius * DiskRadius * 4;

            foreach (Disk nearbyDisk in PlacedDisks)
            {
                double x = nearbyDisk.XLocation - disk.XLocation;
                double newY = Math.Sqrt(rr4 - x * x) + nearbyDisk.YLocation;
                if(!double.IsNaN(newY))
                {
                    maxY = Math.Max(maxY, newY);
                }
            }

            disk.YLocation = maxY;
            PlacedDisks.Add(disk);
        }
    }
}
