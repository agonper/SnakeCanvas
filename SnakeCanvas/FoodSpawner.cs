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
        private static readonly int simultaneousFood = 1;

        private readonly Random random = new Random();

        private Canvas gameCanvas;
        private GameGrid gameGrid;
        private int foodSize;

        private Dictionary<GameGrid.Cell, Food> spawnedFood = new Dictionary<GameGrid.Cell, Food>();

        public FoodSpawner(Canvas gameCanvas, GameGrid gameGrid)
        {
            this.gameCanvas = gameCanvas;
            this.gameGrid = gameGrid;
            foodSize = gameGrid.CellSize;
        }

        public void SpawnFood()
        {
            if (spawnedFood.Count >= simultaneousFood) return;

            GameGrid.Cell spawnPoint = gameGrid.ClaimRandomCell();
            if (spawnPoint == GameGrid.TheVoid) return;
            var food = new Food(this, spawnPoint);
            food.Render();
            spawnedFood.Add(food.Cell, food);
        }

        public bool IsFood(GameGrid.Cell cell)
        {
            if (!spawnedFood.ContainsKey(cell)) return false;
            var food = spawnedFood[cell];
            food.Remove();
            return true;
        }

        private class Food
        {
            public Ellipse Shape { get; private set; }
            public GameGrid.Cell Cell { get; private set; }

            private FoodSpawner spawner;

            public Food(FoodSpawner spawner, GameGrid.Cell gridCell)
            {
                Shape = new Ellipse()
                {
                    Height = spawner.foodSize,
                    Width = spawner.foodSize,
                    Fill = Brushes.Crimson,
                    Stroke = Brushes.PeachPuff
                };
                Cell = gridCell;
                this.spawner = spawner;
            }

            public void Render()
            {
                Canvas.SetLeft(Shape, Cell.Coordinates.X);
                Canvas.SetBottom(Shape, Cell.Coordinates.Y);
                spawner.gameCanvas.Children.Add(Shape);
            }

            public void Remove()
            {
                spawner.gameCanvas.Children.Remove(Shape);
                spawner.spawnedFood.Remove(Cell);
            }
        }
    }
}
