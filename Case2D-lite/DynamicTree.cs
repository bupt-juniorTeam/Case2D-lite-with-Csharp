using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Case2D.Common;
using System.Diagnostics;

namespace Case2D_lite
{

    public struct AABB
    {
        public Vector2f lowerBound; //低点
        public Vector2f upperBound;//高点
        public AABB(Vector2f low, Vector2f up)
        {
            lowerBound = low;
            upperBound = up;
        }

        /// Verify that the bounds are sorted.
        public bool IsValid()
        {
            var d = upperBound - lowerBound;
            bool valid = d.x >= 0.0 && d.y >= 0.0;
            valid = valid && !float.IsInfinity(upperBound.x) && !float.IsNaN(upperBound.x)
                          && !float.IsInfinity(upperBound.y) && !float.IsNaN(upperBound.y)
                          && !float.IsInfinity(lowerBound.x) && !float.IsNaN(lowerBound.x)
                          && !float.IsInfinity(lowerBound.y) && !float.IsNaN(lowerBound.y);
            return valid;
        }

        /// Get the center of the AABB.
        public Vector2f GetCenter()
        {
            return 0.5f * (lowerBound + upperBound);
        }

        /// Get the extents of the AABB (half-widths).
        /// 通过center来描述范围,所以范围的值为half-widths
        public Vector2f GetExtents()
        {
            return 0.5f * (upperBound - lowerBound);
        }
        //周长
        public float GetPerimeter()
        {
            float wx = upperBound.x - lowerBound.x;
            float wy = upperBound.y - lowerBound.y;
            return 2.0f * (wx + wy);
        }

        /// Combine an AABB into this one.
        public void Combine(AABB aabb)
        {
            lowerBound = new Vector2f(
                Math.Min(aabb.lowerBound.x, lowerBound.x),
                Math.Min(aabb.lowerBound.y, lowerBound.y)
                );
            upperBound = new Vector2f(
                Math.Max(aabb.upperBound.x, upperBound.x),
                Math.Max(aabb.upperBound.y, upperBound.y)
                );
        }
        /// Combine two AABBs into this one.
        public void Combine(AABB aabb1, AABB aabb2)
        {
            lowerBound = new Vector2f(
               MyMath.Min(aabb1.lowerBound, aabb2.lowerBound)
                );
            upperBound = new Vector2f(
              MyMath.Max(aabb1.upperBound, aabb2.upperBound)
                );
        }
        /// Does this aabb contain the provided AABB.
        public bool Contains(AABB aabb)
        {
            bool result = true;
            result = result && lowerBound.x <= aabb.lowerBound.x;
            result = result && lowerBound.y <= aabb.lowerBound.y;
            result = result && upperBound.x >= aabb.upperBound.x;
            result = result && upperBound.y >= aabb.upperBound.y;
            return result;
        }

        //判断AABB是否包围
        /**************************************************************************
       * 功能描述：测试两个通用的形状是否重叠。
           通过距离【AABB】判断是否重叠
       * 参数说明： a ：AABB a
                   b: AABB b
       * 返 回 值：true    ：重叠
           false   ：不重叠
       ***************************************************************************/
        public static bool TestOverlap(AABB a, AABB b)
        {
            Vector2f d1 = b.lowerBound - a.upperBound;
            Vector2f d2 = a.lowerBound - b.upperBound;

            if (d1.x > 0.0f || d1.y > 0.0f) return false;
            if (d2.x > 0.0f || d2.y > 0.0f) return false;
            return true;
        }
    }


    public struct TreeNode
    {
        //两个孩子的index
        public int child1;
        public int child2;

        //leaf=0, free node = -1
        public int height;

        public bool moved;

        //Enlarged AABB
        public AABB aabb;

        //父节点指针(索引)或孩子节点指针(索引)
        //因为此动态树是申请一块大的连续的空间，即含有n个元素的动态数组罢了
        //故用动态数组的索引值来模拟指针。
        public int parent;
        public int next;

        public Body userData;

        public bool IsLeaf()
        {
            return child1 == -1;
        }
    }
    //动态AABB树，里面存的对象是AABB。
    class DynamicTree
    {
        private int m_root;

