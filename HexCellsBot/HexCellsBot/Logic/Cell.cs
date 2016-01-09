using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace HexCellsBot.Logic
{
    public enum CellState
    {
        Yellow,
        Blue,
        Black
    }

    public enum CellNeighbor
    {
        Top,
        TopRight,
        BottomRight,
        Bottom,
        BottomLeft,
        TopLeft
    }

    public class CellStub
    {
        public CellState State;
        public string Text;

        public CellStub(CellState state, string txt)
        {
            State = state;
            Text = txt;
        }
    }

    public class Cell
    {
        public int Nr;
        public CellState State;
        public ivec2 Center;
        public int Radius;
        // can contain nulls
        public readonly Cell[] Neighbors = new Cell[6];
        public List<Cell> AllNeighbors = new List<Cell>();
        public Model Model;

        public bool Solved = false;
        public CellState SolvedState;

        public string InnerText;

        public NumberConstraint Constraint
        {
            get
            {
                // FUCK OCR
                if (InnerText == "7" && State == CellState.Black)
                    InnerText = "?";

                if (!string.IsNullOrEmpty(InnerText) && InnerText != "?" && InnerText != "’?")
                {
                    if (InnerText == "O")
                        InnerText = "0";

                    var type = ConstraintType.Equal;
                    var txt = InnerText;

                    if (InnerText.StartsWith("{") || InnerText.EndsWith("}"))
                    {
                        type = ConstraintType.Connected;
                        txt = InnerText.Trim('{', '}');
                    }
                    if (InnerText.StartsWith("-") || InnerText.StartsWith(".") || InnerText.StartsWith("_") ||
                        InnerText.EndsWith("-") || InnerText.EndsWith(".") || InnerText.EndsWith("_"))
                    {
                        type = ConstraintType.NonConnected;
                        txt = InnerText.Trim('-', '.', '_');
                    }

                    int cnt;
                    if (!int.TryParse(txt, out cnt))
                    {
                        throw new InvalidOperationException("Invalid Number '" + InnerText + "'");
                    }

                    switch (State)
                    {
                        case CellState.Black:
                            if (cnt > 6)
                                throw new ArgumentOutOfRangeException("cnt");
                            return new NumberConstraint(cnt, Neighbors, type, ConnectionModel.FromCell(this)) { ExtraInfo = "From " + this };
                        default:
                            throw new NotImplementedException();
                    }
                }

                return null;
            }
        }

        public bool MaySolve(CellState newState)
        {
            if (Solved && newState != SolvedState)
                throw new InvalidOperationException("Inconsistent solving");
            return State == CellState.Yellow && !Solved;
        }

        public override string ToString()
            => $"Cell #{Nr} ({Center}, {State}{(string.IsNullOrEmpty(InnerText) ? "" : ", " + InnerText)})";
    }
}
