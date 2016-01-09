using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexCellsBot.Logic;

namespace HexCellsBot.Tests
{
    public class TestBase
    {
        public static CellStub C(CellState s, string txt = "") => new CellStub(s, txt);
        public static CellStub Blue(string txt = "") => new CellStub(CellState.Blue, txt);
        public static CellStub Black(string txt = "") => new CellStub(CellState.Black, txt);
        public static CellStub Yellow(string txt = "") => new CellStub(CellState.Yellow, txt);
        public static ColumnConstraintTestStub Column(int x, int y, string txt, int angle) => new ColumnConstraintTestStub(x, y, txt, angle);
    }
}
