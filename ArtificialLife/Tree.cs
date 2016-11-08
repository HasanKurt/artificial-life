using System;
using System.Collections.Generic;
using System.Text;

namespace ArtificialLife
{
    public class Tree : IDisposable
    {
        private readonly uint m_codeId;
        private readonly List<Branch> m_branches = new List<Branch>();
        private readonly List<Leaf> m_leaves = new List<Leaf>();
        private uint m_branchThatGrows;
        private uint m_food;

        public World World { get; private set; }

        public Coord Start { get; private set; }

        public Tree(World world, Coord start, uint codeId)
        {
            this.World = world;
            this.Start = start;
            this.m_codeId = codeId;
            this.m_food = Const.FoodStart;

            this.AddBranch(null, this.CodePoint(0));
        }

        ~Tree()
        {
            this.Dispose(false);
        }

        public string CodePoint(uint branchId)
        {
            string p = this.World.GetCode(m_codeId).ToString();

            while (p.Length > 0 && branchId != 0)
            {
                if (p[0] == ',')
                {
                    --branchId;
                }

                p = p.Remove(0, 1);
            }

            return p;
        }

        public void AddBranch(Branch prev, string p)
        {
            this.m_branches.Add(new Branch(prev, p, this));
        }

        public void AddLeaf(Coord c)
        {
            this.m_leaves.Add(new Leaf(c, this));
        }

        public void Grow()
        {
            for (uint i = 0; this.m_food > 0 && i < this.m_branches.Count; ++i)
            {
                this.m_branches[(int)this.m_branchThatGrows].Grow(ref this.m_food);
                this.m_branchThatGrows = (uint)((this.m_branchThatGrows + 1) % this.m_branches.Count);
            }
        }

        public void AddFood(uint food)
        {
            this.m_food += food;
            this.World.AddToScore(this.m_codeId, food);
        }

        public void Reproduce()
        {
            this.World.AddTree(this.m_codeId);
        }

        public void Draw(StringBuilder svg)
        {
            foreach (Branch b in this.m_branches)
            {
                b.Draw(svg);
            }

            foreach (Leaf l in this.m_leaves)
            {
                l.Draw(svg);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Leaf leaf in this.m_leaves)
                {
                    leaf.Dispose();
                }
            }
        }
    }
}