        private TreeNode[] m_nodes;
        private int m_nodeCount;
        private int m_nodeCapacity;

        private int m_freeList;

        private int m_insertionCount;
        private const int nullNode = -1;
        /// Constructing the tree initializes the node pool.
        public DynamicTree()
        {
            m_root = nullNode;

            m_nodeCapacity = 32;
            m_nodeCount = 0;
            m_nodes = null;
            m_nodes = new TreeNode[m_nodeCapacity];

            for (int i = 0; i < m_nodeCapacity - 1; ++i)
            {
                m_nodes[i].next = i + 1;
                m_nodes[i].height = -1;
            }
            m_nodes[m_nodeCapacity - 1].next = nullNode;
            m_nodes[m_nodeCapacity - 1].height = nullNode;
            m_freeList = 0;

            m_insertionCount = 0;


        }

        public void Reset()
        {
            m_root = nullNode;

            m_nodeCapacity = 32;
            m_nodeCount = 0;
            m_nodes = new TreeNode[m_nodeCapacity];

            for (int i = 0; i < m_nodeCapacity - 1; ++i)
            {
                m_nodes[i].next = i + 1;
                m_nodes[i].height = -1;
            }
            m_nodes[m_nodeCapacity - 1].next = nullNode;
            m_nodes[m_nodeCapacity - 1].height = nullNode;
            m_freeList = 0;

            m_insertionCount = 0;
        }

        /**************************************************************************
	    * 功能描述：在树上创建一个叶子节点代理
	    * 参数说明：aabb    ：aabb变量
	                user    : 数据
	    * 返 回 值：节点的索引来替代指针,来增长我们的节点池
	    ***************************************************************************/
        public int CreateProxy(AABB aabb, ref Body userData)
        {
            //分配一个节点
            int proxyId = AllocateNode();

            // Fatten the aabb.
            Vector2f r = new Vector2f(Settings.aabbExtension, Settings.aabbExtension);
            m_nodes[proxyId].aabb.lowerBound = aabb.lowerBound - r;
            m_nodes[proxyId].aabb.upperBound = aabb.upperBound + r;
            m_nodes[proxyId].userData = userData;
            m_nodes[proxyId].height = 0;
            m_nodes[proxyId].moved = true;

            //插入该节点
            InsertLeaf(proxyId);

            return proxyId;
        }

        /**************************************************************************
	    * 功能描述：销毁一个代理，如果id无效则断言
	    * 参数说明：poxyid :代理id
	    * 返 回 值：节点的索引来替代指针,来增长我们的节点池
	    ***************************************************************************/
        public void DestroyProxy(int proxyId)
        {
            RemoveLeaf(proxyId);
            FreeNode(proxyId);
        }

        /**************************************************************************
	    * 功能描述：移动一个代理并扫描AABB,如果代理移除了使它包围的AABB盒子
	                代理将会从树上移除并重新插入。否则，函数将立即返回
	    * 参数说明：proxyId     ：叶子代理id
				    aabb        : aabb变量
				    displacement：移动坐标向量
	    * 返 回 值： true :移动后需要更新
	                 false:移动后不需要更新

        By "fattening" the AABBs a small amount it is possible to make many displacements
        of the objects before an update is triggered, i.e. when one of the discs moves outside of its fattened AABB.

        Whenever an AABB changes position we need to delete it from the tree 
        then reinsert the new one (at the updated position).
        This can be a costly operation. So we use a fatten AABB to avoid the frequent
        updates of BVH trees.

        输入的aabb应为proxyId对应的物体更新位置后的swept包围盒。
        看新包围盒是否超出了原来的包围盒，如果超出，则需要删除proxyId当前的节点并重新插入，
        否则不用
        http://allenchou.net/2014/02/game-physics-broadphase-dynamic-aabb-tree/
	***************************************************************************/
        public void MoveProxy(int proxyId, AABB aabb)
        {

            //根据proxyId移除叶子
            RemoveLeaf(proxyId);

            m_nodes[proxyId].aabb = aabb;

            InsertLeaf(proxyId);

            m_nodes[proxyId].moved = true;


        }

