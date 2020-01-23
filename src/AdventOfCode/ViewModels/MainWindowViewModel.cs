using AdventOfCode.Challenges.Day1;
using AdventOfCode.Challenges.Day10;
using AdventOfCode.Challenges.Day11;
using AdventOfCode.Challenges.Day12;
using AdventOfCode.Challenges.Day13;
using AdventOfCode.Challenges.Day14;
using AdventOfCode.Challenges.Day15;
using AdventOfCode.Challenges.Day16;
using AdventOfCode.Challenges.Day17;
using AdventOfCode.Challenges.Day3;
using AdventOfCode.Challenges.Day4;
using AdventOfCode.Challenges.Day6;
using AdventOfCode.Challenges.Day8;
using AdventOfCode.Challenges.IntCodeComputer;
using AdventOfCode.DataReader;
using AdventOfCode.ViewModels.Console;
using AdventOfCode.Views;
using AdventOfCode.Views.Inputs;
using Extension.Mathematics.Combinatorics;
using Extension.Mathematics.VectorSpace;
using Extension.Wpf.Dialogs;
using Extension.Wpf.MVVM;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DirectoryInfo _inputFolder;
        private IIntCodeComputerOutput _consoleOutput;
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

        public MainWindowViewModel(ILogger<MainWindowViewModel> logger, IDialogService dialogs) : base(logger, dialogs)
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

            _consoleOutput = new DelegateOutput((long i) => WriteToConsole(i.ToString()));
        }

        /// <summary>
        /// Function to add txt to our output console.
        /// We will use this as a delegate for all challenges
        /// </summary>
        /// <param name="txt"></param>
        public void WriteToConsole(string txt)
        {
            RunInUiThread(() => ConsoleMessages.Add(new ConsoleMessage(txt)));
        }

        /// <summary>
        /// Adds an empty line in order to better seperate blocks in the output
        /// </summary>
        public void AddEmptyLine()
        {
            RunInUiThread(() => ConsoleMessages.Add(new ConsoleMessage(null)));
        }

        /// <summary>
        /// Execution function for Day 1
        /// </summary>
        public void Execute1()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day1");
                var parser = GetInputParser("Day1Input.txt");
                var masses = parser.GetInputData().Select(m => double.Parse(m));

                var fuelCalc = new FuelCalculation();
                var fuel = fuelCalc.CalculateFuel(masses);
                WriteToConsole($"For the given modules {fuel.ToString("N")} fuel is required");

                AddEmptyLine();
            });
        }

        /// <summary>
        /// Execution function for Day 2
        /// </summary>
        public void Execute2()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day2");
                var parser = GetInputParser("Day2Input.txt");
                var originalCode = parser.GetIntCode();
                var code = originalCode.ToList();

                code[1] = 12;
                code[2] = 2;

                var input = new PopupInput("No input needed");
                var intComputer = new IntCodeComputerInstance(code, input, _consoleOutput);
                intComputer.Compute();
                WriteToConsole($"After execution the value at position 0 is {intComputer.Code[0].ToString("N")}");

                AddEmptyLine();

                WriteToConsole("For part 2 we need to find the inputs that produce the output 19690720");
                int finalNoun = -1;
                int finalVerb = -1;

                for (int noun = 0; noun <= 99; noun++)
                {
                    for (int verb = 0; verb <= 99; verb++)
                    {
                        var testCode = originalCode.ToList();
                        testCode[1] = noun;
                        testCode[2] = verb;
                        intComputer = new IntCodeComputerInstance(testCode, input, _consoleOutput);
                        intComputer.Compute();

                        if (intComputer.Code[0] == 19690720)
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
            });
        }

        /// <summary>
        /// Execution function for Day 3
        /// </summary>
        public void Execute3()
        {
            UserActionAsync(() =>
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
            });
        }

        /// <summary>
        /// Execution function for Day 4
        /// </summary>
        public void Execute4()
        {
            UserActionAsync(() =>
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
            });
        }

        /// <summary>
        /// Execution function for Day 5
        /// </summary>
        public void Execute5()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day5");
                var parser = GetInputParser("Day5Input.txt");
                var originalCode = parser.GetIntCode();
                var code = originalCode.ToList();

                var input = new PopupInput("Started Diagnostics. Inputs:\n1 - air conditioner unit\n5 - thermal radiator controller");
                var intComputer = new IntCodeComputerInstance(code, input, _consoleOutput);
                intComputer.ContinueAfterOutput = true;
                intComputer.Compute();

                WriteToConsole($"Diagnostics finished");

                AddEmptyLine();
            });
        }

        /// <summary>
        /// Execution function for Day 6
        /// </summary>
        public void Execute6()
        {
            UserActionAsync(() =>
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
            });
        }

        /// <summary>
        /// Execution function for Day 7
        /// </summary>
        public void Execute7()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day7");
                var parser = GetInputParser("Day7Input.txt");
                var code = parser.GetIntCode();

                var permu = new PermutationNoRepetition(5, 5);
                var combinations = permu.Get(new List<int> { 0, 1, 2, 3, 4 }).ToList();
                var signalOutputs = combinations.Select(phaseSettings =>
                {
                    var ampli = new AmplificationCircuitConfig(code.ToList(), phaseSettings);
                    return ampli.ConfigureAmplifiers(0);
                }).ToList();

                var maxSignal = signalOutputs.Max();
                var index = signalOutputs.IndexOf(maxSignal);
                var phaseSettings = string.Join(", ", combinations[index]);

                WriteToConsole($"The best phase setting is {phaseSettings} with a signal output of {maxSignal.ToString("N")}");
                AddEmptyLine();

                WriteToConsole("Now we want to find the best pahse setting for a feedback loop configuration");
                combinations = permu.Get(new List<int> { 5, 6, 7, 8, 9 }).ToList();
                signalOutputs = combinations.Select(phaseSettings =>
                {
                    var ampli = new AmplificationCircuitConfig(code.ToList(), phaseSettings);
                    return ampli.ConfigureAmplifiersFeedBackLoop(0);
                }).ToList();

                maxSignal = signalOutputs.Max();
                index = signalOutputs.IndexOf(maxSignal);
                phaseSettings = string.Join(", ", combinations[index]);

                WriteToConsole($"The best phase setting for a feedback loop configuration is {phaseSettings} with a signal output of {maxSignal.ToString("N")}");
                AddEmptyLine();
            });
        }

        /// <summary>
        /// Execution function for Day 8
        /// </summary>
        public void Execute8()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day8");
                var parser = GetInputParser("Day8Input.txt");
                var data = parser.GetInputData();
                var encodedImage = data.ToList()[0].ToCharArray().Select(c => int.Parse(c.ToString())).ToList();

                var decoder = new SpaceImageFormatDecoder(25, 6, encodedImage);
                decoder.Decode();

                var zeroDigitsPerLayer = decoder.LayeredImage.Select(layer => layer.SelectMany(row => row.Where(d => d == 0)).Count()).ToList();
                var fewestZeroDigits = zeroDigitsPerLayer.Min();
                var index = zeroDigitsPerLayer.IndexOf(fewestZeroDigits);
                var targetLayer = decoder.LayeredImage[index];

                var ones = targetLayer.SelectMany(row => row.Where(d => d == 1)).Count();
                var twos = targetLayer.SelectMany(row => row.Where(d => d == 2)).Count();
                var checkSum = ones * twos;

                WriteToConsole($"The layer with the least amount of zeros is layer {index + 1} and the checksum is {checkSum.ToString("N")}");

                AddEmptyLine();

                WriteToConsole($"Now we print the decoded image");
                AddEmptyLine();

                RunInUiThread(() =>
                {
                    var dialog = new ImageDisplay(decoder.DecodedImage);
                    dialog.Show();
                });
            });
        }

        /// <summary>
        /// Execution function for Day 9
        /// </summary>
        public void Execute9()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day9");
                var parser = GetInputParser("Day9Input.txt");
                var originalCode = parser.GetIntCode();
                var code = originalCode.ToList();

                var input = new PopupInput("Start BOOST. Inputs:\n1 - Test Mode\n2 - Sensor Boost Mode");
                var intComputer = new IntCodeComputerInstance(code, input, _consoleOutput);
                intComputer.Compute();

                WriteToConsole($"Finished BOOST");

                AddEmptyLine();
            });
        }

        /// <summary>
        /// Execution function for Day 10
        /// </summary>
        public void Execute10()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day10");
                var parser = GetInputParser("Day10Input.txt");
                var input = parser.GetInputData();
                var map = input.Select(s => s.ToCharArray().ToList()).ToList();

                var system = new AsteroidsMonitoringSystem(map);
                var (position, score) = system.GetBestPosition();

                WriteToConsole($"The best position for the monitoring system is ({position.X}, {position.Y}) with a score of {score}");

                var sequence = system.GetVaporizationSequence(position);
                var destroy = sequence[199];

                WriteToConsole($"The asteroid which is destroy at position 200 is at ({destroy.X}, {destroy.Y})");
            });
        }

        /// <summary>
        /// Execution function for Day 11
        /// </summary>
        public void Execute11()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day11");
                var parser = GetInputParser("Day11Input.txt");
                var originalCode = parser.GetIntCode();
                var code = originalCode.ToList();

                var hullPaintingRobot = new HullPaintingRobot(code);
                hullPaintingRobot.Paint(0);

                WriteToConsole($"The hull paining robot has painted {hullPaintingRobot.PaintedPanels.Count} at least once");

                WriteToConsole($"Now we start with a white panel instead of a black one.");

                hullPaintingRobot = new HullPaintingRobot(code);
                hullPaintingRobot.Paint(1);

                var painting = hullPaintingRobot.GetPainting();

                RunInUiThread(() =>
                {
                    var dialog = new ImageDisplay(painting);
                    dialog.Show();
                });
            });
        }

        /// <summary>
        /// Execution function for Day 12
        /// </summary>
        public void Execute12()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day12");
                var IO = new IntVector(-3, 15, -11);
                var Europa = new IntVector(3, 13, -19);
                var Ganymede = new IntVector(-13, 18, -2);
                var Callisto = new IntVector(6, 0, -1);

                var system = new MoonMonitoringSystem(new List<IntVector>
            {
                IO,
                Europa,
                Ganymede,
                Callisto,
            });

                system.Simulate(1000);
                var energy = system.GetTotalEnegry();

                WriteToConsole($"The total energy in the system after 1000 timesteps is {energy}");

                system = new MoonMonitoringSystem(new List<IntVector>
            {
                IO,
                Europa,
                Ganymede,
                Callisto,
            });

                var count = system.GetRepetitionCount();

                WriteToConsole($"The moons return to their position after {count} time steps");
            });
        }

        /// <summary>
        /// Execution function for Day 13
        /// </summary>
        public void Execute13()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day13");
                var parser = GetInputParser("Day13Input.txt");
                var originalCode = parser.GetIntCode();
                var code = originalCode.ToList();

                var game = new ArcadeGame(code);
                game.Start();
                var blocks = game.Tiles.Where(t => t.Id == TileId.Block).Count();

                WriteToConsole($"The number of blocks in the game is {blocks}");

                game = new ArcadeGame(code);
                game.PlayFree();

                WriteToConsole($"The final score is {game.Score}");
            });
        }

        /// <summary>
        /// Execution function for Day 14
        /// </summary>
        public void Execute14()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day14");
                var parser = GetInputParser("Day14Input.txt");
                var reactions = parser.GetInputData();

                var factory = new NanoFactory(reactions);
                var raw = factory.GetRawMaterialFor(new ReactionTerm("FUEL", 1));
                var ore = raw.FirstOrDefault(e => e.Element == "ORE");

                WriteToConsole($"In order to produce one unit of FUEL {ore.Quantity} units ORE are needed");

                var maxProduction = factory.GetProductionCapacity("FUEL", new ReactionTerm("ORE", 1000000000000));

                WriteToConsole($"With 1000000000000 of ORE {maxProduction} units FUEL might be produced");
            });
        }

        /// <summary>
        /// Execution function for Day 15
        /// </summary>
        public void Execute15()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day15");
                var parser = GetInputParser("Day15Input.txt");
                var originalCode = parser.GetIntCode();
                var code = originalCode.ToList();

                var droid = new RepairDroid(code);
                droid.Explore();

                var route = droid.GetShortestRoute(new IntVector(0, 0), droid.OxygenSystem);

                var map = droid.GetRouteMap(route);

                RunInUiThread(() =>
                {
                    var dialog = new ImageDisplay(map);
                    dialog.Show();
                });

                WriteToConsole($"The droid needed {route.Count - 1} movement commands to locate the oxygen system");

                var time = droid.GetOxygenSpreadingTime(droid.OxygenSystem);

                WriteToConsole($"It takes {time} minutes until the area is filled with oxygen");
            });
        }

        /// <summary>
        /// Execution function for Day 16
        /// </summary>
        public void Execute16()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day16");
                var parser = GetInputParser("Day16Input.txt");
                var data = parser.GetInputData();

                var fft = new FFT(data.First());
                fft.Run(100);

                var first8 = fft.Signal.Substring(0, 8);

                WriteToConsole($"The first 8 digits after 100 iteration are {first8}");

                fft = new FFT(data.First());
                fft.RepeatSignal(10_000);
                fft.RunForMessage(100);

                WriteToConsole($"The final message is {fft.Message}");
            });
        }

        /// <summary>
        /// Execution function for Day 17
        /// </summary>
        public void Execute17()
        {
            UserActionAsync(() =>
            {
                WriteToConsole("Start execution of Day17");
                var parser = GetInputParser("Day17Input.txt");
                var originalCode = parser.GetIntCode();
                var code = originalCode.ToList();

                var ascii = new ASCII(code);
                ascii.Scan();
                var checkSum = ascii.GetAlignmentParameters().Sum();

                var map = ascii.GetScaffoldMap();

                RunInUiThread(() =>
                {
                    var dialog = new ImageDisplay(map);
                    dialog.SetText(ascii.VacuumRobot.X, ascii.VacuumRobot.Y, ascii.VacuumRobotStatus.ToString());
                    dialog.Show();
                });

                WriteToConsole($"The sum of the alignment parameters is {checkSum}");

                var main = new List<char> { 'C', ',', 'A', ',', 'C', ',', 'A', ',', 'B', ',', 'C', ',', 'A', ',', 'B', ',', 'C', ',', 'B', '\n' };
                var a = new List<char> { 'L', ',', '8', ',', 'L', ',', '6', ',', 'L', ',', '9', ',', '1', ',', 'L', ',', '6', '\n' };
                var b = new List<char> { 'R', ',', '6', ',', 'L', ',', '8', ',', 'L', ',', '9', ',', '1', ',', 'R', ',', '6', '\n' };
                var c = new List<char> { 'R', ',', '6', ',', 'L', ',', '6', ',', 'L', ',', '9', ',', '1', '\n' };

                ascii.StartRobot(main, a, b, c, true);

                WriteToConsole($"The total dust collected is {ascii.DustCollected}");
            });
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
