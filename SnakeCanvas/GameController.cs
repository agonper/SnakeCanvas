using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SnakeCanvas
{
    class GameController
    {
        private static readonly int initialGameSpeedFactor = 1;
        private static readonly int baseGameSpeed = 100;
        private static readonly int minDifficulty = 1;
        private static readonly int maxDifficulty = 10;

        private static readonly int gameObjectSize = 10;
        private static readonly int gameObjectMargin = 2;

        public event Action GameOver;
        public event Action<long> Scored;
        public bool GameStared { get; private set; }
        public long Score { get; private set; }
        public int GameSpeed
        {
            get => _gameSpeed;
            private set
            {
                _gameSpeed = value;
                UpdateGameSpeed(value);
            }
        }
        public int Difficulty {
            get => _difficulty;
            set
            {
                if (value < minDifficulty || value > maxDifficulty) return;
                _difficulty = value;
            }
        }

        private DispatcherTimer dispatchTimer;
        private GameGrid gameGrid;
        private Snake snake;
        private FoodSpawner foodSpawner;
        private int _gameSpeed;
        private int _difficulty;
        

        public GameController(Canvas gameCanvas, int difficulty)
        {
            dispatchTimer = new DispatcherTimer();
            GameSpeed = initialGameSpeedFactor;

            gameGrid = new GameGrid((int)gameCanvas.Width, (int)gameCanvas.Height, gameObjectSize, gameObjectMargin);

            snake = new Snake(gameCanvas, gameGrid);
            snake.Spawn();

            foodSpawner = new FoodSpawner(gameCanvas, gameGrid);
            foodSpawner.SpawnFood();

            Difficulty = difficulty;

            dispatchTimer.Tick += DispatchTimer_Tick; ;
        }

        public void StartGame()
        {
            dispatchTimer.Start();
            GameStared = true;
        }

        public void EndGame()
        {
            dispatchTimer.Stop();
        }

        public void Reset()
        {
            snake.Destroy();
            foodSpawner.RemoveAllFood();
            Score = 0;

            snake.Spawn();
            foodSpawner.SpawnFood();
            GameSpeed = initialGameSpeedFactor;
            StartGame();
        }

        public void LeftArrowPressed()
        {
            snake.SteerLeft();
        }

        public void RightArrowPressed()
        {
            snake.SteerRight();
        }

        private void DispatchTimer_Tick(object sender, EventArgs e)
        {
            var nextHeadPosition = snake.NextHeadPosition();
            var nextIsFood = foodSpawner.IsFood(nextHeadPosition);

            if (nextHeadPosition == GameGrid.TheVoid ||
                (gameGrid.IsOccupied(nextHeadPosition) && !nextIsFood))
            {
                GameOver?.Invoke();
                return;
            }

            if (nextIsFood)
            {
                Score += gameGrid.OccupiedCount;
                GameSpeed = CalculateSpeedBasedOn(Difficulty);
                Scored?.Invoke(Score);
            }

            snake.Move(grow: nextIsFood);
            foodSpawner.SpawnFood();
        }

        private int CalculateSpeedBasedOn(int difficulty)
        {
            return (int)Math.Ceiling(Math.Log(1+Score, 11 - difficulty));
        }

        private void UpdateGameSpeed(int factor)
        {
            var timeBetweenUpdates = baseGameSpeed / factor;
            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, timeBetweenUpdates);
        }
    }
}
