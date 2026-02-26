namespace maze_generator
{
    // Used to store statistics about a completed maze
    class MazeStats
    {
        // attributes
        private int width;
        private int height;
        private int algorithm;
        private bool solved;
        private TimeSpan time;
        private int score;

        // constructor
        public MazeStats(int width, int height, int algorithm, bool solved, TimeSpan time, int score)
        {
            this.width = width;
            this.height = height;
            this.algorithm = algorithm;
            this.solved = solved;
            if (solved)
            {
                this.time = time;
                this.score = score;
            }
            else
            {
                this.time = new TimeSpan(1, 0, 0);
                this.score = 0;
            }
        }

        // accessors
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public int GetAlgorithm() { return algorithm; }
        public int GetScore() { return score; }
        public TimeSpan GetTime() { return time; }
        public bool WasSolved() { return solved; }
    }
}