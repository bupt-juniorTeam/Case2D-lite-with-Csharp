using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case2D.Common
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


    class Collide {
        class ClipVertex {
            ClipVertex() { fp.value = 0; }
            Vector2f v;
            FeaturePair fp;
        };
        public void Filp(FeaturePair& fp)
        {

        }
        public int ClipSegmentToLine(ClipVertex[] vOut, ClipVertex[] vIn,Vector2f normal, float offset, char clipEdge)
        {

        }
    }
}
