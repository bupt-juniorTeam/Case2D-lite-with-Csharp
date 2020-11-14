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
        public ArrayList<Body> bodies;
        public ArrayList<Joint> joints;
        public Dictionary<ArbiterKey, Arbiter> arbiters;
        public static bool accumulateImpulses;
        public static bool warmStarting;
        public static bool positionCorrection;
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

            // Integrate forces.
            for (int i = 0; i < (int)bodies.size(); ++i)
            {
                Body* b = bodies[i];

                if (b.invMass == 0.0f)
                    continue;

                b.velocity += dt * (gravity + b.invMass * b.force);
                b.angularVelocity += dt * b.invI * b.torque;
            }

            // Perform pre-steps.
            for (ArbIter arb = arbiters.begin(); arb != arbiters.end(); ++arb)
            {
                arb.second.PreStep(inv_dt);
            }

            for (int i = 0; i < (int)joints.size(); ++i)
            {
                joints[i].PreStep(inv_dt);
            }

            // Perform iterations
            for (int i = 0; i < iterations; ++i)
            {
                for (ArbIter arb = arbiters.begin(); arb != arbiters.end(); ++arb)
                {
                    arb.second.ApplyImpulse();
                }

                for (int j = 0; j < (int)joints.size(); ++j)
                {
                    joints[j].ApplyImpulse();
                }
            }

            // Integrate Velocities
            for (int i = 0; i < (int)bodies.size(); ++i)
            {
                Body* b = bodies[i];

                b.position += dt * b.velocity;
                b.rotation += dt * b.angularVelocity;

                b.force.Set(0.0f, 0.0f);
                b.torque = 0.0f;
            }

        }
        public void BroadPhase()
        {
            // O(n^2) broad-phase
            for (int i = 0; i < (int)bodies.size(); ++i)
            {
                Body* bi = bodies[i];

                for (int j = i + 1; j < (int)bodies.size(); ++j)
                {
                    Body* bj = bodies[j];

                    if (bi.invMass == 0.0f && bj.invMass == 0.0f)
                        continue;

                    Arbiter newArb(bi, bj);
                    ArbiterKey key(bi, bj);

                    if (newArb.numContacts > 0)
                    {
                        ArbIter iter = arbiters.find(key);
                        if (iter == arbiters.end())
                        {
                            arbiters.insert(ArbPair(key, newArb));
                        }
                        else
                        {
                            iter.second.Update(newArb.contacts, newArb.numContacts);
                        }
                    }
                    else
                    {
                        arbiters.erase(key);
                    }
                }
            }
        }
    }
}
