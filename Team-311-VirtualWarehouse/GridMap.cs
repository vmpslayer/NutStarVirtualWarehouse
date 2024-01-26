using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;
// using Internal;

namespace Team_308_VirtualWarehouse
{
    public partial class GridMap : Form
    {
        // Fixed grid dimensions
        private const int gridSize = 5;
        private const int width = gridSize;
        private const int height = gridSize;
        private const int gridScale = 2; // Each grid is 2x2 feet
        private const int mapScale = 30;

        static private readonly int mapWidth = (GridMap.width * gridScale) * mapScale;
        static private readonly int mapHeight = (GridMap.height * gridScale) * mapScale;

        private readonly int windowWidth = ((GridMap.width * gridScale) * mapScale) + 25;
        private readonly int windowHeight = ((GridMap.height * gridScale) * mapScale) + 45;        

        // The data structure that will hold the real-world coordinates of the center of each grid.
        private (int x, int y)[,] gridContents = new (int x, int y)[gridSize, gridSize];

        // grid name such as: 101, 102 is stored in here with corresponding index i and j
        private string[,] gridNames;
        private string resultGridName;
        // variable to set origin coordinatest
        private (int x, int y) origin;
        private (int x, int y) currentCoordinates;
        double normalizedX, normalizedY;

        bool originisSet;

        // Classification for height (z)
        char resultChar;

        private const int MaxCoordinates = 10;
        // average data container
        private List<(int x, int y)> coordinatesBuffer = new List<(int x, int y)>();
        private Form1 formInstance;

        static bool initialOpenGridMap = false;

