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

    public class Cell
    {
        public int Nr;
        public CellState State;
        public ivec2 Center;
        public int Radius;
        // can contain nulls
        public readonly Cell[] Neighbors = new Cell[6];
        public List<Cell> AllNeighbors = new List<Cell>();

        public bool Solved = false;

        public string InnerText;

        public NumberConstraint Constraint
        {
            get
            {
                if (!string.IsNullOrEmpty(InnerText) && InnerText != "?")
                {
                    if (InnerText == "O")
                        InnerText = "0";

                    int cnt;
                    if (!int.TryParse(InnerText, out cnt))
                    {
                        throw new InvalidOperationException("Invalid Number '" + InnerText + "'");
                    }

                    switch (State)
                    {
                        case CellState.Black:
                            return new NumberConstraint(cnt, Neighbors);
                        default:
                            throw new NotImplementedException();
                    }
                }

                return null;
            }
        }

        public bool MaySolve => State == CellState.Yellow && !Solved;

        public override string ToString()
            => $"Cell #{Nr} ({Center}, {State}{(string.IsNullOrEmpty(InnerText) ? "" : ", " + InnerText)})";
    }
}
