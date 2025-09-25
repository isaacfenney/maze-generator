namespace maze_generator
{
    class MazeSettings
    {
        private int width;
        private int height;
        private int algorithm; // 0 = depth-first; 1 = wilson's; 2 = prim's; 3 = kruskal's

        // constructor
        public MazeSettings(int width, int height, int algorithm)
        {
            this.width = width;
            this.height = height;
            this.algorithm = algorithm;
        }

        // accessors
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public int GetAlgorithm() { return algorithm; }
    }
}
