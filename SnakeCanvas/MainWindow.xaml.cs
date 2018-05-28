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
        private static readonly int gameSpeed = 10;

        private static readonly int gameObjectSize = 10;
        private static readonly int gameObjectMargin = 2;

        DispatcherTimer dispatchTimer;
        FoodSpawner foodSpawner;

        ISet<Point> occupiedPoints = new HashSet<Point>();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dispatchTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, gameSpeed) };
            foodSpawner = new FoodSpawner(GameCanvas, gameObjectSize, gameObjectMargin);

            dispatchTimer.Tick += DispatchTimer_Tick;
        }

        private void DispatchTimer_Tick(object sender, EventArgs e)
        {
            var newFoodPoint = foodSpawner.SpawnFood(occupiedPoints);

            if (newFoodPoint != FoodSpawner.TheVoid)
                occupiedPoints.Add(newFoodPoint);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dispatchTimer.Start();
        }
    }
}
