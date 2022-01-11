using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case2D.Common;
namespace Case2D_lite
{

    public class ContactManager
    {
        private DynamicTree m_bvhTree;
        public Dictionary<int, Body> m_proxy2body; // 映射，树中的节点号映射至物体
        public List<ContactPair> m_contactPairs;   //碰撞对
        public Dictionary<Body, int> m_body2proxy;
        public ContactManager()
        {
            m_bvhTree = new DynamicTree();
            m_proxy2body = new Dictionary<int, Body>();
            m_contactPairs = new List<ContactPair>();
            m_body2proxy = new Dictionary<Body, int>();
        }
        public void Initialize(List<Body> bodies)
        {
            for (int i = 0; i < bodies.Count(); ++i)
            {
                //对每一个物体
                Body body = bodies[i];
                //创造其AABB包围盒
                AABB aabb = ComputerAABB(body);
                //加入到BVH树中。树会自平衡
                int proxy = m_bvhTree.CreateProxy(aabb, ref body);
                //记录节点号
                m_proxy2body[proxy] = body;
                m_body2proxy[body] = proxy;
            }

        }
        public void Reset()
        {
            m_body2proxy.Clear();
            m_proxy2body.Clear();
            m_contactPairs.Clear();
            m_bvhTree.Reset();
        }
       
       

        public AABB ComputerAABB(Body body)
        {
            Mat22 R = new Mat22(body.rotation);
            float width = body.width_height.x*0.5f;
            float height = body.width_height.y*0.5f;
            float x = body.position.x;
            float y = body.position.y;

            Vector2f[] points = new Vector2f[4];
            points[0] = new Vector2f(x - width, y - height);
            points[0] = MyMath.Mul(R, points[0]);
            points[1]= new Vector2f(x + width, y - height);
            points[1] = MyMath.Mul(R, points[1]);
            points[2] = new Vector2f(x - width, y + height);
            points[2] = MyMath.Mul(R, points[2]);
            points[3] = new Vector2f(x + width, y + height);
            points[3] = MyMath.Mul(R, points[3]);
            Vector2f p_min=new Vector2f(float.MaxValue, float.MaxValue);
            Vector2f p_max = new Vector2f(float.MinValue, float.MinValue);
            for (int i=0; i < 4; ++i)
            {
                p_min = MyMath.Min(p_min, points[i]);
                p_max = MyMath.Max(p_max, points[i]);
            }
            AABB aabb = new AABB(p_min, p_max);
            return aabb;
        }

        public void PostUpdate(List<Body> bodies)
        {
            /* foreach (var contact in m_contactPairs.ToList())
             {
                 AABB aabb1 = ComputerAABB(contact.body1);
                 AABB aabb2 = ComputerAABB(contact.body2);
                 if (!AABB.TestOverlap(aabb1, aabb2))
                 {
                     m_contactPairs.Remove(contact);
                 }
             }*/
            foreach(var body in bodies)
            {
                MoveAABBInTree(body);
            }
            m_contactPairs.Clear();

        }


        public List<ContactPair> FindContacts()
        {

            foreach (var proxy in m_proxy2body)
            {
                int thisNodeId = proxy.Key;
          
                //与之可能产生碰撞的node id
                int anotherNodeId = m_bvhTree.Query(thisNodeId);
                //如果找到
                if (anotherNodeId != -1)
                {
                    //添加到潜在的碰撞对中
                    ContactPair contactPair;
                    contactPair.body1 = m_proxy2body[anotherNodeId];
                    contactPair.body2 = m_proxy2body[thisNodeId];
                    m_contactPairs.Add(contactPair);
                }
            }
            return m_contactPairs;
        }

        private void MoveAABBInTree(Body body)
        {
            int proxyId = m_body2proxy[body];

            //TODO:
            //更新物体的包围盒
            AABB aabb = ComputerAABB(body);

            //更新在树中的位置
            m_bvhTree.MoveProxy(proxyId, aabb);
        }

    }
}
