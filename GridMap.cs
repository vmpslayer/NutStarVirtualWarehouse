using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Internal;

namespace Team_308_VirtualWarehouse
{
    public partial class GridMap : Form
    {
        // Fixed grid dimensions
        private const int gridSize = 5;
        private const int width = gridSize;
        private const int height = gridSize;
        private const int gridScale = 2; // Each grid is 2x2 feet

        Brush brush = new SolidBrush(Color.Red);

        // The data structure that will hold the real-world coordinates of the center of each grid.
        private (int x, int y)[,] gridContents;
        // grid name such as: 101, 102 is stored in here with corresponding index i and j
        private string[,] gridNames;
        private string resultGridName;
        // variable to set origin coordinatest
        private (int x, int y) origin;
        private (int x, int y) currentCoordinates;
        double normalizedX, normalizedY;

        bool originisSet;

        private const int MaxCoordinates = 10;
        // average data container
        private List<(int x, int y)> coordinatesBuffer = new List<(int x, int y)>();


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

        // paint circle depending on x, y
        public void SetCircle(Graphics g, int x, int y)
        {
            // NOTE: On use in Form1.cs, Do this:
            // System.Drawing.Graphics formGraphics;
            // formGraphics = this.CreateGraphics();
            int radius = 10;

            if (brush != null)
            {
                brush.Dispose();
                SetCircle(g, x, y);
            }
            else
            {
                // previous:
                // System.Drawing.Drawing2D.GraphicsPath map = new System.Drawing.Drawing2D.GraphicsPath();
                // map.AddEllipse(100, 15, 100, 70);
                // System.Drawing.Region r = new System.Drawing.Region(map);
                // Graphics gr = e.Graphics;
                // gr.FillRegion(Brushes.Red, r);

                // base.OnPaint(e)
                // using var myPen = new Pen(Color.Red);
                // var area = new Rectangle(new Point(x, y), new Size(1, 1))
                // e.Graphics.DrawRectangle(myPen, area);
                g.FillEllipse(brush, new Rectangle(x, y, 2 * radius, 2 * radius));
            }
        }

        // overloading with parser
        public void GetCoordinates()
        {
            Form1 formInstance = new Form1();
            for (int i = 0; i < MaxCoordinates; i++)
            {
                currentCoordinates = formInstance.GetCoordinates();
                // for testing
                Console.WriteLine($"X: {currentCoordinates.x}, Y: {currentCoordinates.y}");
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
            //Form1 formInstance = new();
            Form1 formInstance = new Form1();
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
            GetCoordinates();
            normalizeData();

            int diffX = (int)normalizedX - origin.x;
            int diffY = (int)normalizedY - origin.y;

            // TODO: ratio diffX and diffY to real world coordinate sensitivity


            return (diffX, diffY);
        }

        public string calculateGrid()
        {
            int diffX, diffY;

            if (!originisSet)
            {
                Console.WriteLine("Origin is not set, try again\n");
                return null;
            }

            (diffX, diffY) = calculateLocationDifference();

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    // find the grid location based on first smallest boundry within
                    if (diffX <= gridContents[i, j].x && diffY <= gridContents[i, j].y)
                    {
                        // return grid name if found
                        return gridNames[i, j];
                    }
                }
            }

            return "NULL";
        }

        public static void drawMap(Graphics g)
        {
            if (g != null)
            {
                g.Dispose();
                drawMap(g);
            }
            else
            {
                int width = GridMap.width * gridScale;
                int height = GridMap.height * gridScale;

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
                //point1 = new (width, 0);
                //point2 = new (width, height);
                //g.DrawLine(pen, point1, point2);

                // Bottom
                point1 = new PointF(width, height);
                point2 = new PointF(height, 0);
                g.DrawLine(pen, point1, point2);



                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (i % GridMap.gridScale == 0 && j % GridMap.gridScale == 0)
                        {
                            point1 = new PointF(i, 0);
                            point2 = new PointF(i, height);
                            g.DrawLine(pen, point1, point2);

                            point1 = new PointF(0, j);
                            point2 = new PointF(width, j);
                            g.DrawLine(pen, point1, point2);

                        }
                    }
                }

            }
        }
    }

}

