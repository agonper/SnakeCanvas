﻿using System;
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
        private static readonly int gameDifficulty = 2;

        GameController gameController;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            gameController = new GameController(GameCanvas, gameDifficulty);
            gameController.GameOver += GameController_GameOver;
            gameController.Scored += GameController_Scored;

            GameCanvas.Focus();
            GameCanvas.KeyDown += GameCanvas_KeyDown;
        }

        private void GameController_GameOver()
        {
            GameInfo.Text = string.Format("¡Lástima! - Puntuación: {0}", gameController.Score);
            MainButton.Content = "Reintentar";
            MainButton.IsEnabled = true;
            gameController.EndGame();
        }

        private void GameController_Scored(long score)
        {
            UpdateScorePanel(score);
        }

        private void GameCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                gameController.LeftArrowPressed();
            if (e.Key == Key.Right)
                gameController.RightArrowPressed();
            GameCanvas.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!gameController.GameStared) gameController.StartGame();
            else gameController.Reset();

            UpdateScorePanel();
            MainButton.IsEnabled = false;
            GameCanvas.Focus();
        }

        private void UpdateScorePanel(long score = 0)
        {
            GameInfo.Text = string.Format("Puntuación: {0} - Velocidad: x{1}", score, gameController.GameSpeed);
        }
    }
}
