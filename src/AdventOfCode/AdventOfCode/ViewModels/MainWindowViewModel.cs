using AdventOfCode.Challenges.Day1;
using AdventOfCode.Challenges.Day3;
using AdventOfCode.Challenges.Day4;
using AdventOfCode.Challenges.Day6;
using AdventOfCode.Challenges.IntCodeComputer;
using AdventOfCode.DataReader;
using AdventOfCode.ViewModels.Console;
using AdventOfCode.Views.Inputs;
using MVVMSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AdventOfCode.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DirectoryInfo _inputFolder;
        private IReceiveInput _inputReader = new PopupInput();
        public ObservableCollection<IShowOnConsole> ConsoleMessages { get; set; } = new ObservableCollection<IShowOnConsole>();

        public RelayCommand Day1Command { get; set; }
        public RelayCommand Day2Command { get; set; }
        public RelayCommand Day3Command { get; set; }
        public RelayCommand Day4Command { get; set; }
        public RelayCommand Day5Command { get; set; }
        public RelayCommand Day6Command { get; set; }
        public RelayCommand Day7Command { get; set; }
        public RelayCommand Day8Command { get; set; }
        public RelayCommand Day9Command { get; set; }
        public RelayCommand Day10Command { get; set; }
        public RelayCommand Day11Command { get; set; }
        public RelayCommand Day12Command { get; set; }
        public RelayCommand Day13Command { get; set; }
        public RelayCommand Day14Command { get; set; }
        public RelayCommand Day15Command { get; set; }
        public RelayCommand Day16Command { get; set; }
        public RelayCommand Day17Command { get; set; }
        public RelayCommand Day18Command { get; set; }
        public RelayCommand Day19Command { get; set; }
        public RelayCommand Day20Command { get; set; }
        public RelayCommand Day21Command { get; set; }
        public RelayCommand Day22Command { get; set; }
        public RelayCommand Day23Command { get; set; }
        public RelayCommand Day24Command { get; set; }

        public MainWindowViewModel()
        {
            Day1Command = new RelayCommand(Execute1);
            Day2Command = new RelayCommand(Execute2);
            Day3Command = new RelayCommand(Execute3);
            Day4Command = new RelayCommand(Execute4);
            Day5Command = new RelayCommand(Execute5);
            Day6Command = new RelayCommand(Execute6);
            Day7Command = new RelayCommand(Execute7);
            Day8Command = new RelayCommand(Execute8);
            Day9Command = new RelayCommand(Execute9);
            Day10Command = new RelayCommand(Execute10);
            Day11Command = new RelayCommand(Execute11);
            Day12Command = new RelayCommand(Execute12);
            Day13Command = new RelayCommand(Execute13);
            Day14Command = new RelayCommand(Execute14);
            Day15Command = new RelayCommand(Execute15);
            Day16Command = new RelayCommand(Execute16);
            Day17Command = new RelayCommand(Execute17);
            Day18Command = new RelayCommand(Execute18);
            Day19Command = new RelayCommand(Execute19);
            Day20Command = new RelayCommand(Execute20);
            Day21Command = new RelayCommand(Execute21);
            Day22Command = new RelayCommand(Execute22);
            Day23Command = new RelayCommand(Execute23);
            Day24Command = new RelayCommand(Execute24);

            var assemblyRootFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            _inputFolder = new DirectoryInfo(Path.Combine(assemblyRootFolder.FullName, "InputData"));
        }

        /// <summary>
        /// Function to add txt to our output console.
        /// We will use this as a delegate for all challenges
        /// </summary>
        /// <param name="txt"></param>
        public void WriteToConsole(string txt)
        {
            ConsoleMessages.Add(new ConsoleMessage(txt));
        }

        /// <summary>
        /// Adds an empty line in order to better seperate blocks in the output
        /// </summary>
        public void AddEmptyLine()
        {
            ConsoleMessages.Add(new ConsoleMessage(null));
        }

        /// <summary>
        /// Execution function for Day 1
        /// </summary>
        public void Execute1()
        {
            WriteToConsole("Start execution of Day1");
            var parser = GetInputParser("Day1Input.txt");
            var masses = parser.GetInputData().Select(m => double.Parse(m));

            var fuelCalc = new FuelCalculation();
            var fuel = fuelCalc.CalculateFuel(masses);
            WriteToConsole($"For the given modules {fuel.ToString("N")} fuel is required");

            AddEmptyLine();
        }

        /// <summary>
        /// Execution function for Day 2
        /// </summary>
        public void Execute2()
        {
            WriteToConsole("Start execution of Day2");
            var parser = GetInputParser("Day2Input.txt");
            var originalCode = parser.GetIntCode();
            var code = originalCode.ToList();

            code[1] = 12;
            code[2] = 2;

            var intComputer = new IntCodeComputer(code, WriteToConsole, _inputReader);
            var finalCode = intComputer.Compute();
            WriteToConsole($"After execution the value at position 0 is {finalCode[0].ToString("N")}");

            AddEmptyLine();

            WriteToConsole("For part 2 we need to find the inputs that produce the output 19690720");
            int finalNoun = -1;
            int finalVerb = -1;

            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <=99; verb++)
                {
                    var testCode = originalCode.ToList();
                    testCode[1] = noun;
                    testCode[2] = verb;
                    intComputer = new IntCodeComputer(testCode, WriteToConsole, _inputReader);
                    var exitCode = intComputer.Compute();
                    
                    if(exitCode[0] == 19690720)
                    {
                        finalNoun = noun;
                        finalVerb = verb;
                        //Breaks the outer loop
                        noun = 100;
                        break;
                    }
                }
            }

            WriteToConsole($"Found the correct inputs: Noun = {finalNoun}, Verb = {finalVerb}. 100 * Noun + verb = {100 * finalNoun + finalVerb}");

            AddEmptyLine();
        }

        /// <summary>
        /// Execution function for Day 3
        /// </summary>
        public void Execute3()
        {
            WriteToConsole("Start execution of Day3");
            var parser = GetInputParser("Day3Input.txt");

            var startingPoint = new CoordinatePoint(0, 0);
            var wires = parser.GetInputData().Select(line => new Wire(startingPoint, line.Split(','))).ToList();

            var wire1 = wires[0];
            var wire2 = wires[1];

            var intersections = wire1.GetIntersectionsWith(wire2);
            var board = new GridBoard();
            var closestPoint = board.GetClosestPointTo(intersections, startingPoint);

            WriteToConsole($"The closest intersection point is located at {closestPoint.point} with a distance to the starting point of {closestPoint.distance}");

            AddEmptyLine();

            WriteToConsole($"Now we want to find the closest intersection based on the steps both wires combined have to take to reach it");

            var firstIntersection = wire1.GetFirstIntersectionWith(wire2);

            WriteToConsole($"The first intersection point is located at {firstIntersection.point} with a total number of {firstIntersection.steps} steps");

            AddEmptyLine();
        }

        /// <summary>
        /// Execution function for Day 4
        /// </summary>
        public void Execute4()
        {
            WriteToConsole("Start execution of Day4");
            var parser = GetInputParser("Day4Input.txt");

            var stringRange = parser.GetInputData().First().Split('-');

            var start = int.Parse(stringRange[0]);
            var end = int.Parse(stringRange[1]);

            var range = Enumerable.Range(start, end - start);

            var candidates1 = range.Where(i =>
            {
                return CriteriaFunctions.AdjacentEqual(i.ToString()) &&
                CriteriaFunctions.NeverDecreasing(i.ToString());
            });

            WriteToConsole($"Found {candidates1.Count()} possible passwords, that satisfy the first set of criteria functions");

            AddEmptyLine();

            var candidates2 = range.Where(i =>
            {
                return CriteriaFunctions.AdjacentEqualNoGroup(i.ToString()) &&
                CriteriaFunctions.NeverDecreasing(i.ToString());
            });

            WriteToConsole($"Found {candidates2.Count()} possible passwords, that satisfy the second set of criteria functions");

            AddEmptyLine();
        }

        /// <summary>
        /// Execution function for Day 5
        /// </summary>
        public void Execute5()
        {
            WriteToConsole("Start execution of Day5");
            var parser = GetInputParser("Day5Input.txt");
            var originalCode = parser.GetIntCode();
            var code = originalCode.ToList();

            var intComputer = new IntCodeComputer(code, WriteToConsole, _inputReader);
            var finalCode = intComputer.Compute();
            WriteToConsole($"Diagnostics finished");

            AddEmptyLine();
        }

        /// <summary>
        /// Execution function for Day 6
        /// </summary>
        public void Execute6()
        {
            WriteToConsole("Start execution of Day6");
            var parser = GetInputParser("Day6Input.txt");
            var input = parser.GetInputData();

            var map = new OrbitMap(input);
            var orbits = map.GetNumberOfOrbits();

            WriteToConsole($"The given map has {orbits.ToString("N")} orbits");
            AddEmptyLine();

            WriteToConsole($"Now we want to find the shortest orbital transfer from YOU to SAN");

            var you = map.SpaceObjects.FirstOrDefault(o => o.Id == "YOU");
            var san = map.SpaceObjects.FirstOrDefault(o => o.Id == "SAN");
            var (distance, path) = you.GetTransfersToReach(san);

            WriteToConsole($"The shortest transfer from YOU to SAN takes {distance} transfers with the route {path}");
        }

        /// <summary>
        /// Execution function for Day 7
        /// </summary>
        public void Execute7()
        {

        }

        /// <summary>
        /// Execution function for Day 8
        /// </summary>
        public void Execute8()
        {

        }

        /// <summary>
        /// Execution function for Day 9
        /// </summary>
        public void Execute9()
        {

        }

        /// <summary>
        /// Execution function for Day 10
        /// </summary>
        public void Execute10()
        {

        }

        /// <summary>
        /// Execution function for Day 11
        /// </summary>
        public void Execute11()
        {

        }

        /// <summary>
        /// Execution function for Day 12
        /// </summary>
        public void Execute12()
        {

        }

        /// <summary>
        /// Execution function for Day 13
        /// </summary>
        public void Execute13()
        {

        }

        /// <summary>
        /// Execution function for Day 14
        /// </summary>
        public void Execute14()
        {

        }

        /// <summary>
        /// Execution function for Day 15
        /// </summary>
        public void Execute15()
        {

        }

        /// <summary>
        /// Execution function for Day 16
        /// </summary>
        public void Execute16()
        {

        }

        /// <summary>
        /// Execution function for Day 17
        /// </summary>
        public void Execute17()
        {

        }

        /// <summary>
        /// Execution function for Day 18
        /// </summary>
        public void Execute18()
        {

        }

        /// <summary>
        /// Execution function for Day 19
        /// </summary>
        public void Execute19()
        {

        }

        /// <summary>
        /// Execution function for Day 20
        /// </summary>
        public void Execute20()
        {

        }

        /// <summary>
        /// Execution function for Day 21
        /// </summary>
        public void Execute21()
        {

        }

        /// <summary>
        /// Execution function for Day 22
        /// </summary>
        public void Execute22()
        {

        }

        /// <summary>
        /// Execution function for Day 23
        /// </summary>
        public void Execute23()
        {

        }

        /// <summary>
        /// Execution function for Day 24
        /// </summary>
        public void Execute24()
        {

        }

        /// <summary>
        /// Combines the inputfilename with the iunput file folder path
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <returns></returns>
        private IInputParser GetInputParser(string inputFileName)
        {
            var inputFile = Path.Combine(_inputFolder.FullName, inputFileName);
            return InputParser.GetParser(inputFile, WriteToConsole);
        }

    }
}
