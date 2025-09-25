namespace maze_generator
{
    // Manages maze generation
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

                // look for adjacent unvisited cells and add them to a list
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
        public MazeGraph WilsonsAlgorithm(int width, int height)
        {
            MazeGraph maze = new MazeGraph(width, height);
            List<Coordinate> RandomWalk = new List<Coordinate>();

            // choose a random cell and add it to the maze
            maze.AddCell(r.Next(0, width), r.Next(0, height));

            while (!maze.AllCellsVisited())
            {
                // find an unvisited cell to start the next random walk at by checking each cell in order
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
                    // if that cell is part of the maze, end the random walk
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

        // biased towards mazes with lots of short dead ends
        public MazeGraph IterativeRandomizedPrimsAlgorithm(int width, int height)
        {
            MazeGraph maze = new MazeGraph(width, height);
            // edges are stored as an array of 3 integers
            // the first integer is either 0 (indicating a horizontal edge) or 1 (vertical) and the second and third integers indicate the edge's position in the array of horizontal/vertical edges
            List<int[]> adjacentEdges = new List<int[]>();
            Coordinate current = new Coordinate(r.Next(0, width), r.Next(0, height));
            Coordinate next;
            maze.AddCell(current);

            while (!maze.AllCellsVisited())
            {
                // check edges adjacent to current cell, add to list if not there and remove if already there (because then they've definitely been used)
                if (current.x > 0)
                {
                    int[] e = new int[] { 0, current.x - 1, current.y };
                    if (maze.CellVisited(current.x - 1, current.y))
                    {
                        RemoveEdgeFromList(adjacentEdges, e);
                    }
                    else
                    {
                        adjacentEdges.Add(e);
                    }
                }
                if (current.x < width - 1)
                {
                    int[] e = new int[] { 0, current.x, current.y };
                    if (maze.CellVisited(current.x + 1, current.y))
                    {
                        RemoveEdgeFromList(adjacentEdges, e);
                    }
                    else
                    {
                        adjacentEdges.Add(e);
                    }
                }
                if (current.y > 0)
                {
                    int[] e = new int[] { 1, current.x, current.y - 1 };
                    if (maze.CellVisited(current.x, current.y - 1))
                    {
                        RemoveEdgeFromList(adjacentEdges, e);
                    }
                    else
                    {
                        adjacentEdges.Add(e);
                    }
                }
                if (current.y < height - 1)
                {
                    int[] e = new int[] { 1, current.x, current.y };
                    if (maze.CellVisited(current.x, current.y + 1))
                    {
                        RemoveEdgeFromList(adjacentEdges, e);
                    }
                    else
                    {
                        adjacentEdges.Add(e);
                    }
                }
                // choose a random edge from the list, add the edge and cell to the maze
                int n = r.Next(0, adjacentEdges.Count);
                int[] nextEdge = adjacentEdges[n];
                if (nextEdge[0] == 0)
                {
                    if (maze.CellVisited(nextEdge[1], nextEdge[2]))
                    {
                        next = new Coordinate(nextEdge[1] + 1, nextEdge[2]);
                    }
                    else
                    {
                        next = new Coordinate(nextEdge[1], nextEdge[2]);
                    }
                    maze.AddEdgeBetweenCells(new Coordinate(nextEdge[1] + 1, nextEdge[2]), new Coordinate(nextEdge[1], nextEdge[2]));
                }
                else
                {
                    if (maze.CellVisited(nextEdge[1], nextEdge[2]))
                    {
                        next = new Coordinate(nextEdge[1], nextEdge[2] + 1);
                    }
                    else
                    {
                        next = new Coordinate(nextEdge[1], nextEdge[2]);
                    }
                    maze.AddEdgeBetweenCells(new Coordinate(nextEdge[1], nextEdge[2] + 1), new Coordinate(nextEdge[1], nextEdge[2]));
                }

                maze.AddCell(next);
                current = next;
            }
            return maze;
        }
        public List<int[]> RemoveEdgeFromList(List<int[]> list, int[] edge)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i][0] == edge[0] && list[i][1] == edge[1] && list[i][2] == edge[2])
                {
                    list.RemoveAt(i);
                    return list;
                }
            }
            return list;
        }
    }
}