using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Case2D.Common
{
    //3维向量
    public class Vector3d
    {
        public float x;
        public float y;
        public float z;
        //构造函数
        public Vector3d()
        {
        }
        public Vector3d(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        private Vector3d(Vector3d vet)
        {
            this.x = vet.x;
            this.y = vet.y;
            this.z = vet.z;
        }
        public override string ToString()
        {
            return "{" + x + "," + y + "," + z + "}";
        }

        //向量-向量加减乘除
        public static Vector3d operator +(Vector3d lhs, Vector3d rhs)
        {
            Vector3d vet = new Vector3d(lhs);
            vet.x += rhs.x;
            vet.y += rhs.y;
            vet.z += rhs.z;
            return vet;
        }
        public static Vector3d operator -(Vector3d lhs, Vector3d rhs)
        {
            Vector3d vet = new Vector3d(lhs);
            vet.x -= rhs.x;
            vet.y -= rhs.y;
            vet.z -= rhs.z;
            return vet;
        }
        public static Vector3d operator *(Vector3d lhs, Vector3d rhs)
        {
            Vector3d vet = new Vector3d(lhs);
            vet.x *= rhs.x;
            vet.y *= rhs.y;
            vet.z *= rhs.z;
            return vet;
        }
        public static Vector3d operator /(Vector3d lhs, Vector3d rhs)
        {
            Vector3d vet = new Vector3d(lhs);
            vet.x /= rhs.x;
            vet.y /= rhs.y;
            vet.z /= rhs.z;
            return vet;
        }
        public static Vector3d operator ^(Vector3d lhs, Vector3d rhs)
        {
            return new Vector3d(lhs.y * rhs.z - lhs.z * rhs.y,
                -lhs.x * rhs.z + lhs.z * rhs.x,
                lhs.x * rhs.y - lhs.y * rhs.x);
        }

        //向量-标量乘除
        public static Vector3d operator *(Vector3d lhs, float rhs)
        {
            Vector3d vet = new Vector3d(lhs);
            vet.x *= rhs;
            vet.y *= rhs;
            vet.z *= rhs;
            return vet;
        }
        public static Vector3d operator *(float lhs, Vector3d rhs)
        {
            Vector3d vet = new Vector3d(rhs);
            vet.x *= lhs;
            vet.y *= lhs;
            vet.z *= lhs;
            return vet;
        }
        public static Vector3d operator /(Vector3d lhs, float rhs)
        {
            Vector3d vet = new Vector3d(lhs);
            vet.x /= rhs;
            vet.y /= rhs;
            vet.z /= rhs;
            return vet;
        }
        public static Vector3d operator /(float lhs, Vector3d rhs)
        {
            Vector3d vet = new Vector3d(rhs);
            vet.x /= lhs;
            vet.y /= lhs;
            vet.z /= lhs;
            return vet;
        }

        //向量的模
        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        //向量归一化为单位向量
        public void Normalize()
        {
            float magnitude = Length();
            if (Math.Sign(magnitude) == 0)
            {
                x = y = z = 0;
                return;
            }
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;
        }

        //向量每个单位取反
        public void Reverse()
        {
            x = -x;
            y = -y;
            z = -z;
        }

        public void SetZero()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }


    }

    //2维向量，数据为float类型
    public class Vector2f
    {
        public float x;
        public float y;

        //构造函数
        public Vector2f()
        {
        }
        public Vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;

        }
        public Vector2f(Vector2f vet)
        {
            this.x = vet.x;
            this.y = vet.y;

        }
        public override string ToString()
        {
            return "{" + x + "," + y + "}";
        }

        //向量-向量加减乘除
        public static Vector2f operator +(Vector2f lhs, Vector2f rhs)
        {
            Vector2f vet = new Vector2f(lhs);
            vet.x += rhs.x;
            vet.y += rhs.y;
            return vet;
        }
        public static Vector2f operator -(Vector2f lhs, Vector2f rhs)
        {
            Vector2f vet = new Vector2f(lhs);
            vet.x -= rhs.x;
            vet.y -= rhs.y;

            return vet;
        }
        // 取反,单目运算
        public static Vector2f operator -(Vector2f vec)
        {
            Vector2f v = new Vector2f();
            v.Set(-vec.x, -vec.y);
            return v;
        }
        public static Vector2f operator *(Vector2f lhs, Vector2f rhs)
        {
            Vector2f vet = new Vector2f(lhs);
            vet.x *= rhs.x;
            vet.y *= rhs.y;

            return vet;
        }
        public static Vector2f operator /(Vector2f lhs, Vector2f rhs)
        {
            Vector2f vet = new Vector2f(lhs);
            vet.x /= rhs.x;
            vet.y /= rhs.y;

            return vet;
        }
        /*
        //public static Vector2f operator ^(Vector2f lhs, Vector2f rhs)
        //{
        //    return new Vector2f(lhs.y * rhs.z - lhs.z * rhs.y,
        //        -lhs.x * rhs.z + lhs.z * rhs.x,
        //        lhs.x * rhs.y - lhs.y * rhs.x);
        //}
        */

        //向量-标量乘除
        public static Vector2f operator *(Vector2f lhs, float rhs)
        {
            Vector2f vet = new Vector2f(lhs);
            vet.x *= rhs;
            vet.y *= rhs;

            return vet;
        }
        public static Vector2f operator *(float lhs, Vector2f rhs)
        {
            Vector2f vet = new Vector2f(rhs);
            vet.x *= lhs;
            vet.y *= lhs;

            return vet;
        }
        public static Vector2f operator /(Vector2f lhs, float rhs)
        {
            Vector2f vet = new Vector2f(lhs);
            vet.x /= rhs;
            vet.y /= rhs;

            return vet;
        }
        public static Vector2f operator /(float lhs, Vector2f rhs)
        {
            Vector2f vet = new Vector2f(rhs);
            vet.x /= lhs;
            vet.y /= lhs;

            return vet;
        }

        //向量的模
        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }
        public float LengthSquared()
        {
            return x * x + y * y;
        }

        //向量归一化为单位向量
        public void Normalize()
        {
            float magnitude = Length();
            if (Math.Sign(magnitude) == 0)
            {
                x = y = 0;
                return;
            }
            x /= magnitude;
            y /= magnitude;

        }

        //向量每个单位取反
        public void Reverse()
        {
            x = -x;
            y = -y;

        }
        public void SetZero()
        {
            x = 0.0f;
            y = 0.0f;
        }

        public void Set(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

    }

    //二维矩阵
    public class Mat22
    {
        //两个列向量
        public Vector2f ex = new Vector2f();
        public Vector2f ey = new Vector2f();

        public Mat22()
        {
            ex = new Vector2f();
            ey = new Vector2f();
        }
        public Mat22(float angle)
        {
            ex = new Vector2f();
            ey = new Vector2f();
            float c = (float)Math.Cos(angle);
            float s = (float)Math.Sin(angle);
            ex.x = c;
            ey.x = -s;
            ex.y = s;
            ey.y = c;
        }

        public Mat22 Transpose()
        {
            return new Mat22(new Vector2f(ex.x,ey.x),new Vector2f(ex.y,ey.y));
        }
        /**************************************************************************
        * 功能描述：使用列向量构造这个矩阵
        * 参数说明： c1  ：第一列向量
                     c2  ：第二列向量
        * 返 回 值： (void)
        **************************************************************************/
        public Mat22(Vector2f c1, Vector2f c2)
        {
            ex = c1;
            ey = c2;
        }

        /**************************************************************************
        * 功能描述：使用标量构造这个矩阵
        * 参数说明： a11-a22:四个元素值
        * 返 回 值： (void)
        **************************************************************************/
        public Mat22(float a11, float a12, float a21, float a22)
        {
            ex.x = a11; ex.y = a21;
            ey.x = a12; ey.y = a22;
        }

        /**************************************************************************
        * 功能描述：使用列实例话这个矩阵
        * 参数说明： c1:列向量
                     c2:列向量
        * 返 回 值： (void)
        **************************************************************************/
        void Set(Vector2f c1, Vector2f c2)
        {
            ex = c1;
            ey = c2;
        }
        /**************************************************************************
        * 功能描述：设置成单位矩阵
        * 参数说明： (void)
        * 返 回 值： (void)
        **************************************************************************/
        void SetIdentity()
        {
            ex.x = 1.0f; ey.x = 0.0f;
            ex.y = 0.0f; ey.y = 1.0f;
        }

        /**************************************************************************
        * 功能描述：将该矩阵置0
        * 参数说明： (void)
        * 返 回 值： (void)
        **************************************************************************/
        void SetZero()
        {
            ex.x = 0.0f; ey.x = 0.0f;
            ex.y = 0.0f; ey.y = 0.0f;
        }
        /**************************************************************************
        * 功能描述：获取逆矩阵,
                    逆矩阵B = A的伴随矩阵/A的行列式
                    参照 http://zh.wikipedia.org/wiki/逆矩阵
        * 参数说明： (void)
        * 返 回 值： 逆矩阵
        **************************************************************************/
        public Mat22 GetInverse()
        {
            float a = ex.x, b = ey.x, c = ex.y, d = ey.y;
            Mat22 B = new Mat22();
            float det = a * d - b * c;                   //获取行列式的值                      
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            B.ex.x = det * d; B.ey.x = -det * b;
            B.ex.y = -det * c; B.ey.y = det * a;
            return B;
        }
        /**************************************************************************
        * 功能描述：解决A*x = b,其中b是一个列向量
                    这比一次性求反计算更有效率
        * 参数说明： b : 列向量
        * 返 回 值： 列向量
        **************************************************************************/
        public Vector2f Solve(Vector2f b)

        {
            float a11 = ex.x, a12 = ey.x, a21 = ex.y, a22 = ey.y;
            //获取行列式的值
            float det = a11 * a22 - a12 * a21;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            Vector2f x = new Vector2f();
            //[ a22 -a12] * [b.x] = [a22 * b.x - a12*b.y] 
            //[-a21  a11]   [b.y]   [a11 * b.y - a21*b.x]
            x.x = det * (a22 * b.x - a12 * b.y);
            x.y = det * (a11 * b.y - a21 * b.x);
            return x;
        }


        /**************************************************************************
       * 功能描述： 计算两个2X2矩阵相加
       * 参数说明： a :2X2矩阵
                    b :2X2矩阵
       * 返 回 值： 2X2矩阵
       **************************************************************************/
        public static Mat22 operator +(Mat22 A, Mat22 B)
        {
            return new Mat22(A.ex + B.ex, A.ey + B.ey);
        }

        //矩阵和向量相乘
        public static Vector2f operator *(Mat22 A, Vector2f v)
        {
            return new Vector2f(A.ex.x * v.x + A.ey.x * v.y, A.ex.y * v.x + A.ey.y * v.y);
        }

        public static Mat22 operator *(Mat22 A, Mat22 B)
        {
            return new Mat22(A * B.ex, A * B.ey);
        }

    };

    //三维矩阵
    //3X3矩阵，存储方式以列为主的顺序
    public class Mat33
    {
        public Mat33() { }

        /**************************************************************************
        * 功能描述：使用列向量构造这个矩阵
        * 参数说明： c1  ：第一列向量
                     c2  ：第二列向量
        * 返 回 值： (void)
        **************************************************************************/
        public Mat33(Vector3d c1, Vector3d c2, Vector3d c3)
        {
            ex = c1;
            ey = c2;
            ez = c3;
        }

        /**************************************************************************
        * 功能描述：将该矩阵置0
        * 参数说明： (void)
        * 返 回 值： (void)
        **************************************************************************/
        public void SetZero()
        {
            ex.SetZero();
            ey.SetZero();
            ez.SetZero();
        }

        /**************************************************************************
        * 功能描述：解决A*x = b,其中b是一个三维列向量
                    这比一次性求反计算更有效率
        * 参数说明： b : 三维列向量
        * 返 回 值： 三维列向量
        **************************************************************************/
        public Vector3d Solve33(Vector3d b)
        {
            float det = MyMath.Dot(ex, MyMath.Cross(ey, ez));
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            Vector3d x = new Vector3d();
            x.x = det * MyMath.Dot(b, MyMath.Cross(ey, ez));
            x.y = det * MyMath.Dot(ex, MyMath.Cross(b, ez));
            x.z = det * MyMath.Dot(ex, MyMath.Cross(ey, b));
            return x;

        }
        /**************************************************************************
        * 功能描述：解决A*x = b,其中b是一个二维列向量
                    这比一次性求反计算更有效率
                    仅仅解决2X2的矩阵等式
        * 参数说明： b : 三维列向量
        * 返 回 值： 三维列向量
        **************************************************************************/
        public Vector2f Solve22(Vector2f b)
        {
            float a11 = ex.x, a12 = ey.x, a21 = ex.y, a22 = ey.y;
            float det = a11 * a22 - a12 * a21;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            Vector2f x = new Vector2f();
            x.x = det * (a22 * b.x - a12 * b.y);
            x.y = det * (a11 * b.y - a21 * b.x);
            return x;

        }
        /**************************************************************************
        * 功能描述：获取2X2的逆矩阵,
                    如果出现异常则置为0矩阵
                    逆矩阵B = A的伴随矩阵/A的行列式
        * 参数说明： (void)
        * 返 回 值： (void)
        **************************************************************************/
        public void GetInverse22(ref Mat33 M)
        {
            float a = ex.x, b = ey.x, c = ex.y, d = ey.y;
            float det = a * d - b * c;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            M.ex.x = det * d; M.ey.x = -det * b; M.ex.z = 0.0f;
            M.ex.y = -det * c; M.ey.y = det * a; M.ey.z = 0.0f;
            M.ez.x = 0.0f; M.ez.y = 0.0f; M.ez.z = 0.0f;
        }
        /**************************************************************************
        * 功能描述：获取3X3的对称逆矩阵
                    如果出现异常则置为0矩阵
        * 参数说明： (void)
        * 返 回 值： (void)
        **************************************************************************/
        public void GetSymInverse33(ref Mat33 M)
        {
            float det = MyMath.Dot(ex, MyMath.Cross(ey, ez));
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            float a11 = ex.x, a12 = ey.x, a13 = ez.x;
            float a22 = ey.y, a23 = ez.y;
            float a33 = ez.z;

            M.ex.x = det * (a22 * a33 - a23 * a23);
            M.ex.y = det * (a13 * a23 - a12 * a33);
            M.ex.z = det * (a12 * a23 - a13 * a22);

            M.ey.x = M.ex.y;
            M.ey.y = det * (a11 * a33 - a13 * a13);
            M.ey.z = det * (a13 * a12 - a11 * a23);

            M.ez.x = M.ex.z;
            M.ez.y = M.ey.z;
            M.ez.z = det * (a11 * a22 - a12 * a12);

        }
        //矩阵的3个列向量
        public Vector3d ex, ey, ez;
    };


    //旋转
    public class Rot
    {
        public float s;
        public float c;
        public Rot() { }
        public Rot(float angle)
        {
            s = (float)Math.Sin(angle);
            c = (float)Math.Cos(angle);
        }
        //设置转动角
        public void Set(float angle)
        {
            s = (float)Math.Sin(angle);
            c = (float)Math.Cos(angle);
        }

        public void SetIdentity()
        {
            s = 0.0f;
            c = 1.0f;
        }
        public float GetAngle()
        {
            return (float)Math.Atan2(c, s);
        }

        public Vector2f GetXAxis()
        {
            return new Vector2f(c, s);
        }

        public Vector2f GetYAxis()
        {
            return new Vector2f(-s, c);
        }

    }

    //移动，包括平动和转动
    public class Transform
    {
        public Vector2f p; //平动
        public Rot q; //转动
        public Transform() { }

        /// Initialize using a position vector and a rotation.
        Transform(Vector2f position, Rot rotation)
        {
            p = position;
            q = rotation;
        }
        public void SetIndentity()
        {
            p.SetZero();
            q.SetIdentity();
        }
        public void Set(Vector2f position, float angle)
        {
            p = position;
            q.Set(angle);
        }

    }

    //扫描的实现
    //描述运动的body/shape的TOI(Time of Impact)的计算。
    //形状相对于主体原点定义的，这可能没有一致的原点。
    //但是，为了支持动态我们必须篡改质心位置
    public class Sweep
    {
        public Vector2f localCenter;///< local center of mass position
        public Vector2f c0, c;///< center world positions
        public float a0, a;///< world angles

        /// Fraction of the current time step in the range [0,1]
        /// c0 and a0 are the positions at alpha0.
        public float alpha0;

        /// Get the interpolated transform at a specific time.
        /// @param transform the output transform
        /// @param beta is a factor in [0,1], where 0 indicates alpha0.
       /**************************************************************************
	* 功能描述：在特定时间里获得窜改变换
	* 参数说明： transform  :变换的
	             beta  :一个因子，在[0,1]之间，0表示alpha0
	* 返 回 值： (void)
	**************************************************************************/
        public void GetTransform(ref Transform transform, float beta)
        {
            //p是平动，q是转动
            transform.p = c0 + beta * (c - c0);
            float angle = a0 + beta * (a - a0);
            transform.q.Set(angle);

            //shift to origin
            transform.p -= MyMath.Mul(transform.q, localCenter);
        }

        /// Advance the sweep forward, yielding a new initial state.
        /// @param alpha the new initial time.
    /**************************************************************************
	* 功能描述：推进缓慢向前移动，产生一个新的初始状态
    * 参数说明：alpha:新的初始时间
    * 返 回 值： (void)
    **************************************************************************/

        public void Advance(float alpha)
        {
            Debug.Assert(alpha0 < 1.0f);
            float beta = (alpha - alpha0) / (1.0f - alpha0);
            c0 += beta * (c - c0);
            a0 += beta * (a - a0);
            alpha0 = alpha;

        }

        /// Normalize the angles.
    /**************************************************************************
	/** 功能描述：标准化角度
    * 参数说明： (void)
    * 返 回 值： (void)
    **************************************************************************/
        public void Normalize()
        {
            float twoPi = 2 * (float)Math.PI;
            float d = twoPi * (float)Math.Floor(a0 / twoPi);
            a0 -= d;
            a -= d;
        }

    }
    public static class MyMath
    {
        /**************************************************************************
        * 功能描述：计算向量和标量的叉积
        * 参数说明： a :二维列向量
                     s :标量
        * 返 回 值： 二维列向量
        * 顺时针旋转90度
        **************************************************************************/
        public static Vector2f Cross(Vector2f a, float s)
        {
            return new Vector2f(s * a.y, -s * a.x);
        }
        /**************************************************************************
        * 功能描述：计算向量和向量的叉积
        * 参数说明： a :二维列向量
                     b :二维列向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static float Cross(Vector2f a, Vector2f b)
        {
            return a.x * b.y - a.y * b.x;
        }
        /**************************************************************************
        * 功能描述：计算标量和向量的叉积
        * 参数说明： s :标量
                     a :二维列向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static Vector2f Cross(float s, Vector2f a)
        {
            return new Vector2f(-s * a.y, s * a.x);
        }

        /**************************************************************************
        * 功能描述：一个矩阵乘以一个向量
        * 参数说明： a :2X2矩阵
                     v :二维列向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static Vector2f Mul(Mat22 A, Vector2f v)
        {
            return new Vector2f(A.ex.x * v.x + A.ey.x * v.y, A.ex.y * v.x + A.ey.y * v.y);
        }

        /**************************************************************************
        * 功能描述：一个转置矩阵乘以一个向量
        * 参数说明： a :2X2矩阵
                     v :二维列向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static Vector2f MulT(Mat22 A, Vector2f v)
        {
            return new Vector2f(Dot(v, A.ex), Dot(v, A.ey));
        }

        /**************************************************************************
        * 功能描述： 计算两个向量的点积
        * 参数说明： a :二维列向量
                     b :二维列向量
        * 返 回 值： 点积值
        **************************************************************************/
        public static float Dot(Vector2f a, Vector2f b)
        {
            return a.x * b.x + a.y * b.y;
        }
        /**************************************************************************

        /**************************************************************************
        * 功能描述： 计算两向量之间的距离
        * 参数说明： a :二维列向量
                     b :二维列向量
        * 返 回 值： 距离
        **************************************************************************/
        public static float Distance(Vector2f a, Vector2f b)
        {
            Vector2f c = a - b;
            return c.Length();
        }
        /**************************************************************************
        * 功能描述： 获取两向量之间距离的平方
        * 参数说明： a :二维列向量
                     b :二维列向量
        * 返 回 值： 距离的平方
        **************************************************************************/
        public static float DistanceSquared(Vector2f a, Vector2f b)
        {
            Vector2f c = a - b;
            return Dot(c, c);
        }


        /**************************************************************************
        * 功能描述： 计算两个向量的点积
        * 参数说明： a :三维列向量
                     b :三维列向量
        * 返 回 值： 点积值
        **************************************************************************/
        public static float Dot(Vector3d a, Vector3d b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        /**************************************************************************
        * 功能描述： 计算两个向量的叉积
        * 参数说明： a :三维列向量
                     b :三维列向量
        * 返 回 值： 三维列向量
        **************************************************************************/
        public static Vector3d Cross(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        /**************************************************************************
        * 功能描述： 计算两个2X2矩阵相乘
        * 参数说明： a :2X2矩阵
                     b :2X2矩阵
        * 返 回 值： 2X2矩阵
        **************************************************************************/
        public static Mat22 Mul(Mat22 A, Mat22 B)
        {
            return new Mat22(Mul(A, B.ex), Mul(A, B.ey));
        }

        /**************************************************************************
        * 功能描述： 计算一个转置矩阵和矩阵相乘
        * 参数说明： a :2X2矩阵
                     b :2X2矩阵
        * 返 回 值： 2X2矩阵
        **************************************************************************/
        public static Mat22 MulT(Mat22 A, Mat22 B)
        {
            Vector2f c1 = new Vector2f(Dot(A.ex, B.ex), Dot(A.ey, B.ex));
            Vector2f c2 = new Vector2f(Dot(A.ex, B.ey), Dot(A.ey, B.ey));
            return new Mat22(c1, c2);
        }

        /**************************************************************************
        * 功能描述： 计算一个矩阵和向量相乘
        * 参数说明： a :3X3矩阵
                     v :三维列向量
        * 返 回 值： 三维列向量
        **************************************************************************/
        public static Vector3d Mul(Mat33 A, Vector3d v)
        {
            return v.x * A.ex + v.y * A.ey + v.z * A.ez;
        }

        /**************************************************************************
        * 功能描述： 计算一个矩阵和二维列向量相乘
        * 参数说明： a :3X3矩阵
                     v :二维列向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static Vector2f Mul22(Mat33 A, Vector2f v)
        {
            return new Vector2f(A.ex.x * v.x + A.ey.x * v.y, A.ex.y * v.x + A.ey.y * v.y);
        }
        /**************************************************************************
        * 功能描述： 计算两个旋度的乘积
        * 参数说明： q :旋度
                     r :旋度
        * 返 回 值： 旋度
        **************************************************************************/
        public static Rot Mul(Rot q, Rot r)
        {
            // [qc -qs] * [rc -rs] = [qc*rc-qs*rs -qc*rs-qs*rc]
            // [qs  qc]   [rs  rc]   [qs*rc+qc*rs -qs*rs+qc*rc]
            // s = qs * rc + qc * rs
            // c = qc * rc - qs * rs
            Rot qr = new Rot();
            qr.s = q.s * r.c + q.c * r.s;
            qr.c = q.c * r.c - q.s * r.s;
            return qr;
        }

        /**************************************************************************
        * 功能描述： 计算一个转置旋度和一个旋度的乘积
        * 参数说明： q :旋度
                     r :旋度
        * 返 回 值： 旋度
        **************************************************************************/
        public static Rot MulT(Rot q, Rot r)
        {
            // [ qc qs] * [rc -rs] = [qc*rc+qs*rs -qc*rs+qs*rc]
            // [-qs qc]   [rs  rc]   [-qs*rc+qc*rs qs*rs+qc*rc]
            // s = qc * rs - qs * rc
            // c = qc * rc + qs * rs
            Rot qr = new Rot();
            qr.s = q.c * r.s - q.s * r.c;
            qr.c = q.c * r.c + q.s * r.s;
            return qr;
        }

        /**************************************************************************
        * 功能描述： 旋转一个向量
        * 参数说明： q :旋度
                     v :向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static Vector2f Mul(Rot q, Vector2f v)
        {
            // [qc -qs] * [vx] = [qc*vx-qs*vy qs*vx+qc*vy]
            // [qs  qc]   [vy]   
            return new Vector2f(q.c * v.x - q.s * v.y, q.s * v.x + q.c * v.y);
        }

        /**************************************************************************
        * 功能描述： 反旋转一个向量
        * 参数说明： q :旋度
                     v :向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static Vector2f MulT(Rot q, Vector2f v)
        {
            // [ qc  qs] * [vx] = [qc*vx+qs*vy -qs*vx+qc*vy]
            // [-qs  qc]   [vy]   
            return new Vector2f(q.c * v.x + q.s * v.y, -q.s * v.x + q.c * v.y);
        }
        /**************************************************************************
        * 功能描述： 变换一个向量
        * 参数说明： T :变换
                     v :向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static Vector2f Mul(Transform T, Vector2f v)
        {
            // [qc -qs] * [vx]  = [qc*vx-qs*vy qs*vx+qc*vy]
            // [qs  qc]   [vy]  
            float x = (T.q.c * v.x - T.q.s * v.y) + T.p.x;
            float y = (T.q.s * v.x + T.q.c * v.y) + T.p.y;

            return new Vector2f(x, y);
        }
        /**************************************************************************
        * 功能描述： 反变换一个向量
        * 参数说明： T :变换
                     v :向量
        * 返 回 值： 二维列向量
        **************************************************************************/
        public static Vector2f MulT(Transform T, Vector2f v)
        {
            //( [vx] - [tpx] ) *[ qc qs] = [vx - tpx] *[ qc qs]
            //( [vy]   [tpy] )  [-qs qc]   [vy - tpy]  [-qs qc]
            float px = v.x - T.p.x;
            float py = v.y - T.p.y;
            float x = (T.q.c * px + T.q.s * py);
            float y = (-T.q.s * px + T.q.c * py);

            return new Vector2f(x, y);
        }

        /**************************************************************************
        * 功能描述： 变换一个变换
                     v2 = A.q.Rot(B.q.Rot(v1) + B.p) + A.p    
                     = (A.q * B.q).Rot(v1) + A.q.Rot(B.p) + A.p
        * 参数说明： a :变换
                     b :变换
        * 返 回 值： 变换
        **************************************************************************/
        public static Transform Mul(Transform A, Transform B)
        {
            Transform C = new Transform();
            C.q = Mul(A.q, B.q);
            C.p = Mul(A.q, B.p) + A.p;
            return C;
        }

        /**************************************************************************
        * 功能描述：反变换一个变换
                    v2 = A.q' * (B.q * v1 + B.p - A.p)
                    = A.q' * B.q * v1 + A.q' * (B.p - A.p)
        * 参数说明： a：变换
                     b:变换
        * 返 回 值： 变换
        ***************************************************************************/
        public static Transform MulT(Transform A, Transform B)
        {
            Transform C = new Transform();
            C.q = MulT(A.q, B.q);
            C.p = MulT(A.q, new Vector2f(B.p - A.p));
            return C;
        }

        public static Vector2f Min(Vector2f a, Vector2f b)
        {
            return new Vector2f(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
        }
        public static Vector2f Max(Vector2f a, Vector2f b)
        {
            return new Vector2f(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
        }

        public static Vector2f Abs(Vector2f a)
        {
            return new Vector2f(Math.Abs(a.x), Math.Abs(a.y));
        }
        public static Mat22 Abs(Mat22 A)
        {
            return new Mat22(Abs(A.ex), Abs(A.ey));
        }
        public static float Clamp(float a, float low, float high)
        {
            return Math.Max(low, Math.Min(a, high));
        }
        public static float Sign(float x)
        {
            return x < 0.0f ? -1.0f : 1.0f;
        }
    }
}
