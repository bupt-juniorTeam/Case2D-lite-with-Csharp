using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case2D.Common;

namespace Case2D_lite
{
    public class Joint
    {
       public Body body1;
        public Body body2;
        public float biasFactor;
        public float softness;
        public Vector2f P; //累积冲量
        public Vector2f bias;
        public Vector2f r1, r2;
        public Vector2f localAnchor1;
        public Vector2f localAnchor2;
        public Mat22 M;

        public Joint()
        {
            body1 = new Body();
            body2 = new Body();
            P = new Vector2f();
            biasFactor = 0.2f;
            softness = 0.0f;
        }

        //anchor为转动关节坐标（一个点）
        public void Set(Body b1,Body b2,Vector2f anchor)
        {
            body1 = b1;
            body2 = b2;

            Mat22 Rot1 = new Mat22(body1.rotation);
            Mat22 Rot2 = new Mat22(body2.rotation);

            //旋转矩阵的逆矩阵就是它的转置矩阵。这里面相当于反向旋转了
            //由于求矩阵的逆比较费时间但是求转置快，所以就求转置了
            Mat22 Rot1T = Rot1.Transpose();
            Mat22 Rot2T = Rot2.Transpose();

            localAnchor1 = Rot1T * (anchor - body1.position);   //个人理解，先反向旋转，绘制时再反向旋转以抵消
            localAnchor2 = Rot2T * (anchor - body2.position);

            P.Set(0.0f, 0.0f);

            softness = 0.0f;
            biasFactor = 0.2f;


        }
        public void PreStep(float inv_dt)
        {
            // Pre-compute anchors, mass matrix, and bias.
            //计算相对质量
            Mat22 Rot1 = new Mat22(body1.rotation);
            Mat22 Rot2 = new Mat22(body2.rotation);

            r1 = Rot1 * localAnchor1;
            r2 = Rot2 * localAnchor2;
            
            //相对质量计算公式
            // deltaV = deltaV0 + K * impulse
            // invM = [(1/m1 + 1/m2) * I - skew(r1) * invI1 * skew(r1) - skew(r2) * invI2 * skew(r2)]
            //      = [1/m1+1/m2     0    ] + invI1 * [r1.y*r1.y -r1.x*r1.y] + invI2 * [r1.y*r1.y -r1.x*r1.y]
            //        [    0     1/m1+1/m2]           [-r1.x*r1.y r1.x*r1.x]           [-r1.x*r1.y r1.x*r1.x]
            Mat22 K1 = new Mat22();
            K1.ex.x = body1.invMass + body2.invMass;
            K1.ey.x = 0.0f;
            K1.ex.y = 0.0f;
            K1.ey.y = body1.invMass + body2.invMass;

            Mat22 K2 = new Mat22();
            K2.ex.x = body1.invI * r1.y * r1.y;
            K2.ey.x = -body1.invI * r1.x * r1.y;
            K2.ex.y = -body1.invI * r1.x * r1.y; 
            K2.ey.y = body1.invI * r1.x * r1.x;

            Mat22 K3 = new Mat22();
            K3.ex.x = body2.invI * r2.y * r2.y; 
            K3.ey.x = -body2.invI * r2.x * r2.y;
            K3.ex.y = -body2.invI * r2.x * r2.y; 
            K3.ey.y = body2.invI * r2.x * r2.x;

            Mat22 K = K1 + K2 + K3;
            K.ex.x += softness;
            K.ey.y += softness;

            M = K.GetInverse();

            Vector2f p1 = body1.position + r1;
            Vector2f p2 = body2.position + r2;
            Vector2f dp = p2 - p1;

            if (World.positionCorrection)
            {
                bias = -biasFactor * inv_dt * dp;
            }
            else
            {
                bias.Set(0, 0);
            }

            if (World.warmStarting)
            {
                // Apply accumulated impulse.
                body1.velocity -= body1.invMass * P;
                body1.angularVelocity -= body1.invI * MyMath.Cross(r1, P);

                body2.velocity += body2.invMass * P;
                body2.angularVelocity += body2.invI * MyMath.Cross(r2, P);
            }
            else
            {
                P.Set(0, 0);
            }



        }
        public void ApplyImpulse()
        {
            //相对速度
            Vector2f dv = body2.velocity + MyMath.Cross(body2.angularVelocity, r2) - body1.velocity - MyMath.Cross(body1.angularVelocity, r1);

            Vector2f impulse = M * (bias - dv - softness * P);

            body1.velocity -= body1.invMass * impulse;
            body1.angularVelocity -= body1.invI * MyMath.Cross(r1, impulse);


            body1.velocity += body2.invMass * impulse;
            body1.angularVelocity += body2.invI * MyMath.Cross(r2, impulse);

            P += impulse;

        }
    }
}
