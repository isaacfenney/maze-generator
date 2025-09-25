namespace maze_generator
{
    internal class Program
    {
        static void Main()
        {
            Game g = new Game();
            g.PlayGame();
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
            MazeGraph m = g.WilsonsAlgorithm(width, height);
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
        static void PrimTest(int width, int height)
        {
            MazeGenerator g = new MazeGenerator();
            MazeGraph maze = g.IterativeRandomizedPrimsAlgorithm(width, height);
            char[,] display = maze.ReturnDisplayableMaze();
            for (int j = 0; j < display.GetLength(1); j++)
            {
                for (int i = 0; i < display.GetLength(0); i++)
                {
                    Console.Write(display[i, j]); Console.Write(display[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void MazeGenTimingTest()
        {
            MazeGenerator g = new MazeGenerator();

            DateTime primstart = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                g.IterativeRandomizedPrimsAlgorithm(50, 50);
            }
            DateTime primend = DateTime.Now;
            TimeSpan primtime = primend - primstart;
            Console.WriteLine("prim " + primtime.TotalMilliseconds);

            DateTime wilsonstart = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                g.WilsonsAlgorithm(50, 50);
            }
            DateTime wilsonend = DateTime.Now;
            TimeSpan wilsontime = wilsonend - wilsonstart;
            Console.WriteLine("wilson " + wilsontime.TotalMilliseconds);

            DateTime depthstart = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                g.RandomizedDepthFirstSearch(50, 50);
            }
            DateTime depthend = DateTime.Now;
            TimeSpan depthtime = depthend - depthstart;
            Console.WriteLine("depth " + depthtime.TotalMilliseconds);
        }
        static void GuessingGame()
        {
            int numberOfRounds = 0;
            int score = 0;
            while (true)
            {
                numberOfRounds++;
                int r = new Random().Next(1, 4);
                switch (r)
                {
                    case 1: RDFSTest(10, 10); break;
                    case 2: WilsonTest(10, 10); break;
                    case 3: PrimTest(10, 10); break;
                }
                Console.WriteLine("Guess which algorithm this maze was generated with!");
                Console.WriteLine("1: randomized depth-first search; 2: Wilson's algorithm; 3: iterative randomized Prim's algorithm");
                int guess = Convert.ToInt32(Console.ReadLine());
                if (guess == r)
                {
                    score++;
                    Console.WriteLine("Correct!");
                }
                else
                {
                    Console.WriteLine("Incorrect! The answer was " + r);
                }
                Console.WriteLine($"You won {score} out of {numberOfRounds} rounds played.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}