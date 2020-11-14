using System;
using Case2D.Common;
using Case2D_lite;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace Case2D_lite
{
	struct FeaturePair
    {
		struct Edges
		{
			char inEdge1;
			char outEdge1;
			char inEdge2;
			char outEdge2;
		};
		Edges e;
		int value;
	};
	public class Arbiter
	{
		struct Contact
		{
			Vector2f position;
			Vector2f normal; // 作用线(法线)方向向量
			Vector2f r1, r2;
			float separation;
			float Pn;   // 作用线(法线)方向冲量
			float Pt;   // 切线方向冲量
			float Pnb;  // accumulated normal impulse for position bias
			float massNormal, massTangent; // 质量 力矩
			float bias;
			FeaturePair feature;
		}
		struct ArbiterKey
        {
			Body body1;
			Body body2;
			//ArbiterKey(ref Body b1, ref Body b2) 
			//{
			//}
        }
		public Arbiter()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}