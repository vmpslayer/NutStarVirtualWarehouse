using System;
namespace Team_308_VirtualWarehouse
{
	public class GridMap
	{
        // Fixed grid dimensions
        private const int gridSize = 5;
        private const int gridScale = 2; // Each grid is 2x2 feet

        // The data structure that will hold the real-world coordinates of the center of each grid.
        private (int x, int y)[,] gridContents;

        public GridMap()
        {
            // Initialize the gridContents with the fixed size of 5x5.
            gridContents = new (int x, int y)[gridSize, gridSize];

            // Initialize all grid contents with their real-world coordinates.
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    // Calculate the real-world coordinates for the center of each grid cell.
                    int realWorldX = (i * gridScale) + gridScale;
                    int realWorldY = (j * gridScale) + gridScale;
                    gridContents[i, j] = (realWorldX, realWorldY);
                }
            }
        }

        // Method to set the content of a grid cell
        public void SetGridContent(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of range.");
            }
            gridContents[i, j] = (x, y);
        }

        // Method to get the content of a grid cell
        public GridContent GetGridContent(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of range.");
            }
            return gridContents[x, y];
        }
    }

    public class GridContent
    {
        // Add properties to store information about what is in each grid.

    }
}

