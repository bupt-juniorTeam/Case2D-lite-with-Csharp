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
using System.Diagnostics;

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
                lines.Clear();
                rects.Clear();
                world.Clear();
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
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            rendering = false;
        }

        const float timeStep = 1.0f/60.0f;
        const int iterations = 10;
        static Vector2f gravity = new Vector2f(0.0f, -10.0f);
        static int times = 0;

        int numBodies = 0;
        int numJoints = 0;

        int demoIndex = 0;

        const int width = 960;
        const int height = 480;
        World world = new World(gravity, iterations);
        List<Rectangle> rects = new List<Rectangle>();
        List<Line> lines = new List<Line>();

        private void Demo1(object sender, EventArgs e)
        {
            if (numBodies == 0)
            {
                Body b1 = new Body();
                b1.Set(new Vector2f(1000f, 20f), float.MaxValue);
                b1.position.Set(0.0f, -0.5f * b1.width.y+10);
                b1.rotation = 0.0f;
                ++numBodies;
                world.Add(b1);
                Rectangle rect1 = new Rectangle();
                rects.Add(rect1);
                

                Body b2 = new Body();
                b2.Set(new Vector2f(20f, 20f), float.MaxValue);
                b2.position.Set(0.0f, 20f);
                b2.rotation = 0.0f;
                ++numBodies;
                world.Add(b2);
                Rectangle rect2 = new Rectangle();
                rects.Add(rect2);


                Body b3 = new Body();
                b3.Set(new Vector2f(20f, 20f), 20f);
                b3.position.Set(10f, 80f);
                b3.rotation = 0.0f;
                ++numBodies;
                world.Add(b3);
                Rectangle rect3 = new Rectangle();
                rects.Add(rect3);

                BOX.Children.Add(rect1);
                BOX.Children.Add(rect2);
                BOX.Children.Add(rect3);
            }
            else
            {
                Step();
                times++;
            }
        }

        List<Ellipse> ellipses = new List<Ellipse>();
        private void Step()
        {
            // test
            // 调用次数
            System.Console.WriteLine("**********************************times: " + times + ": ");

            world.Step(timeStep);
            ellipses.Clear();

            //int count = 0;
            //foreach (var dic in world.arbiters)
            //{
            //    Arbiter arbiter = dic.Value;
            //    for (int i = 0; i < arbiter.numContacts; ++i)
            //    {
            //        count++;
            //    }
            //}
            // Console.WriteLine(count);

            for (int i = 0; i < numBodies; ++i)
            {
                DrawBody(world.bodies[i], rects[i]);
            }
            for (int i = 0; i < numJoints; ++i)
            {
                DrawJoint(world.joints[i], lines[i], lines[i+1]);
            }
        }

        private void DrawBody(Body body, Rectangle rect)
        {
            Mat22 R = new Mat22(body.rotation);
            Vector2f x = body.position;
            Vector2f h = 0.5f * body.width;

            Vector2f v1 = x + R * new Vector2f(-h.x, -h.y);
            Vector2f v2 = x + R * new Vector2f(h.x, -h.y);
            Vector2f v3 = x + R * new Vector2f(h.x, h.y);
            Vector2f v4 = x + R * new Vector2f(-h.x, h.y);
            
            Vector2f pos = body.position;

            rect.Stroke = System.Windows.Media.Brushes.White;

            rect.Width = body.width.x;
            rect.Height = body.width.y;
            
            Canvas.SetLeft(rect, BOX.Width / 2 + pos.x - rect.Width/2);
            Canvas.SetBottom(rect, pos.y-rect.Height/2);
            RotateTransform rotate = new RotateTransform(-body.rotation*180,rect.Width/2,rect.Height/2);
            rect.RenderTransform = rotate;
        }

        private void DrawJoint(Joint joint, Line l1, Line l2)
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

            l1.Stroke = System.Windows.Media.Brushes.Blue;

            l2.Stroke = System.Windows.Media.Brushes.Blue;
        }
    }
}
