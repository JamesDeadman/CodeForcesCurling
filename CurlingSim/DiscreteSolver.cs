using System;
using System.Collections.Generic;
using System.Linq;

namespace CurlingSim
{
    /// <summary>
    /// This solver works by maintaining a depth map along the X axis.  This depth represents the minimum Y centerline that a disk
    /// may fall to at any given X.  To preserve memory, this map is maintained as a dictionary where any value absent from the dictionary
    /// is considered 0 and values are added only as-required.  This method requires that all X values are integers as stated in the problem.
    /// </summary>
    public class DiscreteSolver : Solver
    {
        /// <summary>A discrete depth field; the projected maximum center point Y center point value for each X</summary>
        public Dictionary<int, double> Field { get; private set; }

        /// <summary>Pre-calculated maximum</summary>
        public double[] DiskProfile { get; private set; }


        /// <summary>The rightmost boundary of the descrete field</summary>
        public int MaxX { get; private set; }

        public DiscreteSolver(int radius, List<int> startingXPositions) : base(radius, startingXPositions)
        {
            int maxX = startingXPositions.Max();
            Field = new Dictionary<int, double>();

            DiskProfile = new double[radius * 2 + 1];
            int rr4 = radius * radius * 4;

            for (int x = 0; x <= radius * 2; x++)
            {
                DiskProfile[x] = Math.Sqrt(rr4 - x * x) - radius;
            }
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
            disk.YLocation = GetFieldHeight(disk.XLocation) + DiskRadius;

            SetFieldHeight(disk.XLocation, disk.YLocation + DiskProfile[0]);

            for (int x = 1; x <= disk.Radius * 2; x++)
            {
                SetFieldHeight(disk.XLocation - x, disk.YLocation + DiskProfile[x]);
                SetFieldHeight(disk.XLocation + x, disk.YLocation + DiskProfile[x]);
            }
        }

        private double GetFieldHeight(int x)
        {
            return Field.ContainsKey(x) ? Field[x] : 0.0f;
        }

        private void SetFieldHeight(int x, double y)
        {
            if(Field.ContainsKey(x))
            {
                Field[x] = Math.Max(Field[x], y);
            }
            else
            {
                Field.Add(x, y);
            }
        }
    }
}