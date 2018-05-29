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
        public static readonly Cell TheVoid = new Cell(-1, -1);



        public int Width { get; private set; }
        public int Height { get; private set; }
        public int CellSize { get; private set; }
        public int CellMargin { get; private set; }
        private int CellFullSize => CellSize + CellMargin;

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

            validXPositions = IntRange(cellMargin, width, CellFullSize);
            validYPositions = IntRange(cellMargin, height, CellFullSize);
        }

        public Cell ClaimRandomCell(int retries = 10)
        {
            for (int retry = 0; retry < retries; retry++)
            {
                var xIndex = random.Next(validXPositions.Length);
                var yIndex = random.Next(validYPositions.Length);

                var cellPoint = new Cell(validXPositions[xIndex], validYPositions[yIndex]);
                if (ClaimCell(cellPoint)) return cellPoint;
            }
            return TheVoid;
        }

        public bool ClaimCell(Cell cell)
        {
            if (occupiedCoordinates.Contains(cell.Coordinates)) return false;
            occupiedCoordinates.Add(cell.Coordinates);
            return true;
        }

        public void UnclaimCell(Cell cell)
        {
            occupiedCoordinates.Remove(cell.Coordinates);
        }

        public Cell GetNeighbourCell(Cell cell, Directions direction)
        {
            var coords = cell.Coordinates;
            var neighbourCoords = new Point(coords.X, coords.Y);
            switch (direction)
            {
                case Directions.NORTH:
                    neighbourCoords.Y += CellFullSize;
                    break;
                case Directions.SOUTH:
                    neighbourCoords.Y -= CellFullSize;
                    break;
                case Directions.EAST:
                    neighbourCoords.X += CellFullSize;
                    break;
                case Directions.WEST:
                    neighbourCoords.X -= CellFullSize;
                    break;
            }

            if (neighbourCoords.X < 0 ||
                neighbourCoords.X >= Width ||
                neighbourCoords.Y < 0 ||
                neighbourCoords.Y >= Height)
                return TheVoid;
            return new Cell((int)neighbourCoords.X, (int)neighbourCoords.Y);
        }

        private int[] IntRange(int start, int end, int step)
        {
            var ints = new List<int>();
            for (var i = start; i < end; i += step) { ints.Add(i); }
            return ints.ToArray();
        }

        public class Cell
        {
            public Point Coordinates { get; private set; }

            public Cell(int x, int y)
            {
                Coordinates = new Point(x, y);
            }

            public override bool Equals(object obj)
            {
                return obj is Cell cell &&
                       EqualityComparer<Point>.Default.Equals(Coordinates, cell.Coordinates);
            }

            public override int GetHashCode()
            {
                return -1484672504 + EqualityComparer<Point>.Default.GetHashCode(Coordinates);
            }
        }
    }

    public enum Directions
    {
        NORTH, SOUTH, EAST, WEST
    }

    public static class DirectionsMethods
    {
        public static Directions Opposite(this Directions direction)
        {
            switch (direction)
            {
                case Directions.NORTH:
                    return Directions.SOUTH;
                case Directions.SOUTH:
                    return Directions.NORTH;
                case Directions.EAST:
                    return Directions.WEST;
                default: // WEST
                    return Directions.EAST;
            }
        }

        public static Directions Left(this Directions direction)
        {
            switch (direction)
            {
                case Directions.NORTH:
                    return Directions.WEST;
                case Directions.SOUTH:
                    return Directions.EAST;
                case Directions.EAST:
                    return Directions.NORTH;
                default: // WEST
                    return Directions.SOUTH;
            }
        }

        public static Directions Right(this Directions direction)
        {
            return direction.Left().Opposite();
        }
    }
}
