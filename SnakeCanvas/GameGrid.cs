using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeCanvas
{
    class GameGrid
    {
        public static readonly Point TheVoid = new Point(-1, -1);



        public int Width { get; private set; }
        public int Height { get; private set; }
        public int CellSize { get; private set; }
        public int CellMargin { get; private set; }

        public int CellCount => validXPositions.Length * validYPositions.Length;
        public int RemainingCellCount => CellCount - occupiedCoordinates.Count;

        Random random = new Random();
        ISet<Point> occupiedCoordinates = new HashSet<Point>();

        int[] validXPositions;
        int[] validYPositions;

        public GameGrid(int width, int height, int cellSize, int cellMargin)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            CellMargin = cellMargin;

            int cellFullSize = cellSize + cellMargin;
            validXPositions = IntRange(cellMargin, width, cellFullSize);
            validYPositions = IntRange(cellMargin, height, cellFullSize);
        }

        public Point ClaimRandomCell(int retries = 10)
        {
            for (int retry = 0; retry < retries; retry++)
            {
                var xIndex = random.Next(validXPositions.Length);
                var yIndex = random.Next(validYPositions.Length);

                var cellPoint = new Point() { X = validXPositions[xIndex], Y = validYPositions[yIndex] };
                if (ClaimCell(cellPoint)) return cellPoint;
            }
            return TheVoid;
        }

        public bool ClaimCell(Point cellCoords)
        {
            if (occupiedCoordinates.Contains(cellCoords)) return false;
            occupiedCoordinates.Add(cellCoords);
            return true;
        }

        private int[] IntRange(int start, int end, int step)
        {
            var ints = new List<int>();
            for (var i = start; i < end; i += step) { ints.Add(i); }
            return ints.ToArray();
        }
    }


}
