using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexCellsBot.Logic
{
    public class ConnectionModel
    {
        public bool RingBuffer = true;
        public Cell[] Cells;

        public IEnumerable<NumberConstraint> NonConnectedMaximumDerivatives(NumberConstraint nc)
        {
            var endIdx = RingBuffer ? Cells.Length : Cells.Length - nc.Count + 1;
            for (var start = 0; start < endIdx; ++start)
            {
                var cells = new Cell[nc.Count];
                for (var i = start; i < start + nc.Count; ++i)
                    cells[i - start] = Cells[i % Cells.Length];
                if (cells.All(c => c != null) && cells.Any(c => c.State == CellState.Yellow))
                    yield return new NumberConstraint(nc.Count - 1, cells, ConstraintType.Maximum, null);
            }
        }

        public IEnumerable<NumberConstraint> SpecialNonConnected2Derivatives
        {
            get
            {
                if (Cells.Length < 4)
                    yield break;
                if (Cells.Count(c => (c?.State ?? CellState.Black) != CellState.Black) > 4)
                    yield break; // only works for EXACLTY 4

                var states = Cells.Select(c => c?.State ?? CellState.Black).ToArray();

                const int ncCount = 4;
                var endIdx = RingBuffer ? Cells.Length : Cells.Length - ncCount + 1;
                var okCnt = 0;
                var okStart = 0;
                for (var start = 0; start < endIdx; ++start)
                {
                    var ok = true;
                    for (var i = start; i < start + ncCount; ++i)
                        if (states[i % Cells.Length] != CellState.Yellow)
                        {
                            ok = false;
                            break;
                        }

                    if (RingBuffer)
                    {
                        if (states[(start - 1 + Cells.Length) % Cells.Length] != CellState.Black)
                            ok = false;
                        if (states[(start + ncCount + Cells.Length) % Cells.Length] != CellState.Black)
                            ok = false;
                    }
                    else
                    {
                        if (start > 0 && states[start - 1] != CellState.Black)
                            ok = false;
                        if (start + ncCount < Cells.Length && states[start + ncCount] != CellState.Black)
                            ok = false;
                    }

                    if (ok)
                    {
                        ++okCnt;
                        okStart = start;
                    }
                }

                if (okCnt == 1)
                {
                    yield return new NumberConstraint(1, new[] { Cells[(okStart + 0) % Cells.Length], Cells[(okStart + 1) % Cells.Length] }, ConstraintType.Equal, null)
                    {
                        ExtraInfo = "Special 2-non-connected-on-4-consecutive p1"
                    };
                    yield return new NumberConstraint(1, new[] { Cells[(okStart + 2) % Cells.Length], Cells[(okStart + 3) % Cells.Length] }, ConstraintType.Equal, null)
                    {
                        ExtraInfo = "Special 2-non-connected-on-4-consecutive p2"
                    };
                    yield return new NumberConstraint(1, new[] { Cells[(okStart + 0) % Cells.Length], Cells[(okStart + 3) % Cells.Length] }, ConstraintType.Minimum, null)
                    {
                        ExtraInfo = "Special 2-non-connected-on-4-consecutive p4"
                    };
                }
            }
        }


        public static ConnectionModel FromCell(Cell c)
        {
            var m = new ConnectionModel
            {
                Cells = c.Neighbors,
                RingBuffer = true
            };
            return m;
        }

        public static ConnectionModel FromColumn(ColumnConstraint columnConstraint)
        {
            var m = new ConnectionModel
            {
                Cells = columnConstraint.Cells.ToArray(),
                RingBuffer = false
            };
            return m;
        }

        public IEnumerable<SolveStep> GenerateStepsFor(NumberConstraint nc)
        {
            if (nc.Type == ConstraintType.Connected)
            {
                var states = Cells.Select(c => c?.State ?? CellState.Black).ToArray();
                var any = states.Select(s => false).ToArray();
                var all = states.Select(s => true).ToArray();

                var endIdx = RingBuffer ? Cells.Length : Cells.Length - nc.Count + 1;
                for (var start = 0; start < endIdx; ++start)
                {
                    var ok = true;
                    for (var i = start; i < start + nc.Count; ++i)
                        if (states[i % Cells.Length] == CellState.Black)
                        {
                            ok = false;
                            break;
                        }
                    if (ok)
                        for (var i = start + nc.Count; i < start + Cells.Length; ++i)
                            if (states[i % Cells.Length] == CellState.Blue)
                            {
                                ok = false;
                                break;
                            }

                    if (ok)
                    {
                        for (var i = start; i < start + nc.Count; ++i)
                            any[i % Cells.Length] = true;

                        for (var i = start + nc.Count; i < start + Cells.Length; ++i)
                            all[i % Cells.Length] = false;
                    }
                }

                for (var i = 0; i < Cells.Length; ++i)
                    if (Cells[i]?.State == CellState.Yellow && Cells[i].MaySolve)
                    {
                        if (all[i])
                            yield return new SolveStep(Cells[i], CellState.Blue, "All conceivable connected solutions of " + nc.IDString + " contain this cell");
                        if (!any[i])
                            yield return new SolveStep(Cells[i], CellState.Black, "All conceivable connected solutions of " + nc.IDString + " do NOT contain this cell");
                    }
            }
            else if (nc.Type == ConstraintType.NonConnected)
            {
                // TODO: This stuff is wrong for n>2, because YBYBY may work
                var states = Cells.Select(c => c?.State ?? CellState.Black).ToArray();
                var endIdx = RingBuffer ? Cells.Length : Cells.Length - nc.Count + 2;

                var any = states.Select(s => false).ToArray();
                var all = states.Select(s => true).ToArray();

                CheckNonConn(states, any, all, 0, 0, nc);

                /*for (var start = 0; start < endIdx; ++start)
                {
                    var ok = true;
                    // n-1 blues
                    for (var i = start; i < start + nc.Count - 1; ++i)
                        if (states[i % Cells.Length] == CellState.Black)
                        {
                            ok = false;
                            break;
                        }

                    if (RingBuffer)
                    {
                        if (states[(start - 1 + Cells.Length) % Cells.Length] == CellState.Blue)
                            ok = false;
                        if (states[(start + nc.Count - 1) % Cells.Length] == CellState.Blue)
                            ok = false;
                    }
                    else
                    {
                        if (start > 0 && states[start - 1] == CellState.Blue)
                            ok = false;
                        if (start + nc.Count - 1 < Cells.Length && states[start + nc.Count - 1] == CellState.Blue)
                            ok = false;
                    }

                    if (ok)
                    {
                        var bc = 0;
                        for (var i = start + nc.Count - 1; i != start; i = (i + 1) % Cells.Length)
                            if (states[i % Cells.Length] == CellState.Blue)
                                ++bc;

                        if (bc >= 2)
                            ok = false;

                        // fixed blue
                        if (ok && bc == 1)
                        {
                            for (var i = start; i < start + nc.Count - 1; ++i)
                                any[i % Cells.Length] = true;

                            for (var i = start + nc.Count - 1; i < start + Cells.Length; ++i)
                                all[i % Cells.Length] = false;
                        }

                        // non-fixed blue
                        if (ok && bc == 0)
                        {
                            var endJIdx = RingBuffer ? start + Cells.Length - 1 : Cells.Length;
                            for (var j = start + nc.Count; j < endJIdx; ++j)
                                if (states[j % Cells.Length] == CellState.Yellow)
                                {
                                    for (var i = start; i < start + nc.Count - 1; ++i)
                                        any[i % Cells.Length] = true;

                                    for (var i = start + nc.Count - 1; i < start + Cells.Length; ++i)
                                        if (i % Cells.Length == j % Cells.Length)
                                            any[i % Cells.Length] = true;
                                        else
                                            all[i % Cells.Length] = false;
                                }

                            if (!RingBuffer)
                            {
                                for (var j = 0; j < start - 1; ++j)
                                    if (states[j % Cells.Length] == CellState.Yellow)
                                    {
                                        for (var i = start; i < start + nc.Count - 1; ++i)
                                            any[i % Cells.Length] = true;

                                        for (var i = start + nc.Count - 1; i < start + Cells.Length; ++i)
                                            if (i % Cells.Length == j % Cells.Length)
                                                any[i % Cells.Length] = true;
                                            else
                                                all[i % Cells.Length] = false;
                                    }
                            }
                        }
                    }
                }*/

                //Debugger.Break();
                for (var i = 0; i < Cells.Length; ++i)
                    if (Cells[i]?.State == CellState.Yellow && Cells[i].MaySolve)
                    {
                        if (all[i])
                            yield return new SolveStep(Cells[i], CellState.Blue, "All conceivable non-connected solutions of " + nc.IDString + " contain this cell");
                        if (!any[i])
                            yield return new SolveStep(Cells[i], CellState.Black, "All conceivable non-connected solutions of " + nc.IDString + " do NOT contain this cell");
                    }

                /*
                for (var i = 0; i < Cells.Length; ++i)
                    if (Cells[i]?.State == CellState.Yellow && Cells[i].MaySolve)
                    {
                        if (all[i])
                            yield return new SolveStep(Cells[i], CellState.Blue, "All conceivable solutions of " + nc.IDString + " contain this cell");
                        if (!any[i])
                            yield return new SolveStep(Cells[i], CellState.Black, "All conceivable solutions of " + nc.IDString + " do NOT contain this cell");
                    }

                // for n consecutive with all blue, one yellow, yellow must be black
                for (var start = 0; start < endIdx; ++start)
                {
                    var bc = 0;
                    var yp = -1;
                    for (var i = start; i < start + nc.Count; ++i)
                    {
                        if (states[i % Cells.Length] == CellState.Blue) bc += 1;
                        if (states[i % Cells.Length] == CellState.Yellow) yp = i;
                    }

                    if (bc == nc.Count - 1 && yp >= 0)
                    {
                        if (Cells[yp].MaySolve)
                            yield return new SolveStep(Cells[yp], CellState.Black, "Otherwise, the resulting connected sequence would violate " + nc.IDString);
                    }
                }

                // for < n consecutive yellow with black borders, all must be black
                for (var start = 0; start < Cells.Length; ++start)
                {
                    if (states[start] != CellState.Black)
                        continue;
                    if (states[(start + 1) % Cells.Length] != CellState.Yellow)
                        continue;

                    var i = start + 1;
                    var cnt = 1;
                    while (cnt < nc.Count && states[(start + i) % Cells.Length] != CellState.Black)
                    {
                        ++i;
                        ++cnt;
                    }
                }*/
            }
            else throw new NotImplementedException();
        }

        private void CheckNonConn(CellState[] states, bool[] any, bool[] all, int bc, int i, NumberConstraint nc)
        {
            if (i == states.Length)
            {
                // Check solution
                bool ok = states.Count(s => s == CellState.Blue) == nc.Count;
                if (ok)
                {
                    int fi = states.FirstIndexOf(CellState.Blue);
                    int li = states.LastIndexOf(CellState.Blue);

                    if (RingBuffer)
                    {
                        int cnt = -1;
                        int j = fi;
                        while (states[j] == CellState.Blue)
                        {
                            j = (j - 1 + states.Length) % states.Length;
                            ++cnt;
                        }
                        j = fi;
                        while (states[j] == CellState.Blue)
                        {
                            j = (j + 1) % states.Length;
                            ++cnt;
                        }

                        ok = cnt < nc.Count;
                    }
                    else
                    {
                        ok = li - fi >= nc.Count;
                    }

                    if (ok)
                    {
                        for (var j = 0; j < states.Length; ++j)
                            if (states[j] == CellState.Blue)
                                any[j] = true;
                            else all[j] = false;
                    }
                }
            }
            else
            {
                if (states[i] == CellState.Yellow)
                {
                    if (bc < nc.Count)
                    {
                        states[i] = CellState.Blue;
                        CheckNonConn(states, any, all, bc + 1, i + 1, nc);
                    }

                    states[i] = CellState.Black;
                    CheckNonConn(states, any, all, bc + 0, i + 1, nc);

                    states[i] = CellState.Yellow;
                }
                else CheckNonConn(states, any, all, bc + (states[i] == CellState.Blue ? 1 : 0), i + 1, nc);
            }
        }
    }
}