        /// Get proxy user data.
        /// @return the proxy user data or 0 if the id is invalid.
        public ref Body GetUserData(int proxyId)
        {
            return ref m_nodes[proxyId].userData;
        }
        public bool WasMoved(int proxyId)
        {
            return m_nodes[proxyId].moved;
        }
        public void ClearMoved(int proxyId)
        {
            m_nodes[proxyId].moved = false;
        }

        /// Get the fat AABB for a proxy.
        public AABB GetFatAABB(int proxyId)
        {
            return m_nodes[proxyId].aabb;
        }

        /**************************************************************************
            * 功能描述：查询一个aabb重叠代理，每个重叠提供AABB的代理都将回调回调类
            * 参数说明：callback ：回调对象
                        aabb     ：要查询的aabb
            * 返 回 值：是否有可能碰撞
            ***************************************************************************/

        public int Query(int thisNodeId)
        {
            Stack<int> stack = new Stack<int>();
            stack.Push(m_root);
            AABB aabb = GetFatAABB(thisNodeId);
            while (stack.Count() > 0)
            {
                int nodeId = stack.Pop();
                if (nodeId == nullNode || nodeId == thisNodeId)
                    continue;

                TreeNode node = m_nodes[nodeId];

                //如果包围盒有重叠，就深入到此包围盒
                if (AABB.TestOverlap(node.aabb, aabb))
                {
                    //如果是叶子节点，说明树上有AABB和输入的aabb重叠。
                    if (node.IsLeaf())
                    {

                        return nodeId;

                    }
                    else
                    {
                        //遍历（由于BVH树不是搜索树，其节点顺序无关，可以随意遍历
                        stack.Push(node.child1);
                        stack.Push(node.child2);
                    }
                }

            }
            return -1;
        }




        public int GetHeight()
        {
            if (m_root == nullNode) return 0;
            return m_nodes[m_root].height;
        }

        /// Get the maximum balance of an node in the tree. The balance is the difference
        /// in height of the two children of a node.
        public int GetMaxBalance()
        {
            int maxBalance = 0;
            for (int i = 0; i < m_nodeCapacity; ++i)
            {
                TreeNode node = m_nodes[i];
                if (node.height <= 1) continue;

                int child1 = node.child1;
                int child2 = node.child2;
                int balance = Math.Abs(m_nodes[child2].height - m_nodes[child1].height);
                maxBalance = Math.Max(maxBalance, balance);

            }
            return maxBalance;
        }

        /// Get the ratio of the sum of the node areas to the root area.
        public float GetAreaRatio()
        {
            if (m_root == nullNode) return 0;
            TreeNode root = m_nodes[m_root];
            float rootArea = root.aabb.GetPerimeter();

            float totalArea = 0;
            for (int i = 0; i < m_nodeCapacity; ++i)
            {
                TreeNode node = m_nodes[i];
                if (node.height < 0)
                {
                    continue;
                }
                totalArea += node.aabb.GetPerimeter();
            }
            return totalArea / rootArea;
        }


        /// Shift the world origin. Useful for large worlds.
        /// The shift formula is: position -= newOrigin
        /// @param newOrigin the new origin with respect to the old origin
        public void ShiftOrigin(Vector2f newOrigin)
        {
            for (int i = 0; i < m_nodeCapacity; ++i)
            {
                m_nodes[i].aabb.lowerBound -= newOrigin;
                m_nodes[i].aabb.upperBound -= newOrigin;
            }
        }


