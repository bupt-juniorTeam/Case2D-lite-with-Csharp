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
        private int now_running = 0;
        private void cmdStart_Clicked_1(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 1;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo1;
            rendering = true;
        }
        private void cmdStart_Clicked_2(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 2;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo2;
            rendering = true;
        }
        private void cmdStart_Clicked_3(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 3;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo3;
            rendering = true;
        }
        private void cmdStart_Clicked_4(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 4;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo4;
            rendering = true;
        }
        private void cmdStart_Clicked_5(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 5;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo5;
            rendering = true;
        }
        private void cmdStart_Clicked_6(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 6;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo6;
            rendering = true;
        }
        private void cmdStart_Clicked_7(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 7;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo7;
            rendering = true;
        }
        private void cmdStart_Clicked_8(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 8;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo8;
            rendering = true;
        }
        private void cmdStart_Clicked_9(object sender, RoutedEventArgs e)
        {
            StopRendering();
            now_running = 9;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            CompositionTarget.Rendering += Demo9;
            rendering = true;
        }
        private void cmdStop_Clicked(object sender, RoutedEventArgs e)
        {
            StopRendering();
        }

        private void StopRendering()
        {
            switch (now_running)
            {
                case 1:
                    CompositionTarget.Rendering -= Demo1;
                    break;
                case 2:
                    CompositionTarget.Rendering -= Demo2;
                    break;
                case 3:
                    CompositionTarget.Rendering -= Demo3;
                    break;
                case 4:
                    CompositionTarget.Rendering -= Demo4;
                    break;
                case 5:
                    CompositionTarget.Rendering -= Demo5;
                    break;
                case 6:
                    CompositionTarget.Rendering -= Demo6;
                    break;
                case 7:
                    CompositionTarget.Rendering -= Demo7;
                    break;
                case 8:
                    CompositionTarget.Rendering -= Demo8;
                    break;
                case 9:
                    CompositionTarget.Rendering -= Demo9;
                    break;
                default:
                    break;
            }
            now_running = 0;
            BOX.Children.Clear();
            numBodies = 0;
            numJoints = 0;
            lines.Clear();
            rects.Clear();
            world.Clear();
            rendering = false;
        }
        const int multiple = 35; // 画图的倍数
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
                b1.Set(new Vector2f(100f, 20f), float.MaxValue);
                b1.position.Set(0.0f, -0.5f * b1.width_height.y);
                b1.rotation = 0.0f;
                ++numBodies;
                world.Add(b1);
                Rectangle rect1 = new Rectangle();
                rect1.Fill = System.Windows.Media.Brushes.Transparent;
                rects.Add(rect1);
                
        
                Body b2 = new Body();
                b2.Set(new Vector2f(1f, 1f), 200f);
                b2.position.Set(0.0f, 4f);
                b2.rotation = 0.0f;
                ++numBodies;
                world.Add(b2);
                Rectangle rect2 = new Rectangle();
                rect2.Fill = System.Windows.Media.Brushes.DeepPink;
                rects.Add(rect2);
        
        
                Body b3 = new Body();
                b3.Set(new Vector2f(1f, 1f), 200f);
                b3.position.Set(0.5f, 8f);
                b3.rotation = 0.0f;
                ++numBodies;
                world.Add(b3);
                Rectangle rect3 = new Rectangle();
                rect3.Fill = System.Windows.Media.Brushes.BlueViolet;
                rects.Add(rect3);
        
                BOX.Children.Add(rect1);
                BOX.Children.Add(rect2);
                BOX.Children.Add(rect3);
                world.Initialize();
            }
            else
            {
                Step();
                times++;
            }
        }
        private void Demo2(object sender, EventArgs e)
        {
            if (numBodies == 0 && numJoints == 0)
            {
                Body b1 = new Body();
                b1.Set(new Vector2f(100.0f, 20.0f), float.MaxValue);
                b1.friction = 0.2f;
                b1.position.Set(0.0f, -0.5f * b1.width_height.y);
                b1.rotation = 0.0f;
                ++numBodies;
                world.Add(b1);
                Rectangle rect1 = new Rectangle();
                rects.Add(rect1);
                

                Body b2 = new Body();
                b2.Set(new Vector2f(1f, 1f), 100f);
                b2.position.Set(9.0f, 11f);
                b2.friction = 0.2f;
                b2.rotation = 0.0f;
                ++numBodies;
                world.Add(b2);
                Rectangle rect2 = new Rectangle();
                rect2.Fill = System.Windows.Media.Brushes.Blue;
                rects.Add(rect2);

                Joint joint = new Joint();
                joint.Set(b1, b2, new Vector2f(0.0f, 11.0f));
                world.Add(joint);
                ++numJoints;


                Line l1 = new Line();
              
                l1.StrokeThickness = 3;
                Line l2 = new Line();
               
              

                lines.Add(l1);
                lines.Add(l2);

                BOX.Children.Add(rect1);
                BOX.Children.Add(rect2);
                BOX.Children.Add(l1);
                BOX.Children.Add(l2);
                world.Initialize();
            }
            else
            {
                times++;
                Step();
            }     
        }
        private void Demo3(object sender, EventArgs e) {
            if (numBodies == 0 && numJoints == 0) {
                Body b;
                Rectangle rect;

                b = new Body();
                b.Set(new Vector2f(100.0f, 20.0f), float.MaxValue);
                b.position.Set(0.0f, -0.5f * b.width_height.y);
                world.Add(b); ++numBodies;
                rect = new Rectangle();
               
                rects.Add(rect); BOX.Children.Add(rect);

                b = new Body();
                b.Set(new Vector2f(13.0f, 0.25f), float.MaxValue);
                b.position.Set(-2.0f, 11.0f);
                b.rotation = -0.25f;
                world.Add(b); ++numBodies;
                rect = new Rectangle();
                rect.Fill = System.Windows.Media.Brushes.Brown;
                rects.Add(rect); BOX.Children.Add(rect);

                b = new Body();
                b.Set(new Vector2f(0.25f, 1.0f), float.MaxValue);
                b.position.Set(5.25f, 9.5f);
                world.Add(b); ++numBodies;
                rect = new Rectangle();
                rect.Fill = System.Windows.Media.Brushes.Brown;
                rects.Add(rect); BOX.Children.Add(rect);

                b = new Body();
                b.Set(new Vector2f(13.0f, 0.25f), float.MaxValue);
                b.position.Set(2.0f, 7.0f);
                b.rotation = 0.25f;
                world.Add(b); ++numBodies;
                rect = new Rectangle();
                rect.Fill = System.Windows.Media.Brushes.Brown;
                rects.Add(rect); BOX.Children.Add(rect);

                b = new Body();
                b.Set(new Vector2f(0.25f, 1.0f), float.MaxValue);
                b.position.Set(-5.25f, 5.5f);
                world.Add(b); ++numBodies;
                rect = new Rectangle();
                rect.Fill = System.Windows.Media.Brushes.Brown;
                rects.Add(rect); BOX.Children.Add(rect);
                

                b = new Body();
                b.Set(new Vector2f(13.0f, 0.25f), float.MaxValue);
                b.position.Set(-2.0f, 3.0f);
                b.rotation = -0.25f;
                world.Add(b); ++numBodies;
                rect = new Rectangle();
                rect.Fill = System.Windows.Media.Brushes.Brown;
                rects.Add(rect); BOX.Children.Add(rect);

                float[] friction = {0.75f,0.5f,0.35f,0.1f,0.0f};
                Body[] B = new Body[5];
                for (int i=0; i<5; i++) {
                    B[i] = new Body();
                    b = B[i];
                    b.Set(new Vector2f(0.5f,0.5f), 25.0f);
                    b.friction = friction[i];
                    b.position.Set(-7.5f + 2.0f*i,14.0f);
                    world.Add(b); ++numBodies;
                    rect = new Rectangle();
                    rect.Fill = System.Windows.Media.Brushes.Green;
                    rects.Add(rect); BOX.Children.Add(rect);
                }
                world.Initialize();
            }
            else
            {
                times++;
                Step();
            }
        }
        private void Demo4(object sender, EventArgs e)
        {
            if (numBodies == 0 && numJoints == 0)
            {
                Body b1 = new Body();
                b1.Set(new Vector2f(100.0f, 20.0f), float.MaxValue);
                b1.friction = 0.2f;
                b1.position.Set(0.0f, -0.5f * b1.width_height.y);
                b1.rotation = 0.0f;
                ++numBodies;
                world.Add(b1);
                Rectangle rect1 = new Rectangle();
                rects.Add(rect1);
                BOX.Children.Add(rect1);

                for (int i = 0; i < 10; ++i)
                {
                    Body b = new Body();
                    b.Set(new Vector2f(1, 1), 1);
                    b.friction = 0.2f;
                    Random random = new Random();
                    double x = random.NextDouble() * 0.2 - 0.1;
                    b.position.Set((float)x, 0.51f + 1.05f * i);
                    world.Add(b);
                    ++numBodies;
                    Rectangle rect = new Rectangle();
                    rects.Add(rect);
                    BOX.Children.Add(rect);
                }
                world.Initialize();
            }
            else
            {
                times++;
                Step();
            }
        }
        private void Demo5(object sender, EventArgs e)
        {
            if (numBodies == 0 && numJoints == 0)
            {
                Body b1 = new Body();
                b1.Set(new Vector2f(100.0f, 20.0f), float.MaxValue);
                b1.friction = 0.2f;
                b1.position.Set(0.0f, -0.5f * b1.width_height.y);
                b1.rotation = 0.0f;
                ++numBodies;
                world.Add(b1);
                Rectangle rect1 = new Rectangle();
                rects.Add(rect1);
                BOX.Children.Add(rect1);

                Vector2f x = new Vector2f(-6, 0.75f);
                Vector2f y = new Vector2f();

                for(int i = 0; i < 12; ++i)
                {
                    y = x;
                    for (int j = i; j < 12; ++j)
                    {
                        Body b = new Body();
                        b.Set(new Vector2f(1, 1), 10);
                        b.friction = 0.2f;
                        b.position = y;
                        world.Add(b);
                        ++numBodies;
                        Rectangle rect = new Rectangle();
                        rects.Add(rect);
                        BOX.Children.Add(rect);
                        y += new Vector2f(1.125f, 0);
                    }
                    x += new Vector2f(0.5625f, 2.0f);
                }
                world.Initialize();
            }
            else
            {
                times++;
                Step();
            }
        }
        private void Demo6(object sender, EventArgs e)
        {
            if (numBodies == 0 && numJoints == 0)
            {
                Body b1 = new Body();
                b1.Set(new Vector2f(100.0f, 20.0f), float.MaxValue);
                b1.position.Set(0.0f, -0.5f * b1.width_height.y);
                ++numBodies;
                world.Add(b1);
                Rectangle rect1 = new Rectangle();
                rects.Add(rect1);

                Body b2 = new Body();
                b2.Set(new Vector2f(12.0f, 0.25f), 100f);
                b2.position.Set(0.0f, 1.0f);
                ++numBodies;
                world.Add(b2);
                Rectangle rect2 = new Rectangle();
                rects.Add(rect2);

                Body b3 = new Body();
                b3.Set(new Vector2f(0.5f, 0.5f), 25);
                b3.position.Set(-5.0f,2f);
                ++numBodies;
                world.Add(b3);
                Rectangle rect3 = new Rectangle();
                rects.Add(rect3);

                Body b4 = new Body();
                b4.Set(new Vector2f(0.5f, 0.5f), 25);
                b4.position.Set(-5.5f, 2f);
                ++numBodies;
                world.Add(b4);
                Rectangle rect4 = new Rectangle();
                rects.Add(rect4);

                Body b5 = new Body();
                b5.Set(new Vector2f(1f, 1f), 100f);
                b5.position.Set(5.5f, 15f);
                ++numBodies;
                world.Add(b5);
                Rectangle rect5 = new Rectangle();
                rects.Add(rect5);

                Joint j = new Joint();
                j.Set(b1, b2, new Vector2f(0f, 1f));
                world.Add(j);
                numJoints++;
                Line l1 = new Line();
                l1.StrokeThickness = 5;
                Line l2 = new Line();
                l2.StrokeThickness = 5;
                lines.Add(l1);
                lines.Add(l2);

                BOX.Children.Add(rect1);
                BOX.Children.Add(rect2);
                BOX.Children.Add(rect3);
                BOX.Children.Add(rect4);
                BOX.Children.Add(rect5);
                BOX.Children.Add(l1);
                BOX.Children.Add(l2);
                world.Initialize();
            }
            else
            {
                times++;
                Step();
            }
        }
        private void Demo7(object sender, EventArgs e)
        {
            if (numBodies == 0 && numJoints == 0)
            {
                Body b1 = new Body();
                b1.Set(new Vector2f(100.0f, 20.0f), float.MaxValue);
                b1.friction = 0.2f;
                b1.position.Set(0.0f, -0.5f * b1.width_height.y);
                b1.rotation = 0.0f;
                world.Add(b1); ++numBodies;
                Rectangle rect1 = new Rectangle();
                rects.Add(rect1); BOX.Children.Add(rect1);

                const int numPlanks = 15;
                float mass = 50.0f;

                // 调谐
                float frequencyHz = 4.0f;  //频率hz
                float dampingRatio = 0.7f; //阻尼率

                // 频率（弧度单位）
                float omega = 2.0f * (float)Math.PI * frequencyHz;

                // 阻尼系数
                float d = 2.0f * mass * dampingRatio * omega;

                // 弹簧刚度
                float k = mass * omega * omega;

                // 神奇公式
                float softness = 1.0f / (d + timeStep * k);
                float biasFactor = timeStep * k / (d + timeStep * k);

                Body[] B = new Body[numPlanks];
                Joint[] J = new Joint[numPlanks + 1];

                Body b2 = b1;
                Body b3 = new Body();
                for (int i = 0; i < numPlanks; i++) {
                    B[i] = new Body();
                    Body b = B[i];
                    b.Set(new Vector2f(1.0f,0.25f),mass);
                    b.friction = 0.2f;
                    b.position.Set(-8.5f + 1.25f*i, 5.0f);
                    world.Add(b); ++numBodies;
                    Rectangle temp = new Rectangle();
                    rects.Add(temp); BOX.Children.Add(temp);

                    b3 = b;
                    J[i] = new Joint();
                    Joint j = J[i];
                    j.Set(b2, b3, new Vector2f(-9.125f + 1.25f * i, 5.0f));
                    j.softness = softness;
                    j.biasFactor = biasFactor;
                    world.Add(j); ++numJoints;
                    Line l1 = new Line();
                    l1.StrokeThickness = 2;
                    Line l2 = new Line();
                    l2.StrokeThickness = 2;
                    lines.Add(l1);
                    lines.Add(l2);
                    BOX.Children.Add(l1);
                    BOX.Children.Add(l2);

                    b2 = b;
                }

                J[numPlanks] = new Joint();
                Joint j1 = J[numPlanks];
                j1.Set(b2, b1, new Vector2f(-9.125f + 1.25f * numPlanks, 5.0f));
                j1.softness = softness;
                j1.biasFactor = biasFactor;
                world.Add(j1);
                world.Add(j1); ++numJoints;
                Line l3 = new Line();
                l3.StrokeThickness = 4;
                Line l4 = new Line();
                l4.StrokeThickness = 4;
                lines.Add(l3);
                lines.Add(l4);
                BOX.Children.Add(l3);
                BOX.Children.Add(l4);
                world.Initialize();
            }
            else
            {
                times++;
                Step();
            }
        }
        private void Demo8(object sender, EventArgs e)
        {
            if (numBodies == 0 && numJoints == 0)
            {
                Body b1 = new Body();
                b1.Set(new Vector2f(100.0f, 20.0f), float.MaxValue);
                b1.position.Set(0.0f,-0.5f*b1.width_height.y);
                world.Add(b1); ++numBodies;
                Rectangle rect1 = new Rectangle();
                rects.Add(rect1); BOX.Children.Add(rect1);

                Body b2 = new Body();
                b2.Set(new Vector2f(12.0f, 0.5f), float.MaxValue);
                b2.position.Set(-1.5f, 10.0f);
                world.Add(b2); ++numBodies;
                Rectangle rect2 = new Rectangle();
                rects.Add(rect2); BOX.Children.Add(rect2);

                Body[] B = new Body[16];
                Body b;
                for (int i=0;i<10;i++) {
                    B[i] = new Body();
                    b = B[i];
                    b.Set(new Vector2f(0.2f,2.0f),10.0f);
                    b.position.Set(-6.0f+1.0f*i,11.125f);
                    b.friction=0.1f;
                    world.Add(b); ++numBodies;
                    Rectangle rect = new Rectangle();
                    rects.Add(rect); BOX.Children.Add(rect);
                }
                Rectangle temp;

                B[10] = new Body();
                b = B[10];
                b.Set(new Vector2f(14.0f, 0.5f), float.MaxValue);
                b.position.Set(1.0f,6.0f);
                b.rotation=0.3f;
                world.Add(b); ++numBodies;
                temp= new Rectangle();
                rects.Add(temp); BOX.Children.Add(temp);

                B[11] = new Body();
                b = B[11];
                b.Set(new Vector2f(0.5f, 3.0f), float.MaxValue);
                b.position.Set(-7.0f,4.0f);
                world.Add(b); ++numBodies;
                temp= new Rectangle();
                rects.Add(temp); BOX.Children.Add(temp);

                B[12] = new Body();
                b = B[12];
                b.Set(new Vector2f(12.0f, 0.25f), 20.0f);
                b.position.Set(-0.9f,1.0f);
                world.Add(b); ++numBodies;
                temp= new Rectangle();
                rects.Add(temp); BOX.Children.Add(temp);

                B[13] = new Body();
                b = B[13];
                b.Set(new Vector2f(0.5f, 0.5f), 10.0f);
                b.position.Set(-10.0f,15.0f);
                world.Add(b); ++numBodies;
                temp= new Rectangle();
                rects.Add(temp); BOX.Children.Add(temp);

                B[14] = new Body();
                b = B[14];
                b.Set(new Vector2f(2.0f, 2.0f), 20.0f);
                b.position.Set(6.0f,2.5f);
                b.friction = 0.1f;
                world.Add(b); ++numBodies;
                temp= new Rectangle();
                rects.Add(temp); BOX.Children.Add(temp);

                B[15] = new Body();
                b = B[15];
                b.Set(new Vector2f(2.0f, 0.2f), 10.0f);
                b.position.Set(6.0f,3.6f);
                world.Add(b); ++numBodies;
                temp= new Rectangle();
                rects.Add(temp); BOX.Children.Add(temp);

                Joint[] J = new Joint[4];
                Joint j;
                Line l1, l2;

                J[0] = new Joint();
                j = J[0];
                j.Set(b1, B[12], new Vector2f(-2.0f,1.0f));
                world.Add(j); ++numJoints;
                l1 = new Line();
                l2 = new Line();
                lines.Add(l1);
                lines.Add(l2);
                BOX.Children.Add(l1);
                BOX.Children.Add(l2);

                J[1] = new Joint();
                j = J[1];
                j.Set(B[11], B[13], new Vector2f(-7.0f,15.0f));
                world.Add(j); ++numJoints;
                l1 = new Line();
                l2 = new Line();
                lines.Add(l1);
                lines.Add(l2);
                BOX.Children.Add(l1);
                BOX.Children.Add(l2);

                J[2] = new Joint();
                j = J[2];
                j.Set(b1, B[14], new Vector2f(6.0f,2.6f));
                world.Add(j); ++numJoints;
                l1 = new Line();
                l2 = new Line();
                lines.Add(l1);
                lines.Add(l2);
                BOX.Children.Add(l1);
                BOX.Children.Add(l2);

                J[3] = new Joint();
                j = J[3];
                j.Set(B[14], B[15], new Vector2f(7.0f,3.5f));
                world.Add(j); ++numJoints;
                l1 = new Line();
                l2 = new Line();
                lines.Add(l1);
                lines.Add(l2);
                BOX.Children.Add(l1);
                BOX.Children.Add(l2);
                world.Initialize();
            }
            else
            {
                times++;
                Step();
            }
        }
        private void Demo9(object sender, EventArgs e)
        {
            if (numBodies == 0 && numJoints == 0)
            {
                Body b1 = new Body();
                b1.Set(new Vector2f(100.0f, 20.0f), float.MaxValue);
                b1.friction = 0.2f;
                b1.position.Set(0.0f, -0.5f * b1.width_height.y);
                b1.rotation = 0.0f;
                ++numBodies;
                world.Add(b1);
                Rectangle rect1 = new Rectangle();
                rects.Add(rect1);
                BOX.Children.Add(rect1);
                
                float mass = 100f;

                // 调谐
                float frequencyHz = 4.0f;  //频率hz
                float dampingRatio = 0.7f; //阻尼率

                // 频率（弧度单位）
                float omega = 2.0f * (float)Math.PI * frequencyHz;

                // 阻尼系数
                float d = 2.0f * mass * dampingRatio * omega;

                // 弹簧刚度
                float k = mass * omega * omega;

                // 神奇公式
                float softness = 1.0f / (d + timeStep * k);
                float biasFactor = timeStep * k / (d + timeStep * k);

                float y = 12.0f;
                const int count = 15;
                Body[] B = new Body[count];
                Joint[] J = new Joint[count];
                Body b = b1;
                for(int i = 0; i < count; ++i)
                {
                    Vector2f x = new Vector2f(0.5f+i,y);
                    B[i] = new Body();
                    B[i].Set(new Vector2f(0.75f,0.25f), mass);
                    B[i].friction = 0.1f;
                    B[i].position = x;
                    B[i].rotation = 0.0f;
                    world.Add(B[i]);
                    ++numBodies;
                    Rectangle temp = new Rectangle();
                    rects.Add(temp);
                    BOX.Children.Add(temp);

                    J[i] = new Joint();
                    J[i].Set(b, B[i], new Vector2f((float)i, y));
                    J[i].softness = softness;
                    J[i].biasFactor = biasFactor;
                    world.Add(J[i]);
                    ++numJoints;
                    Line l1 = new Line();
                    l1.StrokeThickness = 2;
                    Line l2 = new Line();
                    l2.StrokeThickness = 2;
                    lines.Add(l1);
                    lines.Add(l2);
                    BOX.Children.Add(l1);
                    BOX.Children.Add(l2);
                    b = B[i];
                }
                world.Initialize();
            }
            else
            {
                times++;
                Step();
            }
        }
        
        List<Ellipse> ellipses = new List<Ellipse>();
        private void Step()
        {
            world.Step(timeStep);
            ellipses.Clear();
            for (int i = 0; i < numJoints; ++i)
            {
                DrawJoint(world.joints[i], lines[2 * i], lines[2 * i + 1]);
            }
            for (int i = 0; i < numBodies; ++i)
            {
                DrawBody(world.bodies[i], rects[i]);
            }
   
        }

        private void DrawBody(Body body, Rectangle rect)
        {
            Mat22 R = new Mat22(body.rotation);
            Vector2f x = body.position * multiple;
            Vector2f h = 0.5f * body.width_height * multiple;
            Vector2f pos = body.position * multiple;

            if (body.invMass == 0)
            {
                //rect.Stroke = System.Windows.Media.Brushes.Yellow;
                rect.Fill = new SolidColorBrush(Color.FromRgb(173, 213, 162));
                //rect.Fill = System.Windows.Media.Brushes.Yellow;
            }
            else
            {
                rect.Fill = new SolidColorBrush(Color.FromRgb(99, 187, 208));
            }
            //rect.Stroke = System.Windows.Media.Brushes.Yellow;

            rect.Width = body.width_height.x * multiple;
            rect.Height = body.width_height.y * multiple;
            
            Canvas.SetLeft(rect, BOX.Width / 2 + pos.x - rect.Width/2);
            Canvas.SetBottom(rect, pos.y-rect.Height/2);

            RotateTransform rotate = new RotateTransform(-body.rotation*180/Math.PI,rect.Width/2,rect.Height/2);
            rect.RenderTransform = rotate;
        }
        private Vector2f transf(Vector2f p)
        {
            return new Vector2f(p.x+480 , -p.y+480);
       
        }
        private void DrawJoint(Joint joint, Line l1, Line l2)
        { 
            Body b1 = joint.body1;
            Body b2 = joint.body2;

            Mat22 R1 = new Mat22(b1.rotation);
            Mat22 R2 = new Mat22(b2.rotation);

            Vector2f x1 = b1.position;
            Vector2f p1 = x1 + R1 * joint.localAnchor1;

            Vector2f x2 = b2.position;
            Vector2f p2 = x2 + R2 * joint.localAnchor2;

            x1 = transf(x1 * multiple);
            p1 = transf(p1 * multiple);
            x2 = transf(x2 * multiple);
            p2 = transf(p2 * multiple);
/*            x1 *= multiple;
            x2 *= multiple;
            p1 *= multiple;
            p2 *= multiple;
*/
            l1.X1 = p1.x;
            l1.Y1 = p1.y;
            l1.X2 = x1.x;
            l1.Y2 = x1.y;

            l2.X1 = p2.x;
            l2.Y1 = p2.y;
            l2.X2 = x2.x;
            l2.Y2 = x2.y;
            l1.Stroke = new SolidColorBrush(Color.FromRgb(128, 118, 163));
          
            l2.Stroke = new SolidColorBrush(Color.FromRgb(128, 118, 163));

            /*l1.Stroke = System.Windows.Media.Brushes.Yellow;
            l2.Stroke = System.Windows.Media.Brushes.Yellow;*/
            /* l1.HorizontalAlignment = HorizontalAlignment.Left;
             l1.VerticalAlignment = VerticalAlignment.Center;

             l2.HorizontalAlignment = HorizontalAlignment.Left;
             l2.VerticalAlignment = VerticalAlignment.Center;*/

            /*Canvas.SetLeft(l1, BOX.Width / 2);
           
            Canvas.SetLeft(l2, BOX.Width / 2);
           */


        }
    }
}