        public GridMap(Form1 form)
        {
            // ***** just added testing *****
            Size newSize;
            if (windowWidth > 1920 || windowHeight > 1080)
            {
                 newSize = new Size(800, 800);
            }
            else
            {
                newSize = new Size(windowWidth, windowHeight);
            }
            this.Size = newSize;

            this.formInstance = form;
            redPen = new Pen(Color.Red)

            this.Paint += PaintGridMap;
            // PaintGridMap();

            // Initialize the gridContents with the fixed size of 5x5.
            gridContents = new (int x, int y)[gridSize, gridSize];
            gridNames = new string[gridSize, gridSize];

            // Initialize all grid contents with their real-world coordinates.
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    // Calculate the real-world coordinates for the center of each grid cell.
                    // map is initialized from bottom left with 2*2 boundry input into each grid
                    // realWorldX and realWorldY are the maximum boundry limit in x and y axis
                    int realWorldX = (i * gridScale) + gridScale;
                    int realWorldY = (j * gridScale) + gridScale;
                    gridContents[i, j] = (realWorldX, realWorldY);

                    // Initialize grid names
                    // ex: [0, 1] = 102
                    gridNames[i, j] = $"{i + 1}{j + 1:D2}";
                }
            }
            originisSet = false;
        }

        public void PaintGridMap(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);
            //Graphics g = Graphics.FromImage(drawing);
            Graphics g = e.Graphics;
            drawMap(g);
            SetCircle(g);
        }

        // example set
        // public void SetGridContent(int x, int y)
        // {
        //     if (x < 0 || x >= width || y < 0 || y >= height)
        //     {
        //         throw new ArgumentOutOfRangeException("Coordinates are out of range.");
        //     }
        //     gridContents[i, j] = (x, y);
        // }

        // example get
        public (int, int) GetGridContent(int i, int j)
        {
            if (i < 0 || i >= width || j < 0 || j >= height)
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of range.");
            }
            return gridContents[i, j];
        }

        // overloading with parser
        public void SetCoordinates()
        {
            for (int i = 0; i < MaxCoordinates; i++)
            {
                currentCoordinates = formInstance.GetCoordinates();
                if (coordinatesBuffer.Count >= MaxCoordinates)
                {
                    coordinatesBuffer.RemoveAt(0); // Remove the oldest coordinate
                }
                coordinatesBuffer.Add(currentCoordinates);
            }
        }

        private void normalizeData()
        {
            if (coordinatesBuffer.Count == 0)
            {
                throw new InvalidOperationException("No coordinates data available to calculate average.");
            }

            double sumX = 0;
            double sumY = 0;
            foreach (var coord in coordinatesBuffer)
            {
                sumX += coord.x;
                sumY += coord.y;
            }

            normalizedX = sumX / coordinatesBuffer.Count;
            normalizedY = sumY / coordinatesBuffer.Count;

            // return (normalizedX, normalizedY);
        }

        // Function to set the origin
        public void setOrigin()
        {
            // Form1 formInstance = new Form1();
            // 'origin' holds the x, y, z values used for further calculations
            origin = formInstance.GetCoordinates();
            originisSet = true;
        }

        public void DisplayOrigin()
        {
            Console.WriteLine($"Origin is set to: X={origin.x}, Y={origin.y}");
        }

        // get current location difference than original location
        public (int x, int y) calculateLocationDifference()
        {
            SetCoordinates();
            normalizeData();

            int diffX = (int)normalizedX - origin.x;
            int diffY = (int)normalizedY - origin.y;

            // TODO: ratio diffX and diffY to real world coordinate sensitivity


            return (diffX, diffY);
        }

        public string calculateGrid()
        {
            int tempX, tempY;

            if (!originisSet)
            {
                Console.WriteLine("Origin is not set, try again\n");
                return null;
            }

            (tempX, tempY) = calculateLocationDifference();
            Console.WriteLine($"diffX: {tempX}, diffY: {tempY}");

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    // find the grid location based on first smallest boundry within
                    if (tempX <= gridContents[i, j].x && tempY <= gridContents[i, j].y)
                    {
                        // return grid name if found
                        return gridNames[i, j];
                    }
                }
            }

            return null;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GridMap
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "GridMap";
            this.ResumeLayout(false);

        }

        // classify height algorithm v1 -- skeleton and untested.
        public char classifyHeight(string height)
        {
            double heightNum = double.Parse(height);
            int letterIndex = (int)Math.Floor(heightNum / 2);
            if(heightNum < 0)
            {
                Console.WriteLine("Height cannot be a negative number.");
            }
            if(letterIndex > ('Z' - 'A'))
            {
                return 'Z';
            }

            resultChar = (char)('A' + letterIndex);
            
            return resultChar;
        }

        // paint circle depending on x, y
        public void SetCircle(Graphics g)
        {
            Random random = new Random();

            int xrandom = random.Next(1, 6);
            int yrandom = random.Next(1, 6);
            Console.WriteLine("x rand: " + xrandom + "      y rand: " + yrandom);

            int Xpos = (xrandom * (mapScale * gridScale)) - ((mapScale * gridScale) / 2);
            int Ypos = (yrandom * (mapScale * gridScale)) - ((mapScale * gridScale) / 2);

            Console.WriteLine("x pos: " + Xpos + "      y pos: " + Ypos);

            SolidBrush redBrush = new SolidBrush(Color.Red);

            if (redBrush != null)
            {
                // draws position
                g.FillEllipse(redBrush, Xpos, Ypos, 15, 15);
            }
        }

        public static void drawMap(Graphics g)
        {

            int width = mapWidth; // (5*2) = 10 * 30 = 300
            int height = mapHeight; // (5*2) = 10 * 30 = 300
            // 300x300 width and height (BY DEFAULT)


            Pen pen = new Pen(Color.Black);

            //// Left Side
            //PointF point1 = new PointF(0, height);
            //PointF point2 = new PointF(0, 0);
            //g.DrawLine(pen, point1, point2);

            // Top
            PointF point1 = new PointF(0, 0);
            PointF point2 = new PointF(width, 0);
            g.DrawLine(pen, point1, point2);

            //// Right Side
            point1 = new PointF(width, height);
            point2 = new PointF(height, 0);
            g.DrawLine(pen, point1, point2);

            // Bottom
            point1 = new PointF(width, height);
            point2 = new PointF(0, height);
            g.DrawLine(pen, point1, point2);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i % (GridMap.gridScale * mapScale) == 0)
                    {
                        point1 = new PointF(i, 0);
                        point2 = new PointF(i, height);
                        g.DrawLine(pen, point1, point2);

                    }
                    if (j % (GridMap.gridScale * mapScale) == 0)
                    {
                        point1 = new PointF(0, j);
                        point2 = new PointF(width, j);
                        g.DrawLine(pen, point1, point2);

                    }
                }
            }
            
        }
    }

}