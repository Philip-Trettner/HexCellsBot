using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexCellsBot.Logic;
using NUnit.Framework;

namespace HexCellsBot.Tests
{
    [TestFixture]
    public class BasicNonConnectedTests : TestBase
    {
        [Test]
        public void BugNr1()
        {
            var m = Model.FromGrid(new[,]
            {
                {null, Blue(), Black()},
                {Yellow(), Black("-2-"), Yellow()},
                {Yellow(), Yellow(), null},
            });

            m.InternallyApplySteps();
            Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Black);
            Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
            Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Black);
            Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Yellow);
            Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Yellow);
            Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Yellow);
        }

        [Test]
        public void BasicNonConnectedCyclic()
        {
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Yellow()},
                    {Yellow(), Black("-4-"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 0);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Yellow()},
                    {Yellow(), Black("-2-"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 0);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Blue(), Yellow()},
                    {Yellow(), Black("-2-"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 2);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Yellow);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Blue(), Black()},
                    {Yellow(), Black("-2-"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 1);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Yellow);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Blue(), Black()},
                    {Yellow(), Black("-2-"), Yellow()},
                    {Black(), Black(), null},
                });

                Assert.AreEqual(m.Steps.Count, 2);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Blue);
            }
        }
    }
}
