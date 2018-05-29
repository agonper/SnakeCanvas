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

        public void UnclaimCell(Point cellCoords)
        {
            occupiedCoordinates.Remove(cellCoords);
        }

        public Point GetNeighbourCell(Point cell, Directions direction)
        {
            Point neighbour = new Point(cell.X, cell.Y);
            switch (direction)
            {
                case Directions.NORTH:
                    neighbour.Y += CellFullSize;
                    break;
                case Directions.SOUTH:
                    neighbour.Y -= CellFullSize;
                    break;
                case Directions.EAST:
                    neighbour.X += CellFullSize;
                    break;
                case Directions.WEST:
                    neighbour.X -= CellFullSize;
                    break;
            }

            if (neighbour.X < 0 ||
                neighbour.X >= Width ||
                neighbour.Y < 0 ||
                neighbour.Y >= Height)
                return TheVoid;
            return neighbour;
        }

        private int[] IntRange(int start, int end, int step)
        {
            var ints = new List<int>();
            for (var i = start; i < end; i += step) { ints.Add(i); }
            return ints.ToArray();
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
