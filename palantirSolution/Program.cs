using System;
using System.Collections.Generic;

using System.Linq;

namespace palantirSolution
{
    enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down =3,
        Diagup =4,
        Diagdown = 5,
        Revdiagup = 6,
        Revdiagdown =7,

    }

    class Node
    {
        public bool Visted;
        public char Sink;
        public int Elevation;
        public bool IsSink; 

        public Node(int hight)
        {
            Elevation = hight;
            Sink = '1';
            Visted = false;
            IsSink = false;

        }
    }

    class Program
    {
        private static char _currentSink = '0';
        private static int _numberOfNodes;
        private static int _dimensions;
        private static Dictionary<int,int> _sinkCounter = new Dictionary<int, int>();
        
        static void Main()
        {
            _dimensions = Int32.Parse(Console.ReadLine().Trim());
            Node[,] input = new Node[_dimensions,_dimensions];
            
            
            for (int i = 0; i < _dimensions; i++)
            {
                String[] inputArray = Console.ReadLine().Trim().Split(' ');

                for (int j = 0; j < _dimensions; j++)
                {
                    input[i, j] = new Node(Int32.Parse(inputArray[j]));
                }
            }

            //int[,] sample = { {0,2,1,3},{2,1,0,4},{3,3,3,3},{5,5,2,1} };//{{1,5,2},{2,4,7},{3,6,9}};
            //_dimensions = sample.GetLength(0);
            
            Solve(input);
        }

        static void Solve(Node[,] input)
        {
            if (_dimensions == 1)
            {
                Console.Write("1");
                return;
            }

            //find basins 
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(0); j++)
                {
                    if (CheckIfSink(input, i, j))
                    {
                        input[i, j].IsSink = true;
                        input[i, j].Sink = (char)('A' + _sinkCounter.Count);
                        _sinkCounter.Add(_sinkCounter.Count,1);
                        _numberOfNodes++;
                    }
                }
            }

            if (_sinkCounter.Count == 1)
            {
                Console.WriteLine(Math.Pow(_dimensions,2));
                return;
            }


            while (_numberOfNodes < Math.Pow(_dimensions,2))
            {
                for (int i = 0; i < input.GetLength(0); i++)
                {
                    for (int j = 0; j < input.GetLength(0); j++)
                    {
                        if (IsTallest(input, i, j))
                        {
                            Drain(input, i, j, FindNextDirection(input, i, j));

                        }
                            
                    }
                }
//                PrintArray(input);
//                Console.WriteLine("\n");
            }

            //PrintArray(input);
            //Console.WriteLine("\nAnswer: ");
            var sorted = _sinkCounter.Values.ToList().OrderByDescending(x => x);
            foreach (var number in sorted)
            {
                Console.Write(number+" ");
            }

            Console.ReadLine();

        }

        /// <summary>
        /// Simulates draining.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="i">i coordinate.</param>
        /// <param name="j">j coordinate.</param>
        /// <param name="direction">The direction.</param>
        static void Drain(Node[,] array, int i, int j, Direction direction)
        {
            int i2 =0, j2 = 0;

            //checks to see if sink and.... water has reached the destination 
            if (array[i, j].IsSink)
            {
                _currentSink = array[i, j].Sink;
                return; 
            }
            
            switch (direction)
            {
                case Direction.Left:
                    j2 = j-1;
                    i2 = i;
                    break;
                case Direction.Right:
                    j2 = j+1;
                    i2 = i;
                    break;
                case Direction.Up:
                    j2 = j;
                    i2 = i-1;
                    break;
                case Direction.Down:
                    j2 =j;
                    i2 = i+1;
                    break;
                case Direction.Diagup:
                    j2 = j + 1;
                    i2 = i + 1;
                    break;
                case Direction.Diagdown:
                    j2 = j - 1;
                    i2 = i - 1;
                    break;
                case Direction.Revdiagup:
                    i2 = i + 1;
                    j2 = j - 1;
                    break;
                case Direction.Revdiagdown:
                    i2 = i - 1;
                    j2 = j + 1;
                    break;
            }
            direction = FindNextDirection(array, i2, j2);
            Drain(array, i2, j2, direction);
            array[i, j].Sink = _currentSink;
            if (!array[i, j].Visted)
            {
                _numberOfNodes++;
                _sinkCounter[_currentSink - 'A']++;
            }
            array[i, j].Visted = true;
        }

        /// <summary>
        /// Finds the next direction.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="i">i coordinate.</param>
        /// <param name="j">j coordinate.</param>
        /// <returns></returns>
        static Direction FindNextDirection(Node[,] array, int i, int j)
        {
            Direction direction = Direction.Down;
            int currentValue = array[i, j].Elevation;


            if (j != 0)
                if (array[i, j-1].Elevation < currentValue)
                {
                    currentValue = array[i, j-1].Elevation;
                    direction = Direction.Left;
                }
            if (i != array.GetLength(0) - 1)
                if (array[i+1, j].Elevation < currentValue)
                {
                    currentValue = array[i+1,j].Elevation;
                    direction = Direction.Down;
                }

            if (i != 0)
                if (array[i - 1, j].Elevation < currentValue)
                {
                    currentValue = array[i - 1, j].Elevation;
                    direction = Direction.Up;
                }

            if (j != array.GetLength(0) - 1)
                if ( array[i, j + 1].Elevation < currentValue)
                {
                    currentValue = array[i, j + 1].Elevation;
                    direction = Direction.Right;
                }

            if (i != array.GetLength(0) - 1 && j != array.GetLength(0) - 1)
                if (array[i + 1, j + 1].Elevation < currentValue)
                {
                    currentValue = array[i+1, j + 1].Elevation;
                    direction = Direction.Diagup;
                    
                }
            if (i != array.GetLength(0) - 1 && j != 0)
                if (array[i + 1, j - 1].Elevation < currentValue)
                {
                    currentValue = array[i+1, j - 1].Elevation;
                    direction = Direction.Revdiagup;
                    
                }
            if (i != 0 && j != 0)
                if (array[i - 1, j - 1].Elevation < currentValue)
                {
                    currentValue = array[i-1, j -1].Elevation;
                    direction = Direction.Diagdown;
                    
                }
            if (i != 0 && j != array.GetLength(0) - 1)
                if (array[i - 1, j + 1].Elevation < currentValue)
                {
                    currentValue = array[i-1, j + 1].Elevation;
                    direction = Direction.Revdiagdown;
                    
                }
            
            return direction;
        }

        public static void PrintArray(Node[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(0); j++)
                {
                    Console.Write(arr[i,j].Sink + "\t" );
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Determines whether the specified array location is tallest.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="i">i coordinate.</param>
        /// <param name="j">j coordinate.</param>
        /// <returns></returns>
        static bool IsTallest(Node[,] array, int i, int j)
        {
            int value = array[i,j].Elevation;
            
            if (array[i, j].IsSink)
                return false;

            if (array[i, j].Visted)
                return false;

            if (i !=0 )
                if (array[i - 1, j].Elevation > value && !array[i-1,j].Visted)
                    return false;

            if (j != 0)
                if (array[i, j - 1].Elevation > value && !array[i,j-1].Visted)
                    return false;

            if(i != array.GetLength(0)-1)
                if(array[i+1,j].Elevation > value && !array[i+1,j].Visted)
                    return false;

            if (j != array.GetLength(0) -1)
                if(array[i,j+1].Elevation > value && !array[i,j+1].Visted)
                    return false;


            if (i != array.GetLength(0) - 1 && j != array.GetLength(0) - 1)
                if (array[i + 1, j+1].Elevation > value && !array[i + 1, j+1].Visted)
                    return false;
            if (i != array.GetLength(0) - 1 && j != 0)
                if (array[i + 1, j-1].Elevation > value && !array[i + 1, j-1].Visted)
                    return false;
            if (i != 0 && j != 0)
                if (array[i - 1, j-1].Elevation > value && !array[i - 1, j-1].Visted)
                    return false;
            if (i != 0 && j != array.GetLength(0) - 1)
                if (array[i - 1, j+1].Elevation > value && !array[i -1, j+1].Visted)
                    return false;

            return true;
        }

        /// <summary>
        /// Checks if location within the array is a sink.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="i">i coordinate.</param>
        /// <param name="j">j coordinate.</param>
        /// <returns></returns>
        static bool CheckIfSink(Node[,] array, int i, int j)
        {
            int value = array[i,j].Elevation;

            if (i !=0)
                if (array[i - 1, j].Elevation < value)
                    return false;

            if (j != 0)
                if (array[i, j - 1].Elevation < value)
                    return false;

            if(i != array.GetLength(0)-1)
                if(array[i+1,j].Elevation < value)
                    return false;

            if (j != array.GetLength(0) -1)
                if(array[i,j+1].Elevation < value)
                    return false;

            if (i != array.GetLength(0) - 1 && j != array.GetLength(0) - 1)
                if (array[i + 1, j + 1].Elevation < value)
                    return false;

            if (i != array.GetLength(0) - 1 && j!=0)
                if (array[i + 1, j - 1].Elevation < value)
                    return false;

            if (i != 0 && j != 0)
                if (array[i - 1, j - 1].Elevation < value)
                    return false;

            if (i != 0 && j != array.GetLength(0) - 1)
                if (array[i - 1, j + 1].Elevation < value)
                    return false;

            return true;
        }


    }
}
