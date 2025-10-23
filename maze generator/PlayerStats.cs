namespace maze_generator
{
    // Used to store statistics about a player
    class PlayerStats
    {
        // attributes
        private int totalAttempted;
        private int totalSolved;
        private double winRate;
        private TimeSpan timePlayed;
        private int totalPoints;
        private int highScore;
        private double averageScore;
        private int total50x50Solved;
        private TimeSpan total50x50Time;
        private TimeSpan best50x50Time;
        private TimeSpan average50x50Time;
        private List<MazeStats> recentMazes;

        public PlayerStats()
        {
            totalAttempted = 0;
            totalSolved = 0;
            winRate = 0;
            timePlayed = TimeSpan.Zero;
            totalPoints = 0;
            highScore = 0;
            averageScore = 0;
            total50x50Solved = 0;
            best50x50Time = new TimeSpan(1, 0, 0);
            average50x50Time = new TimeSpan(1, 0, 0);
            recentMazes = new List<MazeStats>();
        }

        // accessors
        public int GetTotalAttempted() { return totalAttempted; }
        public int GetTotalSolved() { return totalSolved; }
        public double GetWinRate() { return winRate; }
        public TimeSpan GetTimePlayed() { return timePlayed; }
        public int GetTotalPoints() { return totalPoints; }
        public int GetHighScore() { return highScore; }
        public double GetAverageScore() { return averageScore; }
        public TimeSpan GetBest5050Time() { return best50x50Time; }
        public TimeSpan GetAverage5050Time() { return average50x50Time; }
        public List<MazeStats> GetRecentMazes() { return recentMazes; }

        // mutators
        public void UpdateStats(MazeStats m)
        {
            int score = m.GetScore();
            bool solved = m.IsSolved();
            TimeSpan time = m.GetTime();
            totalAttempted++;
            if (solved) 
            { 
                totalSolved++;
                timePlayed += m.GetTime();
            }
            winRate = totalSolved / totalAttempted;
            timePlayed += m.GetTime();
            totalPoints += score;
            if (score > highScore) 
            { 
                highScore = score; 
            }
            averageScore = totalPoints / totalSolved;
            if (m.GetWidth() == 50 && m.GetHeight() == 50 && solved)
            {
                total50x50Solved++;
                total50x50Time += m.GetTime();
                if (time < best50x50Time)
                {
                    best50x50Time = time;
                }
                average50x50Time = total50x50Time / total50x50Solved;
            }
            recentMazes.Insert(0, m);
        }
    }
}