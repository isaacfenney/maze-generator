namespace maze_generator
{
    class Game
    {
        private MazeGenerator g;
        private PlayerStats p = new PlayerStats();

        public Game()
        {
            g = new MazeGenerator();
        }

        public void PlayGame()
        {
            Console.WriteLine("Welcome to my maze game.");
            Console.WriteLine("Use WASD or the arrow keys to move.");
            Console.WriteLine("You will start in the bottom left corner of the maze and attempt to reach the top right corner.");
            Console.WriteLine();
            int menuChoice = -1;
            while (menuChoice != 3)
            {
                Console.WriteLine("What would you like to do?" + "\n" +
                    "1. Play" + "\n" +
                    "2. View statistics" + "\n" +
                    "3. Quit");
                try
                {
                    menuChoice = Convert.ToInt32(Console.ReadLine());
                    switch (menuChoice)
                    {
                        case 1:
                            MazeGraph maze = GenerateMaze();
                            DisplayMaze(maze);
                            PlayMaze(maze);
                            break;
                        case 2:
                            Console.WriteLine("Not implemented yet");
                            break;
                        case 3:
                            Console.WriteLine("Goodbye.");
                            break;
                    }
                }
                catch { }
            }
        }
        
        public MazeGraph GenerateMaze() // asks the player to specify the width/height of the maze and algorithm used, then generates it
        {
            int width = -1;
            int height = -1;
            int algo = -1;

            bool widthEntered = false;
            while (!widthEntered)
            {
                Console.Write("Enter maze width (minimum 5, maximum 50): ");
                try
                {
                    width = Convert.ToInt32(Console.ReadLine());
                    if (width < 5 || width > 50)
                    {
                        Console.WriteLine("Please enter an integer between 5 and 50.");
                    }
                    else
                    {
                        widthEntered = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter an integer between 5 and 50.");
                }
            }
            Console.WriteLine();

            bool heightEntered = false;
            while (!heightEntered)
            {
                Console.Write("Enter maze height (minimum 5, maximum 50): ");
                try
                {
                    height = Convert.ToInt32(Console.ReadLine());
                    if (height < 5 || height > 50)
                    {
                        Console.WriteLine("Please enter an integer between 5 and 50.");
                    }
                    else
                    {
                        heightEntered = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter an integer between 5 and 50.");
                }
            }

            Console.WriteLine( "\n" +
                "Maze generation algorithms to choose from:" + "\n" + 
                "1. Randomized depth-first search (default)" + "\n" +
                "2. Wilson's algorithm" + "\n" +
                "3. Iterative randomized Prim's algorithm" + "\n" +
                "4. Choose a random algorithm from the list above");
           
            Console.Write("Enter a number to choose which algorithm to use, or anything else for the default: ");
            try
            {
                algo = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                algo = 1;
            }
            if (algo < 0 || algo > 4)
            {
                algo = 1;
            }
            if (algo == 4)
            {
                algo = new Random().Next(1, 4);
            }

            Console.WriteLine("Generating maze (this might take a while if you entered big numbers)...");
            switch (algo)
            {
                case 1:
                    return g.RandomizedDepthFirstSearch(width, height);
                case 2:
                    return g.WilsonsAlgorithm(width, height);
                case 3:
                    return g.IterativeRandomizedPrimsAlgorithm(width, height);
                default:
                    throw new Exception("If you are reading this, I messed up somehow.");
            }
        }
        public void DisplayMaze(MazeGraph maze)
        {
            Console.Clear();
            Console.SetWindowSize(4 * maze.GetWidth() + 2, Math.Min(2 * maze.GetHeight() + 1, 60));
            char[,] display = maze.ReturnDisplayableMaze();
            for (int j = 0; j < display.GetLength(1); j++)
            {
                for (int i = 0; i < display.GetLength(0); i++)
                {
                    Console.Write(display[i, j]); Console.Write(display[i, j]);
                }
                if (j != display.GetLength(1) - 1)
                {
                    Console.WriteLine();
                }
            }
            // place player
            Console.SetCursorPosition(2, 2 * maze.GetHeight() - 1);
            Console.Write("<3");
        }
        public void PlayMaze(MazeGraph maze)
        {
            Coordinate player = new Coordinate(0, maze.GetHeight() - 1);
            Coordinate nextPosition = new Coordinate(-1, -1);
            ConsoleKey input;
            bool gameOver = false;

            DateTime startTime = DateTime.Now;
            
            while (!gameOver)
            {
                // get and handle player input
                input = Console.ReadKey(true).Key;
                switch (input)
                {
                    case ConsoleKey.UpArrow or ConsoleKey.W:
                        if (player.y > 0)
                        {
                            nextPosition = new Coordinate(player.x, player.y - 1);
                        }
                        break;
                    case ConsoleKey.DownArrow or ConsoleKey.S:
                        if (player.y < maze.GetHeight() - 1)
                        {
                            nextPosition = new Coordinate(player.x, player.y + 1);
                        }
                        break;
                    case ConsoleKey.LeftArrow or ConsoleKey.A:
                        if (player.x > 0)
                        {
                            nextPosition = new Coordinate(player.x - 1, player.y);
                        }
                        break;
                    case ConsoleKey.RightArrow or ConsoleKey.D:
                        if (player.x == maze.GetWidth() - 1 && player.y == 0)
                        {
                            gameOver = true;
                            break;
                        }
                        if (player.x < maze.GetWidth() - 1)
                        {
                            nextPosition = new Coordinate(player.x + 1, player.y);
                        }
                        break;
                }
                if (gameOver)
                {
                    break;
                }
                // if possible, move and draw the player in their new position
                if (maze.EdgeExistsBetweenCells(player, nextPosition))
                {
                    Console.SetCursorPosition(4 * player.x + 2, 2 * player.y + 1);
                    Console.Write("  ");
                    player = nextPosition;
                    Console.SetCursorPosition(4 * player.x + 2, 2 * player.y + 1);
                    Console.Write("<3");
                }
            }
            DateTime endTime = DateTime.Now;
            TimeSpan timeTaken = endTime - startTime;
            int timeMinutes = (int)Math.Floor(timeTaken.TotalMinutes);
            double timeSeconds = (Math.Truncate(timeTaken.TotalMilliseconds) / 1000) % 60;
            int score = (int)Math.Floor(10 * maze.GetWidth() * maze.GetHeight() / timeTaken.TotalSeconds);

            Console.Clear();
            Console.SetWindowSize(120, 30);
            Console.WriteLine($"Congratulations! You cleared a {maze.GetWidth()}x{maze.GetHeight()} maze in {timeMinutes} minute(s) and {timeSeconds} seconds.");
            Console.WriteLine($"Score: {score} (based on size and time taken)");
            Thread.Sleep(1000);
        }
    }
}