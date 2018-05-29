using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeCanvas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly int gameSpeed = 100;

        private static readonly int gameObjectSize = 10;
        private static readonly int gameObjectMargin = 2;

        DispatcherTimer dispatchTimer;
        GameGrid gameGrid;
        Snake snake;
        FoodSpawner foodSpawner;

        int its = 0;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dispatchTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, gameSpeed) };
            gameGrid = new GameGrid((int)GameCanvas.Width, (int)GameCanvas.Height, gameObjectSize, gameObjectMargin);

            snake = new Snake(GameCanvas, gameGrid);
            snake.Spawn();

            foodSpawner = new FoodSpawner(GameCanvas, gameGrid);
            foodSpawner.SpawnFood();

            dispatchTimer.Tick += DispatchTimer_Tick;
        }

        private void DispatchTimer_Tick(object sender, EventArgs e)
        {
            if (its % 7 == 0)
            {
                snake.SteerLeft();
            }

            if (its % 11 == 0)
            {
                snake.SteerRight();
            }

            snake.Move();
            foodSpawner.SpawnFood();
            GameInfo.Text = string.Format("Celdas restantes: {0}", gameGrid.RemainingCellCount);
            its++;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dispatchTimer.Start();
        }
    }
}
