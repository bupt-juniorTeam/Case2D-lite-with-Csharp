using System;
using Case2D.Common;
using Case2D_lite;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace Case2D_lite
{
	public class FeaturePair
    {
		public class Edges
		{
			public char inEdge1; // reference
			public char outEdge1; // incident
			public char inEdge2; // reference
			public char outEdge2; // incident
		};
		public Edges e=new Edges();
		public int value;
	};
	public class Contact
	{
		public Vector2f position = new Vector2f(); // 碰撞点位置
		public Vector2f normal = new Vector2f(); // 作用线(法线)方向向量
		public Vector2f r1 = new Vector2f(), r2 = new Vector2f();
		public float separation;
		public float Pn;   // 作用线(法线)方向冲量
		public float Pt;   // 切线方向冲量
		public float Pnb;  // accumulated normal impulse for position bias
		public float massNormal, massTangent; // 质量 力矩
		public float bias;
		public FeaturePair feature=new FeaturePair();
	}
	public struct ArbiterKey
	{
		public Body body1;
		public Body body2;
		//ArbiterKey(ref Body b1, ref Body b2) 
		//{
		//}
	}
	public class Arbiter
	{
		public const int MAX_POINTS = 2; // 接触点最大为2个

		public Contact[] contacts = new Contact[MAX_POINTS]; // 两个碰撞点的contact
		public int numContacts;

		Body body1;
		Body body2;

		float friction;

		public Body Body1
        {
			get;
        }

		public Body Body2
        {
			get;
        }

		public Arbiter(ref Body b1, ref Body b2)
		{
			contacts[0] = new Contact();
			contacts[1] = new Contact();
			numContacts = Collision.Collide(ref contacts, b1, b2); // contact的个数
			friction = (float)Math.Sqrt(b1.friction * b2.friction); // 摩擦力
			body1 = b1;
			body2 = b2;

		}

		/// <summary>
		/// 更新contact
		/// </summary>
		public void Update(ref Contact [] newContacts, int numNewContacts) 
		{
			// 合并后的contact
			Contact []mergedContacts = new Contact[2];
			mergedContacts[0] = new Contact();
			mergedContacts[1] = new Contact();

			for (int i = 0; i < numNewContacts; ++i)
			{ // newcontact
				Contact cNew = newContacts[i];
				int k = -1;
				for (int j = 0; j < numContacts; ++j)
				{
					// oldcontact
					Contact cOld = contacts[j];
					// 找到相同的contact
					if (cNew.feature.value == cOld.feature.value)
					{
						k = j;
						break;
					}
				}

				
				if (k > -1)
				{
					Contact cOld = contacts[k];
					mergedContacts[i] = cNew;
					if (World.warmStarting)
					{
						mergedContacts[i].Pn = cOld.Pn;
						mergedContacts[i].Pt = cOld.Pt;
						mergedContacts[i].Pnb = cOld.Pnb;
					}
					else
					{
						mergedContacts[i].Pn = 0.0f;
						mergedContacts[i].Pt = 0.0f;
						mergedContacts[i].Pnb = 0.0f;
					}
                }
                else
                {
					mergedContacts[i] = cNew;
				}
                
				
			}

			for (int i = 0; i < numNewContacts; ++i)
				contacts[i] = mergedContacts[i];

			numContacts = numNewContacts;
		}
		
		/// <summary>
		/// 计算effective mass
		/// </summary>
		public void PreStep(float inv_dt) 
		{
			const float k_allowedPenetration = 0.01f;
			float k_biasFactor = World.positionCorrection ? 0.2f : 0.0f;

			for (int i = 0; i < numContacts; ++i)
			{
				Contact c = contacts[i];

				// 重心到作用点的距离
				Vector2f r1 = c.position - body1.position;
				Vector2f r2 = c.position - body2.position;

				// Precompute normal mass, tangent mass, and bias.
				float rn1 = MyMath.Dot(r1, c.normal); // r1 x n
				float rn2 = MyMath.Dot(r2, c.normal); // r2 x n
				// 1/m1 + 1/m2
				float kNormal = body1.invMass + body2.invMass;
				// 1 / m_n = 1/m1 + 1/m2 + 1/I1 * (r1 • r1 - rn1 * rn1) + 1/I2 * (r2 • r2 - rn2 * rn2)
				kNormal += body1.invI * (MyMath.Dot(r1, r1) - rn1 * rn1) + body2.invI * (MyMath.Dot(r2, r2) - rn2 * rn2);
				// m_n = 1 / (1 / m_n)
				c.massNormal = 1.0f / kNormal;

				// 切线方向，交换单位垂直向量的方向：(i, j) => (j, -i)
				Vector2f tangent = MyMath.Cross(c.normal, 1.0f);
				float rt1 = MyMath.Dot(r1, tangent);
				float rt2 = MyMath.Dot(r2, tangent);
				// 1/m1 + 1/m2
				float kTangent = body1.invMass + body2.invMass;
				kTangent += body1.invI * (MyMath.Dot(r1, r1) - rt1 * rt1) + body2.invI * (MyMath.Dot(r2, r2) - rt2 * rt2);
				// m_n = 1 / (1 / m_n)
				c.massTangent = 1.0f / kTangent;

				c.bias = -k_biasFactor * inv_dt * Math.Min(0.0f, c.separation + k_allowedPenetration);

				if (World.accumulateImpulses)
				{ // 法线冲量和摩擦冲量
				  // Apply normal + friction impulse
					Vector2f P = c.Pn * c.normal + c.Pt * tangent; // 线性冲量 * 垂向量 + 角冲量 * 切向量

					body1.velocity -= body1.invMass * P; // v -= P/m
					body1.angularVelocity -= body1.invI * MyMath.Cross(r1, P); // 冲量 * 作用距离 / 转动惯量
					
					body2.velocity += body2.invMass * P; // v += P/m
					body2.angularVelocity += body2.invI * MyMath.Cross(r2, P);
				}
			}
		}

		/// <summary>
		/// 计算冲量，更新速度
		/// </summary>
		public void ApplyImpulse() 
		{
			Body b1 = body1;
			Body b2 = body2;

			for (int i = 0; i < numContacts; ++i)
			{
				Contact c = contacts[i];
				// 重心到作用点的距离
				c.r1 = c.position - b1.position;
				c.r2 = c.position - b2.position;

				// ************************************* 法线冲量
				// Relative velocity at contact
				// 相对速度 v + w * r
				Vector2f dv = b2.velocity + MyMath.Cross(b2.angularVelocity, c.r2) - b1.velocity - MyMath.Cross(b1.angularVelocity, c.r1);

				// Compute normal impulse
				// 法线冲量
				float vn = MyMath.Dot(dv, c.normal);

				float dPn = c.massNormal * (-vn + c.bias);

				if (World.accumulateImpulses)
				{
					// Clamp the accumulated impulse
					float Pn0 = c.Pn;
					c.Pn = Math.Max(Pn0 + dPn, 0.0f);
					dPn = c.Pn - Pn0;
				}
				else
				{
					dPn = Math.Max(dPn, 0.0f);
				}
				// Apply contact impulse
				Vector2f Pn = dPn * c.normal;

				b1.velocity -= b1.invMass * Pn; // v = p/m
				b1.angularVelocity -= b1.invI * MyMath.Cross(c.r1, Pn); // w = 冲量 * 作用距离 / 转动惯量

				b2.velocity += b2.invMass * Pn;
				b2.angularVelocity += b2.invI * MyMath.Cross(c.r2, Pn);

				// ************************************* 切线冲量
				// Relative velocity at contact
				// 相对速度
				dv = b2.velocity + MyMath.Cross(b2.angularVelocity, c.r2) - b1.velocity - MyMath.Cross(b1.angularVelocity, c.r1);

				Vector2f tangent = MyMath.Cross(c.normal, 1.0f); // 切线向量
				float vt = MyMath.Dot(dv, tangent); // 线速度
				float dPt = c.massTangent * (-vt);

				if (World.accumulateImpulses)
				{
					// Compute friction impulse
					float maxPt = friction * c.Pn;

					// Clamp friction
					float oldTangentImpulse = c.Pt;
					c.Pt = MyMath.Clamp(oldTangentImpulse + dPt, -maxPt, maxPt);
					dPt = c.Pt - oldTangentImpulse;
				}
				else
				{
					float maxPt = friction * dPn;
					dPt = MyMath.Clamp(dPt, -maxPt, maxPt);
				}

				// Apply contact impulse
				Vector2f Pt = dPt * tangent;

				b1.velocity -= b1.invMass * Pt; // v = p/m
				b1.angularVelocity -= b1.invI * MyMath.Cross(c.r1, Pt);

				b2.velocity += b2.invMass * Pt;
				b2.angularVelocity += b2.invI * MyMath.Cross(c.r2, Pt);
			}
		}
	}
}