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
using Case2D_lite;
using Case2D.Common;
namespace Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool rendering = false;

        private void cmdStart_Clicked(object sender, RoutedEventArgs e)
        {
            if (!rendering)
            {
                BOX.Children.Clear();
                numBodies = 0;
                numJoints = 0;

                CompositionTarget.Rendering += Demo1;
                rendering = true;
            }
        }
        private void cmdStop_Clicked(object sender, RoutedEventArgs e)
        {
            StopRendering();
        }

        private void StopRendering()
        {
            CompositionTarget.Rendering -= Demo1;
            rendering = false;
        }
        const float timeStep = 1.0f/60.0f;
        const int iterations = 10;
        static Vector2f gravity = new Vector2f(0.0f,-10.0f);
        
        int numBodies = 0;
        int numJoints = 0;

        int demoIndex = 0;

        const int width = 960;
        const int height = 480;
        World world = new World(gravity, iterations);
        private void Demo1(object sender, EventArgs e)
        {
            Body b1 = new Body();
            b1.Set(new Vector2f(100f, 20f), float.MaxValue);
            b1.position.Set(0.0f, -0.5f * b1.width.y);
            Body b2 = new Body();
            b2.Set(new Vector2f(1f, 1f), 200f);
        }
    }
}
