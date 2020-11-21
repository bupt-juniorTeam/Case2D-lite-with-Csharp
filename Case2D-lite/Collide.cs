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
    class Collition {
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
            public Vector2f v;
            public FeaturePair fp;
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
        /// 计算射入边
        /// </summary>
        /// <param name="c"></param>
        /// <param name="h"></param>
        /// <param name="pos"></param>
        /// <param name="Rot"></param>
        /// <param name="normal">此法向量为参考面的法向量</param>
        public static void ComputeIncidentEdge(ClipVertex[] c,Vector2f h, Vector2f pos,Mat22 Rot,Vector2f normal)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"> A 和 B 的接触点</param>
        /// <param name="bodyA"></param>
        /// <param name="bodyB"></param>
        /// <returns></returns>
        public static int Collide(Contact[] contacts, Body bodyA, Body bodyB)
        {
            return 0;
        }
    }
}
