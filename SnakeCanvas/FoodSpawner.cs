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

        private Random random = new Random();

        private Canvas gameCanvas;
        private GameGrid gameGrid;
        private int foodSize;

        public FoodSpawner(Canvas gameCanvas, GameGrid gameGrid)
        {
            this.gameCanvas = gameCanvas;
            this.gameGrid = gameGrid;
            this.foodSize = gameGrid.CellSize;
        }

        public bool SpawnFood()
        {
            Point spawnPoint = gameGrid.ClaimRandomCell();
            if (spawnPoint == GameGrid.TheVoid) return false;
            SpawnFoodAt(spawnPoint);
            return true;
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
    }
}
