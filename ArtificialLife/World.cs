using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialLife
{
    public class World
    {
        private Random random = new Random();

        private readonly List<Tree> m_trees = new List<Tree>();
        private readonly List<uint> m_newTrees = new List<uint>();

        private Dictionary<int, List<Leaf>> m_allLeaves = new Dictionary<int, List<Leaf>>();
        private List<string> m_codes;
        private List<uint> m_scores;

        public World(IEnumerable<string> codes)
        {
            this.m_codes = new List<string>(codes);
            this.m_scores = new List<uint>();

            for (uint i = 0; i < this.m_codes.Count; i++)
            {
               this.m_newTrees.Add(i);
               this.m_scores.Add(0);
            }

            for (int i = 0; i < Const.NumColumns; i++)
            {
                this.m_allLeaves.Add(i, new List<Leaf>());
            }
        }

        public bool IsValid(Coord c)
        {
            return c.X >= 0 && c.X < Const.NumColumns;
        }

        public void AddTree(uint codeId)
        {
            this.m_newTrees.Add(codeId);
        }

        public void Step()
        {
            for (int i = 0; i <this.m_newTrees.Count; i++)
            {
                int rnd = random.Next(0, Const.NumColumns);
                this.m_trees.Add(new Tree(this, new Coord(rnd, 0), this.m_newTrees[i]));
            }

           this.m_newTrees.Clear();

            // light
            for (int c = 0; c < Const.NumColumns; ++c)
            {
                uint food = 256;
                foreach (Leaf leaf in this.m_allLeaves[c])
                {
                    leaf.AddLight(food / 2);
                    food -= food / 2;
                }
            }

            List<Tree> clone = new List<Tree>(m_trees);
            foreach (Tree tree in clone)
            {
                tree.Grow();

                int rnd = random.Next(1000);
                if (rnd == 0)
                {
                   this.m_trees.Remove(tree);
                   tree.Dispose();
                }
            }
        }

        public void InsertLeaf(Coord c, Leaf leaf)
        {
            this.m_allLeaves[c.X].Insert(0, leaf);
            this.m_allLeaves[c.X].Sort((a, b) => Leaf.IsHigher(b, a));
        }

        public void RemoveLeaf(Coord c, Leaf leaf)
        {
            this.m_allLeaves.First(kvp => kvp.Value.Contains(leaf)).Value.Remove(leaf);
        }

        public string Draw()
        {
            StringBuilder svg = new StringBuilder();
            svg.AppendFormat("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"{0}\" height=\"{1}\">\n", 10 * Const.NumColumns, Const.SvgHeight);

            foreach (Tree tree in this.m_trees)
            {
                tree.Draw(svg);
            }

            svg.AppendLine("</svg>");

            return svg.ToString();
        }

        public void AddToScore(uint id, uint add)
        {
           this.m_scores[(int)id] += add;
        }

        public string GetCode(uint id)
        {
            return this.m_codes[(int)id];
        }

        public void PrintScores()
        {
            Dictionary<string, int> results = new Dictionary<string, int>();

            for (int id = 0; id <this.m_scores.Count; ++id)
            {
                results.Add(this.m_codes[id], (int)this.m_scores[id]);
            }

            Console.WriteLine("Scores:\n");

            foreach (KeyValuePair<string, int> kvp in results)
            {
                Console.WriteLine(kvp.Value / 1000000 + "M : " + kvp.Key);
            }
        }
    }
}
