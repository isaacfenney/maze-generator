namespace maze_generator
{
    class MazeGraph
    {
        // attributes
        private int width;
        private int height;
        private bool[,] cells;
        private bool[,] horizontalEdges;
        private bool[,] verticalEdges;

        // constructors
        public MazeGraph(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new bool[width, height];
            horizontalEdges = new bool[width - 1, height];
            verticalEdges = new bool[width, height - 1];
        }

        // accessors
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public bool CellVisited(Coordinate c)
        {
            if (cells[c.x, c.y])
            { 
                return true;
            }
            return false;
        }
        public bool CellVisited(int x, int y)
        {
            if (cells[x, y])
            {
                return true;
            }
            return false;
        }

        // mutators
        public void AddCell(int x, int y)
        {
            cells[x, y] = true;
        }
        public void AddCell(Coordinate c)
        {
            cells[c.x, c.y] = true;
        }
        public void AddEdgeBetweenCells(Coordinate c, Coordinate d)
        {
            if (Math.Abs(c.x - d.x) == 1 && c.y == d.y)
            {
                horizontalEdges[Math.Min(c.x, d.x), c.y] = true;
            }
            else if (c.x == d.x && Math.Abs(c.y - d.y) == 1)
            {
                verticalEdges[c.x, Math.Min(c.y, d.y)] = true;
            }
        }

        // other
        public bool AllCellsVisited()
        {
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (!cells[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public char[,] ReturnDisplayableMaze()
        {
            char[,] DisplayedMaze = new char[2 * width + 1, 2 * height + 1];

            // cover the edges of the maze with walls
            for (int i = 0; i <= 2 * width; i++)
            {
                DisplayedMaze[i, 0] = '█';
                DisplayedMaze[i, 2 * height] = '█';
            }
            for (int i = 1; i < 2 * height; i++)
            {
                DisplayedMaze[0, i] = '█';
                DisplayedMaze[2 * width, i] = '█';
            }

            // place a wall in every space with even coordinates (because no cells/edges exist there)
            for (int j = 2; j < 2 * height; j += 2)
            {
                for (int i = 2; i < 2 * width; i += 2)
                {
                    DisplayedMaze[i, j] = '█';
                }
            }

            // place spaces/walls where there are/aren't horizontal edges
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width - 1; i++)
                {
                    if (!horizontalEdges[i, j])
                    {
                        DisplayedMaze[2 * i + 2, 2 * j + 1] = '█';
                    }
                    else
                    {
                        DisplayedMaze[2 * i + 2, 2 * j + 1] = ' ';
                    }
                }
            }

            // place spaces/walls where there are/aren't vertical edges
            for (int j = 0; j < height - 1; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (!verticalEdges[i, j])
                    {
                        DisplayedMaze[2 * i + 1, 2 * j + 2] = '█';
                    }
                    else
                    {
                        DisplayedMaze[2 * i + 1, 2 * j + 2] = ' ';
                    }
                }
            }

            // place a space at every cell
            for (int j = 1; j < 2 * height; j += 2)
            {
                for (int i = 1; i < 2 * width; i += 2)
                {
                    DisplayedMaze[i, j] = ' ';
                }
            }    

            return DisplayedMaze;
        }
    }

    class MazeGenerator
    {
        private Random r;

        // constructor
        public MazeGenerator()
        {
            r = new Random();
        }

        // maze generation algorithms

        // biased towards mazes with long winding corridors
        public MazeGraph RandomizedDepthFirstSearch(int width, int height)
        {
            MazeGraph maze = new MazeGraph(width, height);
            List<Coordinate> potentialNextCells = new List<Coordinate>();
            Stack<Coordinate> path = new Stack<Coordinate>();
            Coordinate current;
            Coordinate next;

            // choose a random starting cell
            current = new Coordinate(r.Next(0, width), r.Next(0, height));
            maze.AddCell(current);
            path.Push(current);

            while (!maze.AllCellsVisited())
            {
                current = path.Peek();
                potentialNextCells.Clear();
                
                // look for adjacent unvisited cells
                if (current.x > 0)
                {
                    if (!maze.CellVisited(current.x - 1, current.y))
                    {
                        potentialNextCells.Add(new Coordinate(current.x - 1, current.y));
                    }
                }
                if (current.x < width - 1)
                {
                    if (!maze.CellVisited(current.x + 1, current.y))
                    {
                        potentialNextCells.Add(new Coordinate(current.x + 1, current.y));
                    }
                }
                if (current.y > 0)
                {
                    if (!maze.CellVisited(current.x, current.y - 1))
                    {
                        potentialNextCells.Add(new Coordinate(current.x, current.y - 1));
                    }
                }
                if (current.y < height - 1)
                {
                    if (!maze.CellVisited(current.x, current.y + 1))
                    {
                        potentialNextCells.Add(new Coordinate(current.x, current.y + 1));
                    }
                }
                // backtrack if there aren't any
                if (potentialNextCells.Count == 0)
                {
                    path.Pop();
                }
                // otherwise, choose a random adjacent unvisited cell and add it to the maze
                else
                {
                    next = potentialNextCells[r.Next(0, potentialNextCells.Count)];
                    maze.AddCell(next);
                    maze.AddEdgeBetweenCells(current, next);
                    path.Push(next);
                }
            }
            return maze;
        }

        // equally likely to generate any maze of the given width/height (NB: may take a while if you're unlucky)
        public MazeGraph WilsonAlgorithm(int width, int height)
        {
            MazeGraph maze = new MazeGraph(width, height);
            List<Coordinate> RandomWalk = new List<Coordinate>();

            // choose a random cell and add it to the maze
            maze.AddCell(r.Next(0, width), r.Next(0, height));

            while (!maze.AllCellsVisited())
            {
                // find an unvisited cell to start the next random walk at
                bool nextUnvisitedCellFound = false;
                int i = 0;
                int j = 0;
                while (!nextUnvisitedCellFound)
                {
                    if (!maze.CellVisited(i, j))
                    {
                        nextUnvisitedCellFound = true;
                        Coordinate nextUnvisitedCell = new Coordinate(i, j);
                        RandomWalk.Add(nextUnvisitedCell);
                    }
                    else
                    {
                        i++;
                        if (i == width)
                        {
                            i = 0;
                            j++;
                        }
                    }
                }
                // perform a random walk until a visited cell is reached
                bool walkHasReachedMaze = false;
                while (!walkHasReachedMaze)
                {
                    // choose a random cell to walk to
                    Coordinate current = RandomWalk.Last();
                    List<Coordinate> potentialNextCells = new List<Coordinate>();
                    if (current.x > 0)
                    {
                        potentialNextCells.Add(new Coordinate(current.x - 1, current.y));
                    }
                    if (current.x < width - 1)
                    {
                        potentialNextCells.Add(new Coordinate(current.x + 1, current.y));
                    }
                    if (current.y > 0)
                    {
                        potentialNextCells.Add(new Coordinate(current.x, current.y - 1));
                    }
                    if (current.y < height - 1)
                    {
                        potentialNextCells.Add(new Coordinate(current.x, current.y + 1));
                    }
                    Coordinate next = potentialNextCells[r.Next(0, potentialNextCells.Count())];
                    // check if that cell is already part of the walk
                    bool walkContainsNext = false;
                    foreach (Coordinate c in RandomWalk)
                    {
                        if (Coordinate.Equals(c, next))
                        {
                            walkContainsNext = true;
                            break;
                        }
                    }
                    // if not, add it
                    if (!walkContainsNext)
                    {
                        RandomWalk.Add(next);
                    }
                    // if so, erase the loop
                    else
                    {
                        while (!Coordinate.Equals(RandomWalk.Last(), next))
                        {
                            RandomWalk.RemoveAt(RandomWalk.Count - 1);
                        }
                    }
                    // check if it's reached the maze yet
                    if (maze.CellVisited(next))
                    {
                        walkHasReachedMaze = true;
                    }
                }
                // add the path of the random walk into the maze
                while (RandomWalk.Count > 1)
                {
                    maze.AddEdgeBetweenCells(RandomWalk.Last(), RandomWalk[RandomWalk.Count - 2]);
                    maze.AddCell(RandomWalk[RandomWalk.Count - 2]);
                    RandomWalk.RemoveAt(RandomWalk.Count - 1);
                }
                RandomWalk.RemoveAt(0);
            }
            return maze;
        }
    }

    class Coordinate
    {
        public int x; public int y;

        // constructor
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // other
        public static bool Equals(Coordinate a, Coordinate b)
        {
            return a.x == b.x && a.y == b.y;
        }
    }

    internal class Program
    {
        static void Main()
        {
            WilsonTest(10, 10);
        }

        // misc tests
        static void RDFSTest(int width, int height)
        {
            MazeGenerator g = new MazeGenerator();
            MazeGraph m = g.RandomizedDepthFirstSearch(width, height);
            char[,] maze = m.ReturnDisplayableMaze();
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    Console.Write(maze[i, j]); Console.Write(maze[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void WilsonTest(int width, int height)
        {
            MazeGenerator g = new MazeGenerator();
            MazeGraph m = g.WilsonAlgorithm(width, height);
            char[,] maze = m.ReturnDisplayableMaze();
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    Console.Write(maze[i, j]); Console.Write(maze[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}