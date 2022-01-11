using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Case2D.Common;
namespace Case2D_lite
{
    public class World
    {
        public int iterations; // 迭代速度
        public Vector2f gravity; // 重力加速度
        public List<Body> bodies; // 物体
        public List<Joint> joints; // 关节
        public Dictionary<ContactPair, Arbiter> arbiters; // 碰撞
        public ContactManager contactManager;
        public static bool accumulateImpulses = true;
        public static bool warmStarting = true;
        public static bool positionCorrection = true;
        private Test test;
        public World(Vector2f gravity, int iterations)
        {
            this.gravity = new Vector2f();
            this.bodies = new List<Body>();
            this.joints = new List<Joint>();
            this.arbiters = new Dictionary<ContactPair, Arbiter>();
            this.gravity = gravity; // 重力加速度(0,-9.8)
            this.iterations = iterations; // 每个时间步长迭代次数：dt * iterations
            this.test = new Test();
            this.contactManager = new ContactManager();

        }
        public void Add(Body body)
        {
            bodies.Add(body);
        }
        public void Add(Joint joint)
        {
            joints.Add(joint);
        }
        public void Clear()
        {
            bodies.Clear();
            joints.Clear();
            arbiters.Clear();
            contactManager.Reset();
        }
        public void Initialize()
        {
            contactManager.Initialize(bodies);
        }
        /// <summary>
        /// 分为几个阶段：
        /// 碰撞检测：BroadPhase NarrowPhase(调用collide())
        /// 计算受力(加速度)：更新速度
        /// 计算约束：关节(joint) 碰撞(contact)，进而更新速度
        /// 更新位置：position 
        /// </summary>
        public void Step(float dt)
        {
            /*test.printBody(ref bodies);
            test.printArbiter(ref arbiters);*/
            float inv_dt = dt > 0.0f ? 1.0f / dt : 0.0f; // 时间步长 dt 1/dt

            BroadPhase();

            // 更新力
            for (int i = 0; i < bodies.Count(); ++i) // 遍历所有物体
            {
                Body b = bodies[i];

                if (b.invMass == 0.0f) // 质量无限大
                    continue;

                b.velocity += dt * (gravity + b.invMass * b.force); // 速度改变：重力加速度+加速度(向量)
                b.angularVelocity += dt * b.invI * b.torque; // 角速度改变：力矩/转动惯量
            }

            // 运行 pre-step
            foreach (var arb in arbiters)
            {
                arb.Value.PreStep(inv_dt); // 碰撞 contact
            }

            for (int i = 0; i < joints.Count(); ++i)
            {
                joints[i].PreStep(inv_dt); // 关节 joint
            }

            // Perform iterations
            for (int i = 0; i < iterations; ++i)
            {
                foreach (var arb in arbiters)
                {
                    arb.Value.ApplyImpulse(); // 冲量
                }

                for (int j = 0; j < joints.Count(); ++j)
                {
                    joints[j].ApplyImpulse(); // 冲量
                }
            }
            //test.printBody(ref bodies);
            // 更新位置
            for (int i = 0; i < bodies.Count(); ++i)
            {
                Body b = bodies[i];

                b.position += dt * b.velocity;
                b.rotation += dt * b.angularVelocity;

                b.force.Set(0.0f, 0.0f);
                b.torque = 0.0f;

            }
            contactManager.PostUpdate(bodies);

        }

        public void BroadPhase()
        {

            //浅拷贝可能有问题，引擎会爆炸
            List<ContactPair> pairs = contactManager.FindContacts();

            foreach (var pair in pairs)
            {
                Body bi = pair.body1;
                Body bj = pair.body2;
                Arbiter newArb = new Arbiter(ref bi, ref bj);
                ContactPair key;
                key.body1 = bi;
                key.body2 = bj;
                if (newArb.numContacts > 0)
                {
                    bool is_find = arbiters.TryGetValue(key, out Arbiter iter);
                    if (!is_find)
                    {
                        arbiters.Add(key, newArb);
                    }
                    else
                    {
                        iter.Update(ref newArb.contacts, newArb.numContacts);
                    }
                }
                else
                {
                    arbiters.Remove(key);
                }
            }

        }

        public void BroadPhase2()
        {
            // O(n^2) broad-phase
            for (int i = 0; i < (int)bodies.Count(); ++i)
            {
                Body bi = bodies[i];

                for (int j = i + 1; j < (int)bodies.Count(); ++j)
                {
                    Body bj = bodies[j];

                    if (bi.invMass == 0.0f && bj.invMass == 0.0f) // 质量无限大
                        continue;

                    Arbiter newArb = new Arbiter(ref bi, ref bj);
                    ContactPair key;
                    key.body1 = bi;
                    key.body2 = bj;
                    if (newArb.numContacts > 0)
                    {
                        bool is_find = arbiters.TryGetValue(key, out Arbiter iter);
                        if (!is_find)
                        {
                            arbiters.Add(key, newArb);
                        }
                        else
                        {
                            iter.Update(ref newArb.contacts, newArb.numContacts);
                        }
                    }
                    else
                    {
                        arbiters.Remove(key);
                    }
                }
            }
        }
    }
}
