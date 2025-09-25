namespace maze_generator
{
    // Used to store statistics about a player
    class PlayerStats
    {
        public int totalAttempted;
        public int totalSolved;
        public double winRate;
        public TimeSpan timePlayed;
        public int totalPoints;
        public int highScore;
        public double averageScore;
        public TimeSpan bestTime;
        public TimeSpan averageTime;
        public List<MazeStats> recentMazes;
    }
}