        //节点height为-1表示未分配,为0表示刚分配
        private int AllocateNode()
        {
            //空闲节点的第一个
            if (m_freeList == nullNode)
            {
                //没有空闲的节点的话就扩容
                m_nodeCapacity *= 2;
                Array.Resize(ref m_nodes, m_nodeCapacity);

                for (int i = m_nodeCount; i < m_nodeCapacity - 1; ++i)
                {
                    m_nodes[i].next = i + 1;
                    m_nodes[i].height = -1;
                }
                m_nodes[m_nodeCapacity - 1].next = nullNode;
                m_nodes[m_nodeCapacity - 1].height = -1;
                m_freeList = m_nodeCount;
                ;
            }
            int nodeId = m_freeList;
            m_freeList = m_nodes[nodeId].next;
            m_nodes[nodeId].parent = nullNode;
            m_nodes[nodeId].child1 = nullNode;
            m_nodes[nodeId].child2 = nullNode;
            m_nodes[nodeId].height = 0;
            m_nodes[nodeId].userData = null;
            m_nodes[nodeId].moved = false;
            ++m_nodeCount;
            return nodeId;

        }
        // Return a node to the pool.
        private void FreeNode(int nodeId)
        {
            m_nodes[nodeId].next = m_freeList;
            m_nodes[nodeId].height = -1;
            m_freeList = nodeId;
            --m_nodeCount;
        }

        private void InsertLeaf(int leaf)
        {
            ++m_insertionCount;
            if (m_root == nullNode)
            {
                m_root = leaf;
                m_nodes[m_root].parent = nullNode;
                return;
            }
            //SAH：启发式算法。3D下，表面积越大，被射中的概率越大。2D下，周长越大，被光线射中的概率越大。
            //整棵树的代价为中间节点包含的包围盒（求两孩子的并）的面积之和
            // SAH gives us a way to compute the cost of inserting L.
            //The cost is the area of the new parent node plus the increased surface area of all the ancestors.
            //This is the surface area added to the tree.

            //首先要找到最合适的插入位置，即最合适的sibling
            //利用branch and bound(分支界定）来快速枚举，得到插入后代价最小的位置

            AABB leafAABB = m_nodes[leaf].aabb;
            int index = m_root;
            while (m_nodes[index].IsLeaf() == false)
            {
                int child1 = m_nodes[index].child1;
                int child2 = m_nodes[index].child2;

                //当前节点若作为sibling的cost
                float area = m_nodes[index].aabb.GetPerimeter();

                AABB combinedAABB = new AABB();
                combinedAABB.Combine(m_nodes[index].aabb, leafAABB);
                float combinedArea = combinedAABB.GetPerimeter();

                // Cost of creating a new parent for this node and the new leaf
                float cost = 2 * combinedArea;

                // Minimum cost of pushing the leaf further down the tree
                float inheritanceCost = 2 * (combinedArea - area);

                //对比当前节点的两个孩子
                // Cost of descending into child1
                float cost1;
                if (m_nodes[child1].IsLeaf())
                {
                    AABB aabb = new AABB();
                    aabb.Combine(leafAABB, m_nodes[child1].aabb);
                    cost1 = aabb.GetPerimeter() + inheritanceCost;
                }
                else
                {
                    AABB aabb = new AABB();
                    aabb.Combine(leafAABB, m_nodes[child1].aabb);
                    float oldArea = m_nodes[child1].aabb.GetPerimeter();
                    float newArea = aabb.GetPerimeter();
                    cost1 = (newArea - oldArea) + inheritanceCost;
                }

                // Cost of descending into child2
                float cost2;
                if (m_nodes[child2].IsLeaf())
                {
                    AABB aabb = new AABB();
                    aabb.Combine(leafAABB, m_nodes[child2].aabb);
                    cost2 = aabb.GetPerimeter() + inheritanceCost;
                }
                else
                {
                    AABB aabb = new AABB();
                    aabb.Combine(leafAABB, m_nodes[child2].aabb);
                    float oldArea = m_nodes[child2].aabb.GetPerimeter();
                    float newArea = aabb.GetPerimeter();
                    cost2 = (newArea - oldArea) + inheritanceCost;
                }
                // Descend according to the minimum cost.
                if (cost < cost1 && cost < cost2)
                {
                    break;
                }

                // Descend
                if (cost1 < cost2)
                {
                    index = child1;
                }
                else
                {
                    index = child2;
                }

            }

            //找到sibling后，插入到合适的位置
            //deals with all the details of modify the tree after a sibling has been chosen. 
            // Edge cases must be handled.
            int sibling = index;
            //将sibling的父节点作为新插入节点的父节点的父节点
            int oldParent = m_nodes[sibling].parent;
            //分配一个新的父节点
            int newParent = AllocateNode();
            m_nodes[newParent].parent = oldParent;
            m_nodes[newParent].userData = null;
            m_nodes[newParent].aabb.Combine(leafAABB, m_nodes[sibling].aabb);
            m_nodes[newParent].height = m_nodes[sibling].height + 1;

            if (oldParent != nullNode)
            {
                // The sibling was not the root.
                if (m_nodes[oldParent].child1 == sibling)
                {
                    m_nodes[oldParent].child1 = newParent;
                }
                else
                {
                    m_nodes[oldParent].child2 = newParent;
                }

                m_nodes[newParent].child1 = sibling;
                m_nodes[newParent].child2 = leaf;
                m_nodes[sibling].parent = newParent;
                m_nodes[leaf].parent = newParent;
            }
            else
            {
                // The sibling was the root.
                m_nodes[newParent].child1 = sibling;
                m_nodes[newParent].child2 = leaf;
                m_nodes[sibling].parent = newParent;
                m_nodes[leaf].parent = newParent;
                m_root = newParent;
            }

            //refitting the tree
            //adjusts the AABBs of the new leaf’s ancestors. This is called refitting.
            // Walk back up the tree fixing heights and AABBs
            index = m_nodes[leaf].parent;
            while (index != nullNode)
            {
                index = Balance(index);

                int child1 = m_nodes[index].child1;
                int child2 = m_nodes[index].child2;

                m_nodes[index].height = 1 + Math.Max(m_nodes[child1].height, m_nodes[child2].height);
                m_nodes[index].aabb.Combine(m_nodes[child1].aabb, m_nodes[child2].aabb);

                index = m_nodes[index].parent;
            }
        }

