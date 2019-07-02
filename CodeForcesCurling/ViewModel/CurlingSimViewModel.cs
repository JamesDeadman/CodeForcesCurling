using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using CodeForcesCurling.Model;
using CurlingSim;

namespace CodeForcesCurling.ViewModel
{
    public class CurlingSimViewModel : INotifyPropertyChanged
    {
        private readonly char[] ListDelimiter = new char[] { ' ' };

        public event PropertyChangedEventHandler PropertyChanged;

        public SimpleDelegateCommand GenerateRandomCommand { get; }

        public SimpleDelegateCommand SolveCommand { get; }
        public SimpleDelegateCommand CloseCommand { get; }


        public string InputRadius { get; set; } = "2";
        public string InputXLocations { get; set; } = "5 5 6 8 3 12";
        public List<CurlingSimSolverRunner> Solvers { get; } = new List<CurlingSimSolverRunner>();
        public CurlingSimSolverRunner SelectedSolver { get; set; }
        public Solver CompletedSolver { get; set; }
        public string OutputYValues { get; set; }
        public string OutputDuration { get; set; }

        public CurlingSimViewModel()
        {
            GenerateRandomCommand = new SimpleDelegateCommand(() => GenerateRandom());
            SolveCommand = new SimpleDelegateCommand(() => Solve());
            CloseCommand = new SimpleDelegateCommand(() => Close());

            Solvers.Add(new CurlingSimSolverRunner("Discrete Solver", (r, x) => new DiscreteSolver(r, x)));
            Solvers.Add(new CurlingSimSolverRunner("Proximity Solver", (r, x) => new ProximitySolver(r, x)));
            Solvers.Add(new CurlingSimSolverRunner("Brute Solver", (r, x) => new BruteSolver(r, x)));
        }

        /// <summary>
        /// Fill the x location field with a set of randomly generated numbers
        /// </summary>
        public void GenerateRandom()
        {
            Random randomizer = new Random(DateTime.Now.Millisecond);
            InputXLocations = string.Join(' ', Enumerable.Range(0, 100).Select(i => randomizer.Next(2, 100)).ToArray());
            NotifyPropertyChanged(nameof(InputXLocations));
        }

        /// <summary>
        /// Run the selected solver and update the output values
        /// </summary>
        public void Solve()
        {
            if(SelectedSolver != null)
            {
                int inputRadius = int.Parse(InputRadius);
                List<int> xLocations = InputXLocations.Split(ListDelimiter).Where(x => IsValid(x)).Select(x => int.Parse(x)).ToList();

                Solver solver = SelectedSolver.Run(inputRadius, xLocations);

                OutputDuration = solver.ExecutionTime.ToString("s'.'ffff");
                OutputYValues = string.Join(' ', solver.Disks.Select(d => d.YLocation).ToArray());
                CompletedSolver = solver;

                NotifyPropertyChanged(nameof(CompletedSolver));
                NotifyPropertyChanged(nameof(OutputDuration));
                NotifyPropertyChanged(nameof(OutputYValues));
            }
        }

        /// <summary>
        /// Checks if a string can be parsed into a valid x value
        /// </summary>
        /// <param name="x">String containing a number representing an X value</param>
        /// <returns>True if valid</returns>
        private static bool IsValid(string x)
        {
            try
            {
                int value;
                return int.TryParse(x, out value);
            }
            catch(Exception)
            {
                return false;
            }
        }

        public void Close()
        {
            System.Environment.Exit(0);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
