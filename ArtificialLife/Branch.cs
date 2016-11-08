using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ArtificialLife
{
    public class Branch
    {
        private List<Coord> m_twigs = new List<Coord>();
        private Branch m_prev;
        private string m_p;
        private Tree m_tree;
        uint m_width = 0;
        uint m_localFood = 0;
        private List<Coord> m_seeds = new List<Coord>();

        public Branch(Branch prev, string ptr, Tree t)
        {
            this.m_prev = prev;
            this.m_p = ptr;
            this.m_tree = t;
        }

        public void Grow(ref uint food)
        {
            if (m_p.Length == 0 || this.m_p[0] == ',')
            {
                food += this.m_localFood;
                this.m_localFood = 0;
                return;
            }

            this.m_localFood += food;
            food = 0;

            uint leafCost = 1000;
            uint twigCost = this.GrowCost(1) + 1;

            char ch = this.m_p[0];
            if (ch == 'b' && this.m_localFood >= leafCost)
            {
                this.m_tree.AddLeaf(this.Tip());
                this.m_p = this.m_p.Remove(0, 1);
                food = this.m_localFood - leafCost;
                this.m_localFood = 0;
            }
            else if (ch == '<' && this.m_localFood >= twigCost && this.AddTwig(new Coord(-1, 0)))
            {
                this.m_p = this.m_p.Remove(0, 1);
                food = this.m_localFood - twigCost;
                this.m_localFood = 0;
            }
            else if (ch == '>' && this.m_localFood >= twigCost && this.AddTwig(new Coord(1, 0)))
            {
                this.m_p = this.m_p.Remove(0, 1); 
                food = this.m_localFood - twigCost;
                this.m_localFood = 0;
            }
            else if (ch == '^' && this.m_localFood >= twigCost && this.AddTwig(new Coord(0, 1)))
            {
                this.m_p = this.m_p.Remove(0, 1);
                food = this.m_localFood - twigCost;
                this.m_localFood = 0;
            }
            else if (char.IsDigit(ch))
            {
                int digit = int.Parse("" + ch);
                this.m_tree.AddBranch(this, this.m_tree.CodePoint((uint)digit));
                this.m_tree.AddBranch(this, this.m_p.Substring(1));
                this.m_p = string.Empty;
                food = this.m_localFood;
                this.m_localFood = 0;
            }
            else if (ch == 'z' && this.m_localFood >= Const.FoodStart)
            {
                this.m_seeds.Add(this.Tip());
                this.m_tree.Reproduce();
                this.m_p = this.m_p.Remove(0, 1);
                food = this.m_localFood - Const.FoodStart;
                this.m_localFood = 0;
            }
        }

        private bool AddTwig(Coord direction)
        {
            Coord newC = this.Tip() + direction;
            if (m_tree.World.IsValid(newC))
            {
                this.m_twigs.Add(newC);
                this.CorrectWidth(0);
                return true;
            }
            return false;
        }

        private Coord Tip()
        {
            if (m_twigs.Count == 0)
            {
                return this.Start();
            }

            return this.m_twigs[m_twigs.Count - 1];
        }

        private Coord Start()
        {
            if (m_prev != null)
            {
                return this.m_prev.Tip();
            }

            return this.m_tree.Start;
        }

        private uint GrowCost(uint tipWidth)
        {
            Debug.Assert(tipWidth + this.m_twigs.Count <= 1 + this.m_width);

            uint newWidth = this.m_width;
            uint cost = 0;

            if (tipWidth + this.m_twigs.Count == 1 + this.m_width) // need to grow
            {
                for (uint i = 0; i != this.m_twigs.Count; ++i)
                {
                    cost += (uint)(1 << (int)(tipWidth + i));
                }
                ++newWidth;
            }

            if (m_prev != null)
            {
                cost += this.m_prev.GrowCost(newWidth);
            }

            return cost;
        }

        private void CorrectWidth(uint tipWidth)
        {
            this.m_width = (uint)Math.Max(m_width, tipWidth + this.m_twigs.Count);

            if (m_prev != null)
            {
                this.m_prev.CorrectWidth(m_width);
            }
        }

        public void Draw(StringBuilder svg)
        {
            Coord c = this.Start();

            for (int i = 0; i != this.m_twigs.Count; ++i)
            {
                svg.AppendLine(Drawing.DrawLine(c, this.m_twigs[i], (uint)((m_width + this.m_twigs.Count - 1 - i) / 10 + 1), "red"));
                c = this.m_twigs[i];
            }

            foreach (Coord seed in this.m_seeds)
            {
                svg.AppendLine(Drawing.DrawCircle(seed));
            }
        }
    }
}