        private void RemoveLeaf(int leaf)
        {
            if (leaf == m_root)
            {
                m_root = nullNode;
                return;
            }
            int parent = m_nodes[leaf].parent;
            int grandParent = m_nodes[parent].parent;
            int sibling;
            if (m_nodes[parent].child1 == leaf)
            {
                sibling = m_nodes[parent].child2;
            }
            else
            {
                sibling = m_nodes[parent].child1;
            }

            if (grandParent != nullNode)
            {
                // Destroy parent and connect sibling to grandParent.
                if (m_nodes[grandParent].child1 == parent)
                {
                    m_nodes[grandParent].child1 = sibling;
                }
                else
                {
                    m_nodes[grandParent].child2 = sibling;
                }
                m_nodes[sibling].parent = grandParent;
                FreeNode(parent);

                // Adjust ancestor bounds.
                int index = grandParent;
                while (index != nullNode)
                {
                    index = Balance(index);

                    int child1 = m_nodes[index].child1;
                    int child2 = m_nodes[index].child2;

                    m_nodes[index].aabb.Combine(m_nodes[child1].aabb, m_nodes[child2].aabb);
                    m_nodes[index].height = 1 + Math.Max(m_nodes[child1].height, m_nodes[child2].height);

                    index = m_nodes[index].parent;
                }
            }
            else
            {
                m_root = sibling;
                m_nodes[sibling].parent = nullNode;
                FreeNode(parent);
            }
        }

