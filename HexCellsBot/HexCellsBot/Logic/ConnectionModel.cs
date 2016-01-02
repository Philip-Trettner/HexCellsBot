using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexCellsBot.Logic
{
    public class ConnectionModel
    {
        public bool RingBuffer = true;
        public Cell[] Cells;

        public static ConnectionModel FromCell(Cell c)
        {
            var m = new ConnectionModel
            {
                Cells = c.Neighbors,
                RingBuffer = true
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

                var endIdx = RingBuffer ? Cells.Length : Cells.Length - nc.Count;
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
                var states = Cells.Select(c => c?.State ?? CellState.Black).ToArray();
                var endIdx = RingBuffer ? Cells.Length : Cells.Length - nc.Count;

                var any = states.Select(s => false).ToArray();
                var all = states.Select(s => true).ToArray();

                for (var start = 0; start < endIdx; ++start)
                {
                    var ok = true;
                    // n-1 blues
                    for (var i = start; i < start + nc.Count - 1; ++i)
                        if (states[i % Cells.Length] == CellState.Black)
                        {
                            ok = false;
                            break;
                        }
                    if (states[(start - 1 + Cells.Length) % Cells.Length] == CellState.Blue)
                        ok = false;
                    if (states[(start + nc.Count - 1) % Cells.Length] == CellState.Blue)
                        ok = false;

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
                            for (var i = start; i < start + nc.Count - 1; ++i)
                                any[i % Cells.Length] = true;

                            for (var j = start + nc.Count; j < start + Cells.Length - 1; ++j)
                                if (states[j % Cells.Length] == CellState.Yellow)
                                    for (var i = start + nc.Count - 1; i < start + Cells.Length; ++i)
                                        if (i == j)
                                            any[i % Cells.Length] = true;
                                        else
                                            all[i % Cells.Length] = false;
                        }
                    }
                }

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
    }
}
