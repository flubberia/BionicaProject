﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
			RectangleF[][][] Animations = new RectangleF[Enum.GetValues(typeof(Sprite.AnimationAction)).Length][][];
			for (int i = 0; i < Enum.GetValues(typeof(Sprite.AnimationAction)).Length; i++)
			{
				switch ((Sprite.AnimationAction)i)
				{
					case Sprite.AnimationAction.Standing:
						int lengthS = 23;

						Animations[i] = new RectangleF[1][];
						Animations[i][1] = new RectangleF[lengthS];
						for (int j = 0; j < lengthS; j++)
						{
							Animations[i][1][j] = new RectangleF(72 * j, 224, 72, 88);
						}
						break;
					case Sprite.AnimationAction.Moving:
						int lengthM = 4;

						Animations[i] = new RectangleF[1][];
						Animations[i][1] = new RectangleF[lengthM];
						Animations[i][1][0] = new RectangleF(0, 320, 72, 88);
						Animations[i][1][1] = new RectangleF(100, 320, 72, 88);
						Animations[i][1][2] = new RectangleF(200, 320, 72, 88);
						Animations[i][1][3] = new RectangleF(10, 320, 72, 88);

						break;
					case Sprite.AnimationAction.Attacking:

						int lengthA = 28;

						Animations[i] = new RectangleF[1][];
						Animations[i][1] = new RectangleF[lengthA];
						for (int j = 0; j < lengthA; j++)
						{
							Animations[i][1][j] = new RectangleF(72 * j, 96, 72, 120);
						}

						break;
					case Sprite.AnimationAction.TakingDamage:
						int lengthT = 13;

						Animations[i] = new RectangleF[1][];
						Animations[i][1] = new RectangleF[lengthT];
						for (int j = 0; j < lengthT; j++)
						{
							Animations[i][1][j] = new RectangleF(72 * j, 0, 72, 88);
						}

						break;
					case Sprite.AnimationAction.Dying:
						break;
				}

			}

			//var gSprite = new Sprite( Animations);





			enemy = new Player(new double[4] { 0.1, 0.9, 0.9, 0.9 });
			Unit humanKnights = new Unit(4, 17, 3, 5, 7, 25, 1.5f);
			Unit humanSoliders = new Unit(4, 16, 2, 4, 4, 30, 1.5f);
			Unit humanArcher = new Unit(4, 12, 5, 4, 3, 20, 10f);
			int n = 4;
			army = new Squad[3 * n];
			for (int i = 0; i < army.Length; i += 3)
			{
				army[i] = new Squad(humanKnights);
				army[i + 1] = new Squad(humanSoliders);
				army[i + 2] = new Squad(humanArcher);
			}
			//for (int i = 0; i < ; i++)
			//{
			//    if (i<army.Length/2)
			//    army[i] = new Squad(humanKnights);
			//    else
			//    army[i] = new Squad(humanSoliders);
			//}

			Stopwatch sw = Stopwatch.StartNew();

			ga = new GA(enemy, army, battleCount: 5, populationSize: 6, mapSize: 32);
			ga.Go();

			sw.Stop();
			System.Windows.MessageBox.Show("Genetic algorithm has finished in " + sw.Elapsed.TotalSeconds.ToString());

			SandBox sb = new SandBox(enemy, ga.GetBest(), army, army, 32) { Visualization = true };
			v = sb.BattleData.Visualization;
			sb.Fight(1);
			v.SetTime(0);
			Battle.Visualization = v;
			Battle.Frame = 0;
		}



		//void HowToCreateSprite()
		//{

		//    RectangleF[][][] Animations = new RectangleF[Enum.GetValues(typeof(Sprite.AnimationAction)).Length][][];
		//    for (int i = 0; i < Enum.GetValues(typeof(Sprite.AnimationAction)).Length; i++)
		//    {
		//        switch ((Sprite.AnimationAction)i)
		//        {
		//            case Sprite.AnimationAction.Standing:

		//                Animations[i] = new RectangleF[1][];
		//                Animations[i][1] = new RectangleF[frames];

		//                Animations[i][1][frame1] = new RectangleF(0, 0, 50, 50);
		//                Animations[i][1][frame2] = new RectangleF(50, 0, 50, 50);
		//                Animations[i][1][frame3] = new RectangleF(100, 0, 50, 50);

		//                break;
		//            case Sprite.AnimationAction.Moving:
		//                break;
		//            case Sprite.AnimationAction.Attacking:
		//                break;
		//            case Sprite.AnimationAction.TakingDamage:
		//                break;
		//            case Sprite.AnimationAction.Dying:
		//                break;
		//        }

		//    }


		//}


		Visualization v;
		//BitmapImage BitmapToImageSource(Bitmap bitmap)
		//{
		//	using (MemoryStream memory = new MemoryStream())
		//	{
		//		bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
		//		memory.Position = 0;
		//		BitmapImage bitmapimage = new BitmapImage();
		//		bitmapimage.BeginInit();
		//		bitmapimage.StreamSource = memory;
		//		bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
		//		bitmapimage.EndInit();

		//		return bitmapimage;
		//	}
		//}
		//Bitmap b;

		private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			double d = TimeSlider.Maximum;
			SubTurnLabel.Content = "Subturn: " + ((int)(e.NewValue * v.BattleLength / TimeSlider.Maximum)).ToString();
			Task.Run(() =>
			{
				Battle.SetTimeAndFrame(
					((int)(e.NewValue * v.BattleLength / d)),
					(float)(e.NewValue * v.BattleLength * 60 / d) % 60);
			});
		}

		private async void Button_Click_1(object sender, RoutedEventArgs e)
		{

			while (TimeSlider.Value < TimeSlider.Maximum)
			{
				TimeSlider.Value += TimeSlider.Maximum * 0.00025;
				await Task.Delay(1);
			}
		}
	}
}
