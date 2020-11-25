using System;
using Case2D.Common;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace Case2D_lite
{
	public class Body
	{
		public Vector2f position = new Vector2f(); // 位置(x,y)
		public float rotation; // 旋转角度

		public Vector2f velocity = new Vector2f(); // 速度(向量)
		public float angularVelocity; // 角速度

		public Vector2f force = new Vector2f(); // 力(向量)
		public float torque; // 力矩

		public Vector2f width = new Vector2f(); // 长和宽 (w,h)

		public float friction; // 摩擦力
		public float mass, invMass; // 质量 1/质量
		public float I, invI; // 转动惯量 1/转动惯量

		public Body()
		{
		
			position.Set(0.0f, 0.0f);
			rotation = 0.0f;
			velocity.Set(0.0f, 0.0f);
			angularVelocity = 0.0f;
			force.Set(0.0f, 0.0f);
			torque = 0.0f;
			friction = 0.2f;

			width.Set(1.0f, 1.0f);
			mass = float.MaxValue;
			invMass = 0.0f;
			I = float.MaxValue;
			invI = 0.0f;
		}

		public void Set(Vector2f w, float m)
		{
			position.Set(0.0f, 0.0f);
			rotation = 0.0f;
			velocity.Set(0.0f, 0.0f);
			angularVelocity = 0.0f;
			force.Set(0.0f, 0.0f);
			torque = 0.0f;
			friction = 0.2f;

			width = w;
			mass = m;

			if (mass < float.MaxValue)
			{
				invMass = 1.0f / mass;
				I = mass * (width.x * width.x + width.y * width.y) / 12.0f; // 长方体转动惯量计算公式
				invI = 1.0f / I;
			}
			else
			{
				invMass = 0.0f;
				I = float.MaxValue;
				invI = 0.0f;
			}
		}

		public void AddForce(Vector2f f)
		{
			force += f;
		}
	}
}
