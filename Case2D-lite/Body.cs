using System;
using Case2D.Common;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace Case2D_lite
{
	public class Body
	{
		private Vector2f position; // 位置(x,y)
		private float rotation; // 旋转角度

		private Vector2f velocity; // 速度
		private float angularVelocity; // 角速度

		private Vector2f force; // 力
		private float torque; // 力矩

		private Vector2f width; // 长和宽 (w,h)

		private float friction; // 摩擦力
		private float mass, invMass; // 质量 1/质量
		private float I, invI; // 转动惯量 1/转动惯量

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
				I = mass * (width.x * width.x + width.y * width.y) / 12.0f;
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
