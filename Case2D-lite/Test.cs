using System;
using System.Collections.Generic;

namespace Case2D_lite
{
    public class Test
    {
        public void printBody(ref List<Body> bodies)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                Body b = bodies[i];
                if (b == null || b.mass == float.MaxValue)
                    continue;
                System.Console.Write("Body: " + b.mass + " ");
                System.Console.Write("pos: " + b.position + " ");
                System.Console.Write("rot: " + b.rotation + " ");
                //System.Console.Write("v: " + b.velocity + " ");
                //System.Console.Write("w: " + b.angularVelocity + " ");
                System.Console.WriteLine();
            }
        }

        public void printArbiter(ref Dictionary<ContactPair, Arbiter> arbs)
        {
            System.Console.WriteLine("Arbiter: ");
            foreach (KeyValuePair<ContactPair, Arbiter> obj in arbs)
            {
                Arbiter arb = obj.Value;
                List<Body> bodies = new List<Body>();
                bodies.Add(arb.Body1);
                bodies.Add(arb.Body2);
                printBody(ref bodies);
                printContact(ref arb.contacts);
            }
            System.Console.WriteLine();
        }

        public void printContact(ref Contact [] contacts)
        {
            for (int i = 0; i < 2; i++)
            {
                Contact contact = contacts[i];
                System.Console.Write("Contact: ");
                System.Console.Write(contact.position + " ");
                System.Console.Write(contact.normal + " ");
                System.Console.Write(contact.massTangent + " ");
                System.Console.Write(contact.Pn + " ");
                System.Console.WriteLine();
            }
        }
    }
}