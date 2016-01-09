using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexCellsBot.Logic
{
    public class SolveStep
    {
        public Cell Cell;
        public CellState NewState;

        public string Explanation;

        public SolveStep(Cell cell, CellState newState, string explanation)
        {
            Cell = cell;
            if (cell.State != CellState.Yellow)
                throw new InvalidOperationException("Only allowed for yellow");

            cell.Solved = true;
            cell.SolvedState = newState;

            NewState = newState;
            if (newState == CellState.Yellow)
                throw new InvalidOperationException("Cannot switch to yellow");
            Explanation = explanation;
        }

        public override string ToString() => $"{Cell} -> {NewState}, because '{Explanation}'";
    }
}
