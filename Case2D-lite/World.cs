using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Case2D.Common;
namespace Case2D_lite {
    class World {
        public int iterations;
        public Vector2f gravity;
        public List<Body> bodies;
        public List<Joint> joints;
        public Dictionary<ArbiterKey, Arbiter> arbiters;
        public static bool accumulateImpulses = true;
        public static bool warmStarting = true;
        public static bool positionCorrection = true;
        World(Vector2f gravity,int iterations)
        {
            this.gravity = gravity;
            this.iterations = iterations;
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
        }
        public void Step(float dt)
        {
            float inv_dt = dt > 0.0f ? 1.0f / dt : 0.0f;

            BroadPhase();

            // 更新力
            for (int i = 0; i < bodies.Count(); ++i)
            {
                Body b = bodies[i];

                if (b.invMass == 0.0f)
                    continue;

                b.velocity += dt * (gravity + b.invMass * b.force);
                b.angularVelocity += dt * b.invI * b.torque;
            }

            // 运行 pre-step
            foreach (var arb in arbiters)
            {
                arb.Value.PreStep(inv_dt);
            }

            for (int i = 0; i < joints.Count(); ++i)
            {
                joints[i].PreStep(inv_dt);
            }

            // Perform iterations
            for (int i = 0; i < iterations; ++i)
            {
                foreach (var arb in arbiters)
                {
                    arb.Value.ApplyImpulse();
                }

                for (int j = 0; j < joints.Count(); ++j)
                {
                    joints[j].ApplyImpulse();
                }
            }

            // 更新速度
            for (int i = 0; i < bodies.Count(); ++i)
            {
                Body b = bodies[i];

                b.position += dt * b.velocity;
                b.rotation += dt * b.angularVelocity;

                b.force.Set(0.0f, 0.0f);
                b.torque = 0.0f;
            }

        }
        public void BroadPhase()
        {
            // O(n^2) broad-phase
            for (int i = 0; i < (int)bodies.Count(); ++i)
            {
                Body bi = bodies[i];

                for (int j = i + 1; j < (int)bodies.Count(); ++j)
                {
                    Body bj = bodies[j];

                    if (bi.invMass == 0.0f && bj.invMass == 0.0f)
                        continue;

                    Arbiter newArb = new Arbiter(bi, bj);
                    ArbiterKey key(bi, bj);

                    if (newArb.numContacts > 0)
                    {
                        bool is_find = arbiters.TryGetValue(key, out Arbiter iter);
                        if (!is_find)
                        {
                            arbiters.Add(key, newArb);
                        }
                        else
                        {
                            iter.Update(newArb.contacts, newArb.numContacts);
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
