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
        private static readonly Brush bodyBlockColor = Brushes.LightGreen;
        private static readonly Brush bodyBlockBorderColor = Brushes.YellowGreen;

        private static readonly GameGrid.Cell initialHeadPosition = new GameGrid.Cell(194, 98);
        private static readonly Directions initialDirection = Directions.EAST;
        private static readonly int initialBodyBlocks = 1;

        private Canvas gameCanvas;
        private GameGrid gameGrid;
        private int partSize;

        private BodyPart head;
        private List<BodyPart> bodyBlocks = new List<BodyPart>();
        public Directions direction;

        public Snake(Canvas gameCanvas, GameGrid gameGrid)
        {
            this.gameCanvas = gameCanvas;
            this.gameGrid = gameGrid;
            direction = initialDirection;
            partSize = gameGrid.CellSize;
        }

        public void Spawn()
        {
            direction = initialDirection;
            RenderHeadAt(initialHeadPosition);
            var lastBodyPartPosition = head.Cell;
            for (var bp = 0; bp < initialBodyBlocks; bp++)
            {
                lastBodyPartPosition = gameGrid.GetNeighbourCell(lastBodyPartPosition, initialDirection.Opposite());
                RenderBodyBlockAt(lastBodyPartPosition);
            }
        }

        public void Move(bool grow = false)
        {
            var nextHeadPosition = NextHeadPosition();
            
            if (!grow)
                RemoveLastBodyBlock();

            head.TurnIntoBodyBlock();
            bodyBlocks.Insert(0, head);
            RenderHeadAt(nextHeadPosition);
        }

        public GameGrid.Cell NextHeadPosition()
        {
            return gameGrid.GetNeighbourCell(head.Cell, direction);
        }

        public void SteerLeft()
        {
            direction = direction.Left();
        }

        public void SteerRight()
        {
            direction = direction.Right();
        }

        public void Destroy()
        {
            head.Remove();
            foreach (var block in bodyBlocks)
            {
                block.Remove();
            }
            bodyBlocks.Clear();
        }

        private bool RenderHeadAt(GameGrid.Cell cell)
        {            
            var head = BodyPart.CreateHead(this, cell);
            var rendered = head.Render();
            if (rendered)
                this.head = head;
            return rendered;
        }

        private bool RenderBodyBlockAt(GameGrid.Cell cell)
        {
            var bodyBlock = BodyPart.CreateBodyBlock(this, cell);
            var rendered = bodyBlock.Render();
            if (rendered)
            {
                bodyBlocks.Add(bodyBlock);
            }
            return rendered;
        }

        private void RemoveLastBodyBlock()
        {
            var lastBodyBlock = bodyBlocks.Last();
            bodyBlocks.Remove(lastBodyBlock);
            lastBodyBlock.Remove();
        }

        private class BodyPart
        {
            public Rectangle Shape { get; private set; }
            public GameGrid.Cell Cell { get; private set; }

            private Snake snake;

            public static BodyPart CreateHead(Snake snake, GameGrid.Cell gridCell)
            {
                return new BodyPart(snake, headColor, headBorderColor, gridCell);
            }

            public static BodyPart CreateBodyBlock(Snake snake, GameGrid.Cell gridCell)
            {
                return new BodyPart(snake, bodyBlockColor, bodyBlockBorderColor, gridCell);
            }

            private BodyPart(Snake snake, Brush color, Brush borderColor, GameGrid.Cell gridCell)
            {
                Shape = new Rectangle()
                {
                    Height = snake.partSize,
                    Width = snake.partSize,
                    Fill = color,
                    Stroke = borderColor
                };
                Cell = gridCell;
                this.snake = snake;
            }

            public bool Render()
            {
                if (!snake.gameGrid.ClaimCell(Cell)) return false;
                Canvas.SetLeft(Shape, Cell.Coordinates.X);
                Canvas.SetBottom(Shape, Cell.Coordinates.Y);
                snake.gameCanvas.Children.Add(Shape);
                return true;
            }

            public void Remove()
            {
                snake.gameCanvas.Children.Remove(Shape);
                snake.gameGrid.UnclaimCell(Cell);
            }

            public void TurnIntoBodyBlock()
            {
                Shape.Fill = bodyBlockColor;
                Shape.Stroke = bodyBlockBorderColor;
            }
        }
    }
}
