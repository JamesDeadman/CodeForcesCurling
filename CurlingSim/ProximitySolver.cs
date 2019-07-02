using System;
using System.Collections.Generic;
using System.Linq; 

namespace CurlingSim
{
    /// <summary>
    /// This solver works by checking each previously placed disk within 2 R in the X axis; similar to the brute force method but includes a sorted dictionary to reduce comparrisons
    /// testing shows that this is actually a performance loss with small data sets
    /// </summary>

    public class ProximitySolver : Solver
    {
        public SortedDictionary<int, List<Disk>> PlacedDisksByX { get; private set; } = new SortedDictionary<int, List<Disk>>();

        public ProximitySolver(int radius, List<int> startingXPositions) : base(radius, startingXPositions)
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

            foreach (Disk nearbyDisk in GetNearbyTopDisks(disk.XLocation))
            {
                double x = nearbyDisk.XLocation - disk.XLocation;
                double newY = Math.Sqrt(rr4 - x * x) + nearbyDisk.YLocation;
                maxY = Math.Max(maxY, newY);
            }

            disk.YLocation = maxY;

            if(!PlacedDisksByX.ContainsKey(disk.XLocation))
            {
                PlacedDisksByX.Add(disk.XLocation, new List<Disk>());
            }
            PlacedDisksByX[disk.XLocation].Add(disk);
        }

        public IEnumerable<Disk> GetNearbyTopDisks(int x)
        {
            int r2 = DiskRadius * 2;
            return PlacedDisksByX.Where(d => Math.Abs(x - d.Key) <= r2).Select(d => d.Value.Last());
        }
    }
}
