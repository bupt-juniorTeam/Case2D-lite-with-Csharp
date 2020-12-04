using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case2D.Common;
// Box vertex and edge numbering:
//
//        ^ y
//        |
//        e1
//   v2 ------ v1
//    |        |
// e2 |        | e4  --> x
//    |        |
//   v3 ------ v4
//        e3

namespace Case2D_lite {
    class Collision {
        enum Axis {
            FACE_A_X,
            FACE_A_Y,
            FACE_B_X,
            FACE_B_Y
        };
        enum EdgeNumbers {
            NO_EDGE = 0,
            EDGE1,
            EDGE2,
            EDGE3,
            EDGE4
        };
        public class ClipVertex {
            public ClipVertex() { fp.value = 0; }
            public Vector2f v=new Vector2f();
            public FeaturePair fp=new FeaturePair();
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fp"></param>
        public static void Flip(FeaturePair fp)
        {
            var temp = fp.e.inEdge1;
            fp.e.inEdge1 = fp.e.inEdge2;
            fp.e.inEdge2 = temp;

            temp = fp.e.outEdge1;
            fp.e.outEdge1 = fp.e.outEdge2;
            fp.e.outEdge2 = temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vOut">传出的在参考边后面的端点</param>
        /// <param name="vIn">传入的两个端点</param>
        /// <param name="normal">参考边的法向量</param>
        /// <param name="offset">距离等于offset看作接触</param>
        /// <param name="clipEdge">参考边的编号</param>
        /// <returns>输出碰撞的点数</returns>
        public static int ClipSegmentToLine(ref ClipVertex[] vOut, ref ClipVertex[] vIn,Vector2f normal, float offset, char clipEdge)
        {
            int numOut = 0;
            // 计算端点到直线的距离
            float distance0 = MyMath.Dot(normal, vIn[0].v) - offset;
            float distance1 = MyMath.Dot(normal, vIn[1].v) - offset;
            // 如果端点在直线的后面就存进vOut
            if (distance0 <= 0.0f) vOut[numOut++] = vIn[0];
            if (distance1 <= 0.0f) vOut[numOut++] = vIn[1];
            // 如果端点在直线的两边
            if(distance0*distance1 < 0.0f)
            {
                // 找到两条直线的交点的比例
                float interp = distance0 / (distance0 - distance1);
                // 把交点存进vOut
                vOut[numOut].v = vIn[0].v + interp * (vIn[1].v - vIn[0].v);
                // 第一个端点在直线上方
                if(distance0 > 0.0f)
                {
                    vOut[numOut].fp = vIn[0].fp;
                    vOut[numOut].fp.e.inEdge1 = clipEdge;
                    vOut[numOut].fp.e.inEdge2 = (char)EdgeNumbers.NO_EDGE;
                }
                // 第二个端点在直线上方
                else
                {
                    vOut[numOut].fp = vIn[1].fp;
                    vOut[numOut].fp.e.outEdge1 = clipEdge;
                    vOut[numOut].fp.e.outEdge2 = (char)EdgeNumbers.NO_EDGE;
                }
                ++numOut;
            }
            return numOut;
        }
        /// <summary>
        /// 计算候选点6
        /// </summary>
        /// <param name="c">两个碰撞点</param>
        /// <param name="h"></param>
        /// <param name="pos"></param>
        /// <param name="Rot"></param>
        /// <param name="normal">此法向量为参考面的法向量</param>
        public static void ComputeIncidentEdge(ref ClipVertex[] c,Vector2f h, Vector2f pos,Mat22 Rot,Vector2f normal)
        {
            Mat22 RotT = Rot.Transpose();
            Vector2f n = -(RotT * normal);
            Vector2f nAbs = MyMath.Abs(n);

            if (nAbs.x > nAbs.y)
            {
                if (MyMath.Sign(n.x) > 0.0f)
                {
                    c[0].v.Set(h.x, -h.y);
                    c[0].fp.e.inEdge2 = (char)EdgeNumbers.EDGE3;
                    c[0].fp.e.outEdge2 = (char)EdgeNumbers.EDGE4;

                    c[1].v.Set(h.x, h.y);
                    c[1].fp.e.inEdge2 = (char)EdgeNumbers.EDGE4;
                    c[1].fp.e.outEdge2 = (char)EdgeNumbers.EDGE1;
                }
                else
                {
                    c[0].v.Set(-h.x, h.y);
                    c[0].fp.e.inEdge2 = (char)EdgeNumbers.EDGE1;
                    c[0].fp.e.outEdge2 = (char)EdgeNumbers.EDGE2;

                    c[1].v.Set(-h.x, -h.y);
                    c[1].fp.e.inEdge2 = (char)EdgeNumbers.EDGE2;
                    c[1].fp.e.outEdge2 = (char)EdgeNumbers.EDGE3;
                }
            }
            else
            {
                if (MyMath.Sign(n.y) > 0.0f)
                {
                    c[0].v.Set(h.x, h.y);
                    c[0].fp.e.inEdge2 = (char)EdgeNumbers.EDGE4;
                    c[0].fp.e.outEdge2 = (char)EdgeNumbers.EDGE1;

                    c[1].v.Set(-h.x, h.y);
                    c[1].fp.e.inEdge2 = (char)EdgeNumbers.EDGE1;
                    c[1].fp.e.outEdge2 = (char)EdgeNumbers.EDGE2;
                }
                else
                {
                    c[0].v.Set(-h.x, -h.y);
                    c[0].fp.e.inEdge2 = (char)EdgeNumbers.EDGE2;
                    c[0].fp.e.outEdge2 = (char)EdgeNumbers.EDGE3;

                    c[1].v.Set(h.x, -h.y);
                    c[1].fp.e.inEdge2 = (char)EdgeNumbers.EDGE3;
                    c[1].fp.e.outEdge2 = (char)EdgeNumbers.EDGE4;
                }
            }

            c[0].v = pos + Rot * c[0].v;
            c[1].v = pos + Rot * c[1].v;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"> A 和 B 的接触点</param>
        /// <param name="bodyA"></param>
        /// <param name="bodyB"></param>
        /// <returns></returns>
        public static int Collide(ref Contact[] contacts, Body bodyA, Body bodyB)
        {
        // 初始化
            Vector2f hA = 0.5f * bodyA.width;
            Vector2f hB = 0.5f * bodyB.width;

            Vector2f posA = bodyA.position;
            Vector2f posB = bodyB.position;

            Mat22 RotA = new Mat22(bodyA.rotation);
            Mat22 RotB = new Mat22(bodyB.rotation);
            Mat22 RotAT = RotA.Transpose();
            Mat22 RotBT = RotB.Transpose();

            Vector2f dp = posB - posA;
            Vector2f dA = RotAT * dp;
            Vector2f dB = RotBT * dp;

            Mat22 C = RotAT * RotB;
            Mat22 absC = MyMath.Abs(C);
            Mat22 absCT = absC.Transpose();

            //dA是两个矩形终点连线的向量，做了Abs运算后，dA表示的只是数值意义。
            //实际上，下面的代码是Abs(dA) - (hA + absC * hB)
            //hA就是矩形A右上角的点，absC * hB就是求出B点在A矩形的局部坐标系下的四个顶点中
            //x,y方向上的最大值，因为dA，hA,hB都是从(0,0)点出发的向量，所以向量中的数值仅仅
            //表示数值意义
            //Abs(dA) - (hA + absC * hB)对于这个式子，如果只看x轴
            //实际上就是dA的x坐标表示两个矩形的中心连线在x轴投影长度为s，
            //hA是矩形A的x方向在中心连线的最大投影长度a,absC*hB就是矩形B的x方向在中心连线的最大投影长度b
            //如果s-a-b大于0，这说明投影不相交。
            // Box A faces
            Vector2f faceA = MyMath.Abs(dA) - hA - absC * hB;
            if (faceA.x > 0.0f || faceA.y > 0.0f)
                return 0;
            // Box B facesa
            Vector2f faceB = MyMath.Abs(dB)  - absCT * hA - hB;
            if (faceB.x > 0.0f || faceB.y > 0.0f)
                return 0;
        // 找到最佳碰撞轴
            Axis axis;
            float separation;
  

            // Box A faces
            axis = Axis.FACE_A_X;
            separation = faceA.x;
            Vector2f normal = dA.x > 0.0f ? RotA.ex : -RotA.ex;

            const float relativeTol = 0.95f;
            const float absoluteTol = 0.05f;

            if (faceA.y > relativeTol * separation + absoluteTol * hA.y)
            {
                axis = Axis.FACE_A_Y;
                separation = faceA.y;
                normal = dA.y > 0.0f ? RotA.ey : -RotA.ey;
            }
            // Box B faces
            if (faceB.x > relativeTol * separation + absoluteTol * hB.x)
            {
                axis = Axis.FACE_B_X;
                separation = faceB.x;
                normal = dB.x > 0.0f ? RotB.ex : -RotB.ex;
            }

            if (faceB.y > relativeTol * separation + absoluteTol * hB.y)
            {
                axis = Axis.FACE_B_Y;
                separation = faceB.y;
                normal = dB.y > 0.0f ? RotB.ey : -RotB.ey;
            }
            // 根据分离轴初始化分离平面
            Vector2f frontNormal = new Vector2f();
            Vector2f sideNormal = new Vector2f();
            ClipVertex[] incidentEdge = new ClipVertex[2];
            incidentEdge[0] = new ClipVertex();
            incidentEdge[1] = new ClipVertex();

            float front = 0.0f, negSide = 0.0f, posSide = 0.0f;
            char negEdge = (char)EdgeNumbers.NO_EDGE, posEdge = (char)EdgeNumbers.NO_EDGE;
            // 计算分离线和要分离的线段
            switch (axis)
            {
                case Axis.FACE_A_X:
                    {
                        frontNormal = normal;
                        front = MyMath.Dot(posA, frontNormal) + hA.x;
                        sideNormal = RotA.ey;
                        float side = MyMath.Dot(posA, sideNormal);
                        negSide = -side + hA.y;
                        posSide = side + hA.y;
                        negEdge = (char)EdgeNumbers.EDGE3;
                        posEdge = (char)EdgeNumbers.EDGE1;
                        ComputeIncidentEdge(ref incidentEdge, hB, posB, RotB, frontNormal);
                    }
                    break;
                case Axis.FACE_A_Y:
                    {
                        frontNormal = normal;
                        front = MyMath.Dot(posA, frontNormal) + hA.y;
                        sideNormal = RotA.ex;
                        float side = MyMath.Dot(posA, sideNormal);
                        negSide = -side + hA.x;
                        posSide = side + hA.x;
                        negEdge = (char)EdgeNumbers.EDGE2;
                        posEdge = (char)EdgeNumbers.EDGE4;
                        ComputeIncidentEdge(ref incidentEdge, hB, posB, RotB, frontNormal);
                    }
                    break;

                case Axis.FACE_B_X:
                    {
                        frontNormal = -normal;
                        front = MyMath.Dot(posB, frontNormal) + hB.x;
                        sideNormal = RotB.ey;
                        float side = MyMath.Dot(posB, sideNormal);
                        negSide = -side + hB.y;
                        posSide = side + hB.y;
                        negEdge = (char)EdgeNumbers.EDGE3;
                        posEdge = (char)EdgeNumbers.EDGE1;
                        ComputeIncidentEdge(ref incidentEdge, hA, posA, RotA, frontNormal);
                    }
                    break;

                case Axis.FACE_B_Y:
                    {
                        frontNormal = -normal;
                        front = MyMath.Dot(posB, frontNormal) + hB.y;
                        sideNormal = RotB.ex;
                        float side = MyMath.Dot(posB, sideNormal);
                        negSide = -side + hB.x;
                        posSide = side + hB.x;
                        negEdge = (char)EdgeNumbers.EDGE2;
                        posEdge = (char)EdgeNumbers.EDGE4;
                        ComputeIncidentEdge(ref incidentEdge, hA, posA, RotA, frontNormal);
                    }
                    break;
            }
            // 分离其他面
            ClipVertex[] clipPoints1 = new ClipVertex[2];
            clipPoints1[0] = new ClipVertex();
            clipPoints1[1] = new ClipVertex();
            ClipVertex[] clipPoints2 = new ClipVertex[2];
            clipPoints2[0] = new ClipVertex();
            clipPoints2[1] = new ClipVertex();
            int np;

            // Clip to box side 1
            np = ClipSegmentToLine(ref clipPoints1, ref incidentEdge, -sideNormal, negSide, negEdge);

            if (np < 2)
                return 0;

            // Clip to negative box side 1
            np = ClipSegmentToLine(ref clipPoints2, ref clipPoints1, sideNormal, posSide, posEdge);

            if (np < 2)
                return 0;

            // Now clipPoints2 contains the clipping points.
            // 由于舍入，分离可能删去所有点

            int numContacts = 0;
            for (int i = 0; i < 2; ++i)
            {
                separation = MyMath.Dot(frontNormal, clipPoints2[i].v) - front;

                if (separation <= 0)
                {
                    contacts[numContacts].separation = separation;
                    contacts[numContacts].normal = normal;
                    // slide contact point onto reference face (easy to cull)
                    contacts[numContacts].position = clipPoints2[i].v - separation * frontNormal;
                    contacts[numContacts].feature = clipPoints2[i].fp;
                    if (axis == Axis.FACE_B_X || axis == Axis.FACE_B_Y)
                        Flip(contacts[numContacts].feature);
                    ++numContacts;
                }
            }
            Console.WriteLine("normal");
            Console.WriteLine(normal);
          /*  Console.WriteLine("RotA");
            Console.WriteLine(RotA.ex);
            Console.WriteLine(RotA.ey);
            Console.WriteLine("RotB");
            Console.WriteLine(RotB.ex);
            Console.WriteLine(RotB.ey);*/
            return numContacts;
        }
    }
}
