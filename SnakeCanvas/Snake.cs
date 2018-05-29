using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeCanvas
{
    class Snake
    {
        private static readonly Brush headColor = Brushes.RosyBrown;
        private static readonly Brush headBorderColor = Brushes.PaleVioletRed;
        private static readonly Brush bodyColor = Brushes.LightGreen;
        private static readonly Brush bodyBorderColor = Brushes.YellowGreen;

        private static readonly Point initialHeadPosition = new Point() { X = 194, Y = 98 };
        private static readonly Directions initialDirection = Directions.EAST;
        private static readonly int initialBodyParts = 4;

        public Point HeadPosition { get; private set; }
        public Directions Direction { get; private set; }

        private Canvas gameCanvas;
        private GameGrid gameGrid;
        private int partSize;

        private Rectangle head;
        private List<Rectangle> bodyParts = new List<Rectangle>();
        private List<Point> bodyPartsCells = new List<Point>();

        public Snake(Canvas gameCanvas, GameGrid gameGrid)
        {
            this.gameCanvas = gameCanvas;
            this.gameGrid = gameGrid;
            Direction = initialDirection;
            partSize = gameGrid.CellSize;
        }

        public void Spawn()
        {
            RenderHeadAt(initialHeadPosition);
            var lastBodyPartPosition = HeadPosition;
            for (var bp = 0; bp < initialBodyParts; bp++)
            {
                lastBodyPartPosition = gameGrid.GetNeighbourCell(lastBodyPartPosition, initialDirection.Opposite());
                RenderBodPartyAt(lastBodyPartPosition);
            }
        }

        public void Move(bool grow = false)
        {
            var nextHeadPosition = NextHeadPosition();
            
            if (!grow)
                RemoveLastBodyPart();

            TransformHeadIntoBodyPart();
            RenderHeadAt(nextHeadPosition);
        }

        public Point NextHeadPosition()
        {
            return gameGrid.GetNeighbourCell(HeadPosition, Direction);
        }

        public void SteerLeft()
        {
            Direction = Direction.Left();
        }

        public void SteerRight()
        {
            Direction = Direction.Right();
        }

        private bool RenderHeadAt(Point cell)
        {            
            var head = CreatePart(headColor, headBorderColor);
            var rendered = RenderPartAt(cell, head);
            if (rendered)
            {
                HeadPosition = cell;
                this.head = head;
            }
            return rendered;
        }

        private bool RenderBodPartyAt(Point cell)
        {
            var bodyPart = CreatePart(bodyColor, bodyBorderColor);
            var rendered = RenderPartAt(cell, bodyPart);
            if (rendered)
            {
                bodyParts.Add(bodyPart);
                bodyPartsCells.Add(cell);
            }
            return rendered;
        }

        private bool RenderPartAt(Point cell, Rectangle part)
        {
            if (!gameGrid.ClaimCell(cell)) return false;
            Canvas.SetLeft(part, cell.X);
            Canvas.SetBottom(part, cell.Y);
            gameCanvas.Children.Add(part);
            return true;
        }

        private Rectangle CreatePart(Brush color, Brush borderColor)
        {
            return new Rectangle()
            {
                Height = partSize,
                Width = partSize,
                Fill = color,
                Stroke = borderColor
            };
        }

        private void TransformHeadIntoBodyPart()
        {
            head.Fill = bodyColor;
            head.Stroke = bodyBorderColor;
            bodyParts.Insert(0, head);
            bodyPartsCells.Insert(0, HeadPosition);
        }

        private void RemoveLastBodyPart()
        {
            var lastBodyPart = bodyParts.Last();
            bodyParts.Remove(lastBodyPart);

            gameCanvas.Children.Remove(lastBodyPart);

            gameGrid.UnclaimCell(bodyPartsCells.Last());
            bodyPartsCells.Remove(bodyPartsCells.Last());
        }
    }
}
