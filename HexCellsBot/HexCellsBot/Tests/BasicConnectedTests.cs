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
    public class BasicConnectedTests : TestBase
    {
        [Test]
        public void BasicConnectedCyclic()
        {
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Yellow()},
                    {Yellow(), Black("{6}"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 6);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Blue);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Black()},
                    {Yellow(), Black("{5}"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 5);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Blue);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Black()},
                    {Yellow(), Black("{4}"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 3);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Blue);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Yellow);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Black()},
                    {Yellow(), Black("{3}"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 1);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Blue);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Yellow);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Blue(), Black()},
                    {Yellow(), Black("{3}"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 4);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Black);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Black()},
                    {Yellow(), Black("{3}"), Blue()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 4);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Blue);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Black()},
                    {Yellow(), Black("{3}"), Yellow()},
                    {Yellow(), Blue(), null},
                });

                Assert.AreEqual(m.Steps.Count, 2);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(0, 1).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(0, 2).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(1, 2).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(2, 1).State, CellState.Yellow);
            }
        }

        [Test]
        public void BasicConnectedNonCyclic()
        {
            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Yellow(), Yellow()},
                }, Column(0, 0, "{3}", 0));

                Assert.AreEqual(m.Steps.Count, 2);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(0, 0).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(3, 0).State, CellState.Yellow);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Yellow(), Yellow()},
                }, Column(0, 0, "{2}", 0));

                Assert.AreEqual(m.Steps.Count, 0);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(0, 0).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(3, 0).State, CellState.Yellow);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {Blue(), Yellow(), Yellow(), Yellow()},
                }, Column(0, 0, "{3}", 0));

                Assert.AreEqual(m.Steps.Count, 3);

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(0, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(3, 0).State, CellState.Black);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Blue(), Yellow(), Yellow()},
                }, Column(0, 0, "{3}", 0));

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(0, 0).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(3, 0).State, CellState.Yellow);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Yellow(), Blue()},
                }, Column(0, 0, "{3}", 0));

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(0, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(3, 0).State, CellState.Blue);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Yellow(), Blue()},
                }, Column(0, 0, "{2}", 0));

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(0, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(3, 0).State, CellState.Blue);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Blue(), Blue()},
                }, Column(0, 0, "{2}", 0));

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(0, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(3, 0).State, CellState.Blue);
            }
            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Blue(), Yellow()},
                }, Column(0, 0, "{2}", 0));

                m.InternallyApplySteps();
                Assert.AreEqual(m.GridCellAt(0, 0).State, CellState.Black);
                Assert.AreEqual(m.GridCellAt(1, 0).State, CellState.Yellow);
                Assert.AreEqual(m.GridCellAt(2, 0).State, CellState.Blue);
                Assert.AreEqual(m.GridCellAt(3, 0).State, CellState.Yellow);
            }
        }
    }
}
