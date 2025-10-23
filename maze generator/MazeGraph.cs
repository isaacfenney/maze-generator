namespace maze_generator
{
    // Specialised graph where the nodes represent a rectangular grid, with an array of nodes and 2 arrays of horizontal/vertical edges between them
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
        public bool EdgeExistsBetweenCells(int cx, int cy, int dx, int dy)
        {
            Coordinate c = new Coordinate(cx, cy);
            Coordinate d = new Coordinate(dx, dy);
            return EdgeExistsBetweenCells(c, d);
        }
        public bool EdgeExistsBetweenCells(Coordinate c, Coordinate d)
        {
            if (c.x == d.x && Math.Abs(c.y - d.y) == 1)
            {
                if (verticalEdges[c.x, Math.Min(c.y, d.y)])
                {
                    return true;
                }
            }
            else if (c.y == d.y && Math.Abs(c.x - d.x) == 1)
            {
                if (horizontalEdges[Math.Min(c.x, d.x), c.y])
                {
                    return true;
                }
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
            else
            {
                throw new Exception("Attempted to add an edge between two non-adjacent cells.");
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

            // cover the edges of the maze with walls, except the right wall of the top-right cell because that's the exit
            for (int i = 0; i <= 2 * width; i++)
            {
                DisplayedMaze[i, 0] = '█';
                DisplayedMaze[i, 2 * height] = '█';
            }
            DisplayedMaze[0, 1] = '█';
            for (int i = 2; i < 2 * height; i++)
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
}