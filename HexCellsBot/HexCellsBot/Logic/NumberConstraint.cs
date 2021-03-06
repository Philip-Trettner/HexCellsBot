﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexCellsBot.Logic
{
    public enum ConstraintType
    {
        Equal,
        Connected,
        NonConnected,
        Minimum,
        Maximum
    }

    public class NumberConstraint
    {
        public static int NextID = 0;
        public int Generation;
        public readonly int ID = ++NextID;

        public readonly ConstraintType Type;
        public readonly int Count;

        public readonly List<Cell> Cells = new List<Cell>();
        public ConnectionModel ConnectionModel;

        public string ExtraInfo;

        public string IDString => $"Rule #{ID}";

        public bool HasEqualSemantics
        {
            get
            {
                switch (Type)
                {
                    case ConstraintType.Equal:
                    case ConstraintType.Connected:
                    case ConstraintType.NonConnected:
                        return true;
                    case ConstraintType.Minimum:
                    case ConstraintType.Maximum:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public bool HasGreaterOrEqualSemantics
        {
            get
            {
                switch (Type)
                {
                    case ConstraintType.Equal:
                    case ConstraintType.Connected:
                    case ConstraintType.NonConnected:
                    case ConstraintType.Minimum:
                        return true;
                    case ConstraintType.Maximum:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public bool HasLesserOrEqualSemantics
        {
            get
            {
                switch (Type)
                {
                    case ConstraintType.Equal:
                    case ConstraintType.Connected:
                    case ConstraintType.NonConnected:
                    case ConstraintType.Maximum:
                        return true;
                    case ConstraintType.Minimum:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        // rule without blacks or blues
        public NumberConstraint PureRule
        {
            get
            {
                if (Type != ConstraintType.Equal)
                    return null; // only works for vanilla

                var bc = Cells.Count(c => c.State == CellState.Blue);
                return new NumberConstraint(Count - bc, Cells.Where(c => c.State == CellState.Yellow), ConstraintType.Equal, null) { ExtraInfo = "Pure of " + IDString };
            }
        }

        // rule without blacks
        public NumberConstraint PositiveRule
        {
            get
            {
                // works for every type
                //f (Type != ConstraintType.Vanilla)

                return new NumberConstraint(Count, Cells.Where(c => c.State != CellState.Black), Type, ConnectionModel) { ExtraInfo = "Positive of " + IDString };
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
            if (cnt < 0)
                throw new ArgumentOutOfRangeException("cnt");

            ConnectionModel = connectionModel;
            Type = type;
            Count = cnt;
            foreach (var c in cells)
                if (c != null)
                    Cells.Add(c);

            if (HasGreaterOrEqualSemantics && Cells.Count(c => c.State != CellState.Black) < Count)
                throw new Exception("Self-Inconsistent constraint");
        }

        public string ConstraintString
        {
            get
            {
                var s = "";
                switch (Type)
                {
                    case ConstraintType.Equal:
                        s += "Exactly " + Count;
                        break;
                    case ConstraintType.Connected:
                        s += Count + " connected";
                        break;
                    case ConstraintType.NonConnected:
                        s += Count + " non-connected";
                        break;
                    case ConstraintType.Minimum:
                        s += "At least " + Count;
                        break;
                    case ConstraintType.Maximum:
                        s += "At most " + Count;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                s += " Cell" + (Count > 1 ? "s" : "");
                return s;
            }
        }

        public string CellString
        {
            get
            {
                var s = "";
                if (Cells.Count > 6)
                    s += "... " + Cells.Count + " cells ...";
                else
                    s += Cells.Select(c => $"#{c.Nr} ({c.State})").Aggregate((s1, s2) => s1 + ", " + s2);
                return s;
            }
        }

        public string CellCompleteString
        {
            get
            {
                var s = "";
                s += Cells.Select(c => $"#{c.Nr} ({c.State})").Aggregate((s1, s2) => s1 + ", " + s2);
                return s;
            }
        }

        public override string ToString()
        {
            var s = "#" + ID + ":";
            if (!string.IsNullOrEmpty(ExtraInfo))
                s += " " + ExtraInfo + ",";
            s += " " + ConstraintString + " of {";
            s += CellString;
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
            Debug.Assert(Type == ConstraintType.Equal);
            Debug.Assert(nc.HasEqualSemantics);
            Debug.Assert(nc.IsStrictSubsetOf(this));

            var newCells = Cells.Where(c => !nc.Cells.Contains(c));
            var newCnt = Count - nc.Count;
            return new NumberConstraint(newCnt, newCells, ConstraintType.Equal, null) { ExtraInfo = IDString + " without " + nc.IDString };
        }

        public bool HasCutWith(NumberConstraint nc2)
        {
            return Cells.Any(c => nc2.Cells.Contains(c));
        }

        // Checks if cut between nc and this has meaningful rule
        public IEnumerable<NumberConstraint> ProbeCutWith(NumberConstraint nc)
        {
            var nc1only = Cells.Where(c => !nc.Cells.Contains(c)).ToArray();
            var nc2only = nc.Cells.Where(c => !Cells.Contains(c)).ToArray();
            var bothCells = Cells.Where(c => nc.Cells.Contains(c)).ToArray();

            // blues from RHS leak into this
            if (HasLesserOrEqualSemantics && nc.HasGreaterOrEqualSemantics)
                if (nc.Count - nc2only.Length >= Count)
                {
                    if (nc.Count - nc2only.Length > Count)
                        throw new InvalidOperationException("Hm, something is fishey");

                    yield return new NumberConstraint(0, nc1only, ConstraintType.Equal, null)
                    {
                        ExtraInfo = $"{IDString} and {nc.IDString} force these cells to be empty"
                    };
                }

            // blues from this leak into rhs
            if (HasGreaterOrEqualSemantics && nc.HasLesserOrEqualSemantics)
                if (nc.Count <= Count - nc1only.Length)
                {
                    if (nc.Count < Count - nc1only.Length)
                        throw new InvalidOperationException("Hm, something is fishey");

                    yield return new NumberConstraint(nc1only.Length, nc1only, ConstraintType.Equal, null)
                    {
                        ExtraInfo = $"{IDString} and {nc.IDString} force these cells to be all-blue"
                    };
                }

            // guaranteed min nr of cells bleeding into this
            if (nc.HasGreaterOrEqualSemantics)
                if (nc.Count > nc2only.Length)
                {
                    yield return new NumberConstraint(nc.Count - nc2only.Length, bothCells, ConstraintType.Minimum, null)
                    {
                        ExtraInfo = $"{bothCells.Length} cells of {IDString} overlap {nc.IDString}, with at least {nc.Count - nc2only.Length} blue ones in the overlap"
                    };
                }
        }

        public IEnumerable<NumberConstraint> DerivativeRules
        {
            get
            {
                // special -2- case for 4 connected cells
                if (Type == ConstraintType.NonConnected && Count == 2)
                {
                    foreach (var nc in ConnectionModel.SpecialNonConnected2Derivatives)
                        yield return nc;
                }

                if (Type == ConstraintType.NonConnected)
                    foreach (var nc in ConnectionModel.NonConnectedMaximumDerivatives(this))
                        yield return nc;
            }
        }
    }
}
