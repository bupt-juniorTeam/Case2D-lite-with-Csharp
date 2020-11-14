using System;
using Case2D.Common;
using Case2D_lite;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace Case2D_lite
{
	public struct FeaturePair
    {
		public struct Edges
		{
			public char inEdge1;
			public char outEdge1;
			public char inEdge2;
			public char outEdge2;
		};
		public Edges e;
		public int value;
	};
	public struct Contact
	{
		public Vector2f position;
		public Vector2f normal; // 作用线(法线)方向向量
		public Vector2f r1, r2;
		public float separation;
		public float Pn;   // 作用线(法线)方向冲量
		public float Pt;   // 切线方向冲量
		public float Pnb;  // accumulated normal impulse for position bias
		public float massNormal, massTangent; // 质量 力矩
		public float bias;
		public FeaturePair feature;
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
		public const int MAX_POINTS = 2;

		public Contact[] contacts = new Contact[MAX_POINTS];
		public int numContacts;

		Body body1;
		Body body2;

		float friction;

		public Arbiter(ref Body b1, ref Body b2)
		{
			numContacts = Collition.Collide(contacts, body1, body2);
			friction = (float)Math.Sqrt(body1.friction * body2.friction);

		}

		public void Update(ref Contact [] newContacts, int numNewContacts) 
		{
			Contact []mergedContacts = new Contact[2];

			for (int i = 0; i < numNewContacts; ++i)
			{
				Contact cNew = newContacts[i];
				int k = -1;
				for (int j = 0; j < numContacts; ++j)
				{
					Contact cOld = contacts[j];
					if (cNew.feature.value == cOld.feature.value)
					{
						k = j;
						break;
					}
				}

				if (k > -1)
				{
					Contact c = mergedContacts[i];
					Contact cOld = contacts[k];
					c = cNew;
					if (World.warmStarting)
					{
						c.Pn = cOld.Pn;
						c.Pt = cOld.Pt;
						c.Pnb = cOld.Pnb;
					}
					else
					{
						c.Pn = 0.0f;
						c.Pt = 0.0f;
						c.Pnb = 0.0f;
					}
				}
				else
				{
					mergedContacts[i] = newContacts[i];
				}
			}

			for (int i = 0; i < numNewContacts; ++i)
				contacts[i] = mergedContacts[i];

			numContacts = numNewContacts;
		}

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