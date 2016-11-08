namespace ArtificialLife
{
    public static class Drawing
    {
        public static string DrawLine(Coord c1, Coord c2, uint width, string color)
        {
            c1 = 10 * c1;
            c2 = 10 * c2;

            if (c1 == c2)
            {
                c1 = c1 + new Coord(-4, 1);
                c2 = c2 + new Coord(+4, 1);
            }

            return string.Format("<line x1=\"{0}\" y1=\"{1}\" x2=\"{2}\" y2=\"{3}\" style=\"stroke:{4};stroke-width:{5}\" />", c1.X, Const.SvgHeight - c1.Y, c2.X, Const.SvgHeight - c2.Y, color, width);
        }

        public static string DrawCircle(Coord c)
        {
            return string.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"2\" fill=\"blue\" />", (10 * c).X, Const.SvgHeight - (10 * c).Y);
        }
    }
}
