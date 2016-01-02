using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace HexCellsBot.Logic
{
    public class ColumnConstraint
    {
        public readonly Cell Seed;
        public readonly int AngleInDeg;
        public readonly Model Model;
        public string Text;
        public readonly ivec2 TextCenter;

        // ordered list of cells for this column
        public readonly List<Cell> Cells = new List<Cell>();

        public ColumnConstraint(Cell seed, int angleInDeg, string text, Model m, ivec2 textCenter)
        {
            Model = m;
            TextCenter = textCenter;
            Seed = seed;
            AngleInDeg = angleInDeg;
            Text = text;

            var origin = (vec2)seed.Center;
            var dir = vec2.FromAngle(new vec2(0, -1).Angle + angleInDeg / 180.0 * Math.PI);
            var n = new vec2(-dir.y, dir.x);
            var d = vec2.Dot(n, origin);

            foreach (var c in m.Cells)
            {
                var center = (vec2)c.Center;

                var dis = vec2.Dot(center, n) - d;

                var prog = vec2.Dot(center, dir) - vec2.Dot(origin, dir);
                if (Math.Abs(dis) < c.Radius / 2f && prog < c.Radius / 2f)
                    Cells.Add(c);
            }
            Cells.Sort((c1, c2) => vec2.Dot(-dir, c1.Center).CompareTo(vec2.Dot(-dir, c2.Center)));

            Debug.Assert(Cells.Contains(seed));
        }

        public NumberConstraint Constraint
        {
            get
            {
                if (!string.IsNullOrEmpty(Text) && Text != "?")
                {
                    if (Text == "O")
                        Text = "0";

                    var type = ConstraintType.Equal;
                    var txt = Text;

                    if (Text.StartsWith("{"))
                    {
                        type = ConstraintType.Connected;
                        txt = Text.Trim('{', '}');
                    }
                    if (Text.StartsWith("-") || Text.StartsWith(".") || Text.StartsWith("_"))
                    {
                        type = ConstraintType.NonConnected;
                        txt = Text.Trim('-', '.', '_');
                    }

                    int cnt;
                    if (!int.TryParse(txt, out cnt))
                    {
                        throw new InvalidOperationException("Invalid Number '" + Text + "'");
                    }

                    return new NumberConstraint(cnt, Cells, type, ConnectionModel.FromColumn(this)) { ExtraInfo = "From Column at " + AngleInDeg + " degree on " + Seed };
                }

                return null;
            }
        }
    }
}
