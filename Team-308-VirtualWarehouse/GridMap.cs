using System;
namespace Team_308_VirtualWarehouse
{
	public class GridMap
	{
        // Fixed grid dimensions
        private const int gridSize = 5;
        private const int gridScale = 2; // Each grid is 2x2 feet

        private Ellipse circle;

        // The data structure that will hold the real-world coordinates of the center of each grid.
        private (int x, int y)[,] gridContents;
        // variable to set origin coordinatest
        private (int x, int y) origin;
        private (int x, int y) currentCoordinates;


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

        // example set
        public void SetGridContent(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of range.");
            }
            gridContents[i, j] = (x, y);
        }

        // example get
        public GridContent GetGridContent(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of range.");
            }
            return gridContents[x, y];
        }

        public void SetCircle(int x, int y)
        {
            if (circle != NULL)
            {
                this->circle = null;
                Ellipse circle = new Ellipse(50, 50, x, y);
            }
            else if (circleDraw == NULL)
            {
                Ellipse circle = new Ellipse(50, 50, x, y);
            }
        }

        // overloading with parser
        public GridContent GetCoordinates()
        {
            currentCoordinates = Form1.GetCoordinates();
            Console.WriteLine($"X: {currentCoordinates.x}, Y: {currentCoordinates.y}");
        }

        // Function to set the origin
        public void setOrigin()
        {
            // 'origin' holds the x, y, z values used for further calculations
            origin = Form1.GetCoordinates();
        }

        public void DisplayOrigin()
        {
            Console.WriteLine($"Origin is set to: X={origin.x}, Y={origin.y}");
        }

        // get current location difference than original location
        public (int x, int y) calculateLocationDifference()
        {
            int diffX = currentCoordinates.x - origin.x;
            int diffY = currentCoordinates.y - origin.y;
            return (diffX, diffY);
        }

    }

}

