using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexCellsBot.Logic
{
    public enum ConstraintType
    {
        Vanilla,
        Connected,
        NonConnected
    }

    public class NumberConstraint
    {
        private static int nextID = 0;
        public readonly int ID = ++nextID;

        public readonly ConstraintType Type;
        public readonly int Count;

        public readonly List<Cell> Cells = new List<Cell>();
        public ConnectionModel ConnectionModel;

        public string ExtraInfo;

        public string IDString => $"Rule #{ID}";

        // rule without blacks or blues
        public NumberConstraint PureRule
        {
            get
            {
                if (Type != ConstraintType.Vanilla)
                    return null; // only works for vanilla

                var bc = Cells.Count(c => c.State == CellState.Blue);
                return new NumberConstraint(Count - bc, Cells.Where(c => c.State == CellState.Yellow), ConstraintType.Vanilla, null) { ExtraInfo = "Pure of " + IDString };
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
        public NumberConstraint(int cnt, IEnumerable<Cell> cells, ConstraintType type, ConnectionModel connectionModel)
        {
            ConnectionModel = connectionModel;
            Type = type;
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
            s += " Exactly " + Count;
            if (Type != ConstraintType.Vanilla)
                s += " " + Type;
            s += " Cells of {";
            s += Cells.Select(c => c.ToString()).Aggregate((s1, s2) => s1 + ", " + s2);
            s += "}";
            return s;
        }

        public bool SameAs(NumberConstraint nc)
        {
            if (Type != nc.Type)
                return false;

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

        // does not work with non.vanilla
        public NumberConstraint Without(NumberConstraint nc)
        {
            Debug.Assert(Type == ConstraintType.Vanilla);
            Debug.Assert(nc.Type == ConstraintType.Vanilla);
            Debug.Assert(nc.IsStrictSubsetOf(this));

            var newCells = Cells.Where(c => !nc.Cells.Contains(c));
            var newCnt = Count - nc.Count;
            return new NumberConstraint(newCnt, newCells, ConstraintType.Vanilla, null) { ExtraInfo = IDString + " without " + nc.IDString };
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

            // blues from RHS leak into this
            if (nc.Count > nc2only.Length)
            {
                if (nc.Count - nc2only.Length >= Count)
                {
                    if (nc.Count - nc2only.Length > Count)
                        throw new InvalidOperationException("Hm, something is fishey");

                    return new NumberConstraint(0, nc1only, ConstraintType.Vanilla, null) { ExtraInfo = $"{nc.IDString} forces these cells to be empty" };
                }
            }

            // blues from this leak into rhs
            if (Count > nc1only.Length)
            {
                if (nc.Count <= Count - nc1only.Length)
                {
                    if (nc.Count < Count - nc1only.Length)
                        throw new InvalidOperationException("Hm, something is fishey");

                    return new NumberConstraint(nc1only.Length, nc1only, ConstraintType.Vanilla, null) { ExtraInfo = $"{nc.IDString} forces these cells to be all-blue" };
                }
            }

            return null;
        }
    }
}
