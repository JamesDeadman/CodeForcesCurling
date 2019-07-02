using CurlingSim;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CurlingSimTests
{
    [TestClass]
    public class DescreteSolverTests
    {
        [TestMethod]
        public void DescreteSolver_CanSolve()
        {
            var epsilon = 10e-6;

            var startingPositions = new List<int>() { 5, 5, 6, 8, 3, 12 };
            var expectedResult = new List<double>() { 2, 6.0, 9.87298334621, 13.3370849613, 12.5187346573, 13.3370849613 };
            var solver = new DiscreteSolver(2, startingPositions);
            var maxTime = new TimeSpan(0, 0, 2);

            solver.Execute();

            Assert.AreEqual(expectedResult.Count, solver.Disks.Count, epsilon);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                Console.WriteLine($"Expected Value: {expectedResult[i]}, Found Value: {solver.Disks[i].YLocation}");
                Assert.AreEqual(expectedResult[i], solver.Disks[i].YLocation, epsilon);
            }

            Assert.IsTrue(solver.ExecutionTime <= maxTime, "Solution took longer than the allowed time.");
        }
    }
}
