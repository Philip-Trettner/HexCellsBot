using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexCellsBot.Logic
{
    public class NumberConstraint
    {
        private static int nextID = 0;
        public readonly int ID = ++nextID;

        public bool ForceConnected = false;
        public bool ForceNonConnected = false;
        public readonly int Count;

        public readonly List<Cell> Cells = new List<Cell>();

        public string ExtraInfo;

        public string IDString => $"Rule #{ID}";

        // rule without blacks or blues
        public NumberConstraint PureRule
        {
            get
            {
                var bc = Cells.Count(c => c.State == CellState.Blue);
                return new NumberConstraint(Count - bc, Cells.Where(c => c.State == CellState.Yellow));
            }
        }

        public bool IsRelevant
        {
            get
            {
                if (Cells.All(c => c.State != CellState.Yellow))
                    return false;

                return true;
            }
        }

        // Cells can contain null
        public NumberConstraint(int cnt, IEnumerable<Cell> cells)
        {
            Count = cnt;
            foreach (var c in cells)
                if (c != null)
                    Cells.Add(c);
        }

        public override string ToString()
        {
            var s = "#" + ID + ":";
            if (!string.IsNullOrEmpty(ExtraInfo))
                s += " " + ExtraInfo + ",";
            s += " Exactly " + Count + " Cells of {";
            s += Cells.Select(c => c.ToString()).Aggregate((s1, s2) => s1 + ", " + s2);
            s += "}";
            return s;
        }

        public bool SameAs(NumberConstraint nc)
        {
            if (Count != nc.Count)
                return false;

            if (Cells.Count != nc.Cells.Count)
                return false;

            foreach (var cell in Cells)
                if (!nc.Cells.Contains(cell))
                    return false;

            return true;
        }

        public bool IsStrictSubsetOf(NumberConstraint nc)
        {
            if (Cells.Count == nc.Cells.Count)
                return false; // not a strict subset

            foreach (var c in Cells)
                if (!nc.Cells.Contains(c))
                    return false;

            return true;
        }

        public NumberConstraint Without(NumberConstraint nc)
        {
            Debug.Assert(nc.IsStrictSubsetOf(this));

            var newCells = Cells.Where(c => !nc.Cells.Contains(c));
            var newCnt = Count - nc.Count;
            return new NumberConstraint(newCnt, newCells) { ExtraInfo = IDString + " without " + nc.IDString };
        }

        public bool HasCutWith(NumberConstraint nc2)
        {
            return Cells.Any(c => nc2.Cells.Contains(c));
        }

        // Checks if cut between nc and this has meaningful rule
        public NumberConstraint ProbeCutWith(NumberConstraint nc)
        {
            var nc1only = Cells.Where(c => !nc.Cells.Contains(c)).ToArray();
            var nc2only = nc.Cells.Where(c => !Cells.Contains(c)).ToArray();
            var bothCells = Cells.Where(c => nc.Cells.Contains(c)).ToArray();

            if (nc.Count > nc2only.Length)
        }
    }
}
