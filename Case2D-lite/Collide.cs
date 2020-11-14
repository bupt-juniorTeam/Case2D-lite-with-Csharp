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
        public class ClipVertex {
            ClipVertex() { fp.value = 0; }
            Vector2f v;
            FeaturePair fp;
        };
        public static void Filp(FeaturePair fp)
        {
            var temp = fp.e.inEdge1;
            fp.e.inEdge1 = fp.e.inEdge2;
            fp.e.inEdge2 = temp;

            temp = fp.e.outEdge1;
            fp.e.outEdge1 = fp.e.outEdge2;
            fp.e.outEdge2 = temp;
        }
        public static int ClipSegmentToLine(ClipVertex[] vOut, ClipVertex[] vIn,Vector2f normal, float offset, char clipEdge)
        {
            return 0;
        }
        public static void ComputeIncidentEdge(ClipVertex[] c,Vector2f h, Vector2f pos,Mat22 Rot,Vector2f normal)
        {

        }
        public static int Collide(Contact contacts, Body bodyA, Body bodyB)
        {
            return 0;
        }
    }
}
