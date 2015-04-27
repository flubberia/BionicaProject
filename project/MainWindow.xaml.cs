﻿using System.Diagnostics;
using System.Drawing;
using System.Windows;

namespace project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GA ga;
        Player enemy;
        Squad[] army;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            enemy = new Player(new double[4]{0.9,0.1,0.1,0.9});
            Unit humanKnights = new Unit( 4, 17, 3, 5, 13, 25, 1.5f);
            Unit humanSoliders = new Unit( 4, 16, 2, 4, 7, 30, 1.5f);
            int n = 6;
            army = new Squad[2*n];
            for (int i = 0; i < army.Length; i++)
            {
                if (i<army.Length/2)
                army[i] = new Squad(humanKnights);
                else
                army[i] = new Squad(humanSoliders);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
           

            ga = new GA(enemy, army, battleCount: 10,populationSize:10,generationSize:10);
            ga.Go();

            sw.Stop();
            System.Windows.MessageBox.Show(sw.Elapsed.TotalSeconds.ToString());

			SandBox sb = new SandBox(enemy, ga.GetBest(), army, army, 64) { Visualization = true };
			var v = sb.BattleData.Visualization;
			sb.Fight(0);

			//Bitmap b = new Bitmap((int)PictureBox.Width, (int)PictureBox.Height);
			//using (Graphics g = Graphics.FromImage(b))
			//{
			//	v.DrawFrame(g, b.Width, b.Height, 0);
			//}
			//PictureBox.DataContext = b;
                        
        }
    }
}
