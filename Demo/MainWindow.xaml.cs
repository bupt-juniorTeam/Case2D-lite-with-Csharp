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

        private void DrawBody(ref Body body,ref Rectangle rect)
        {
            Mat22 R = new Mat22(body.rotation);
            Vector2f pos = body.position;

            rect.Fill = System.Windows.Media.Brushes.Red;
            rect.Stroke = System.Windows.Media.Brushes.Gray;

            rect.Width = body.width.x;
            rect.Height = body.width.y;

            Canvas.SetLeft(rect, BOX.ActualWidth / 2+pos.x);
            Canvas.SetBottom(rect, pos.y);
        }

        private void DrawJoint(ref Joint joint,ref Line l1,ref Line l2)
        {
            Body b1 = joint.body1;
            Body b2 = joint.body2;

            Mat22 R1 = new Mat22(b1.rotation);
            Mat22 R2 = new Mat22(b2.rotation);

            Vector2f x1 = b1.position;
            Vector2f p1 = x1 + R1 * joint.localAnchor1;

            Vector2f x2 = b1.position;
            Vector2f p2 = x2 + R2 * joint.localAnchor2;

            l1.X1 = x1.x;
            l1.Y1 = x1.y;
            l1.X2 = p1.x;
            l1.Y2 = p1.y;

            l1.X1 = x2.x;
            l1.Y1 = x2.y;
            l1.X2 = p2.x;
            l1.Y2 = p2.y;

            l1.Stroke= System.Windows.Media.Brushes.Blue;

            l2.Stroke = System.Windows.Media.Brushes.Blue;


        }
    }
}