        // Perform a left or right rotation if node A is imbalanced.
        // Returns the new root index.
        private int Balance(int iA)
        {
            TreeNode A = m_nodes[iA];
            if (m_nodes[iA].IsLeaf() || m_nodes[iA].height < 2)
            {
                return iA;
            }

            int iB = m_nodes[iA].child1;
            int iC = m_nodes[iA].child2;




            int balance = m_nodes[iC].height - m_nodes[iB].height;

            // Rotate C up
            if (balance > 1)
            {
                int iF = m_nodes[iC].child1;
                int iG = m_nodes[iC].child2;


                // Swap A and C
                m_nodes[iC].child1 = iA;
                m_nodes[iC].parent = m_nodes[iA].parent;
                m_nodes[iA].parent = iC;

                // A's old parent should point to C
                if (m_nodes[iC].parent != nullNode)
                {
                    if (m_nodes[m_nodes[iC].parent].child1 == iA)
                    {
                        m_nodes[m_nodes[iC].parent].child1 = iC;
                    }
                    else
                    {

                        m_nodes[m_nodes[iC].parent].child2 = iC;
                    }
                }
                else
                {
                    m_root = iC;
                }

                // Rotate
                if (m_nodes[iF].height > m_nodes[iG].height)
                {
                    m_nodes[iC].child2 = iF;
                    m_nodes[iA].child2 = iG;
                    m_nodes[iG].parent = iA;
                    m_nodes[iA].aabb.Combine(m_nodes[iB].aabb, m_nodes[iG].aabb);
                    m_nodes[iC].aabb.Combine(m_nodes[iA].aabb, m_nodes[iF].aabb);

                    m_nodes[iA].height = 1 + Math.Max(m_nodes[iB].height, m_nodes[iG].height);
                    m_nodes[iC].height = 1 + Math.Max(m_nodes[iA].height, m_nodes[iF].height);


                }
                else
                {
                    m_nodes[iC].child2 = iG;
                    m_nodes[iA].child2 = iF;
                    m_nodes[iF].parent = iA;
                    m_nodes[iA].aabb.Combine(m_nodes[iB].aabb, m_nodes[iF].aabb);
                    m_nodes[iC].aabb.Combine(m_nodes[iA].aabb, m_nodes[iG].aabb);

                    m_nodes[iA].height = 1 + Math.Max(m_nodes[iB].height, m_nodes[iF].height);
                    m_nodes[iC].height = 1 + Math.Max(m_nodes[iA].height, m_nodes[iG].height);

                }

                return iC;
            }

            // Rotate B up
            if (balance < -1)
            {
                int iD = m_nodes[iB].child1;
                int iE = m_nodes[iB].child2;

                // Swap A and B
                m_nodes[iB].child1 = iA;
                m_nodes[iB].parent = m_nodes[iA].parent;
                m_nodes[iA].parent = iB;

                // A's old parent should point to B
                if (m_nodes[iB].parent != nullNode)
                {
                    if (m_nodes[m_nodes[iB].parent].child1 == iA)
                    {
                        m_nodes[m_nodes[iB].parent].child1 = iB;
                    }
                    else
                    {

                        m_nodes[m_nodes[iB].parent].child2 = iB;
                    }
                }
                else
                {
                    m_root = iB;
                }

                // Rotate
                if (m_nodes[iD].height > m_nodes[iE].height)
                {
                    m_nodes[iB].child2 = iD;
                    m_nodes[iA].child1 = iE;
                    m_nodes[iE].parent = iA;
                    m_nodes[iA].aabb.Combine(m_nodes[iC].aabb, m_nodes[iE].aabb);
                    m_nodes[iB].aabb.Combine(m_nodes[iA].aabb, m_nodes[iD].aabb);

                    m_nodes[iA].height = 1 + Math.Max(m_nodes[iC].height, m_nodes[iE].height);
                    m_nodes[iB].height = 1 + Math.Max(m_nodes[iA].height, m_nodes[iD].height);

                }
                else
                {
                    m_nodes[iB].child2 = iE;
                    m_nodes[iA].child1 = iD;
                    m_nodes[iD].parent = iA;
                    m_nodes[iA].aabb.Combine(m_nodes[iC].aabb, m_nodes[iD].aabb);
                    m_nodes[iB].aabb.Combine(m_nodes[iA].aabb, m_nodes[iE].aabb);

                    m_nodes[iA].height = 1 + Math.Max(m_nodes[iC].height, m_nodes[iD].height);
                    m_nodes[iB].height = 1 + Math.Max(m_nodes[iA].height, m_nodes[iE].height);

                }

                return iB;
            }

            return iA;
        }

        private int ComputeHeight()
        {
            int height = ComputeHeight(m_root);
            return height;
        }
        private int ComputeHeight(int nodeId)
        {

            TreeNode node = m_nodes[nodeId];

            if (node.IsLeaf())
            {
                return 0;
            }

            int height1 = ComputeHeight(node.child1);
            int height2 = ComputeHeight(node.child2);
            return 1 + Math.Max(height1, height2);
        }




    }
}
