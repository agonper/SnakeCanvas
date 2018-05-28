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
    class FoodSpawner
    {
        public static readonly Point TheVoid = new Point(-1, -1);

        private Random random = new Random();

        private Canvas gameCanvas;
        private int foodSize;

        private int[] validXPositions;
        private int[] validYPositions;

        public FoodSpawner(Canvas gameCanvas, int foodSize, int margin)
        {
            this.gameCanvas = gameCanvas;
            this.foodSize = foodSize;

            validXPositions = IntRange(margin, (int)gameCanvas.Width, foodSize + margin);
            validYPositions = IntRange(margin, (int)gameCanvas.Height, foodSize + margin);
        }

        public Point SpawnFood(ISet<Point> forbiddenPoints, int spawnRetries=10)
        {
            for (int retry = 0; retry < spawnRetries; retry++)
            {
                var xIndex = random.Next(validXPositions.Length);
                var yIndex = random.Next(validYPositions.Length);

                var spawnPoint = new Point() { X = validXPositions[xIndex], Y = validYPositions[yIndex] };
                if (!forbiddenPoints.Contains(spawnPoint))
                {
                    SpawnFoodAt(spawnPoint);
                    return spawnPoint;
                }
            }
            return TheVoid;
        }

        private void SpawnFoodAt(Point point)
        {
            var food = new Ellipse()
            {
                Height = foodSize,
                Width = foodSize,
                Fill = Brushes.Crimson,
                Stroke = Brushes.PeachPuff
            };

            Canvas.SetLeft(food, point.X);
            Canvas.SetBottom(food, point.Y);

            gameCanvas.Children.Add(food);
        }

        private int[] IntRange(int start, int end, int step)
        {
            var ints = new List<int>();
            for (var i = start; i < end; i += step) { ints.Add(i); }
            return ints.ToArray();
        }
    }
}
