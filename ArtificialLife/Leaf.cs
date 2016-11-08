using System;
using System.Text;

namespace ArtificialLife
{
    public class Leaf : IDisposable
    {
        private Coord m_coord;
        private Tree m_tree;

        public Leaf(Coord c, Tree t)
        {
            this.m_coord = c;
            this.m_tree = t;

            this.m_tree.World.InsertLeaf(this.m_coord, this);
        }

        ~Leaf()
        {
            this.Dispose(false);
        }

        public void AddLight( uint light )
        {
          this.m_tree.AddFood( light / 2 );
        }

        public void Draw(StringBuilder svg)
        {
            svg.AppendLine(Drawing.DrawLine(this.m_coord, this.m_coord, 1, "green"));
        }

        public static int IsHigher(Leaf lhs, Leaf rhs)
        {
            return lhs.m_coord.Y.CompareTo(rhs.m_coord.Y);
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
                this.m_tree.World.RemoveLeaf(this.m_coord, this);
            }
        }
    }
}
