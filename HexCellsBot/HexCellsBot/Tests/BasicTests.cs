using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexCellsBot.Logic;
using NUnit.Framework;

namespace HexCellsBot.Tests
{
    [TestFixture]
    public class BasicTests : TestBase
    {
        [Test]
        public void GridTest()
        {
            var m = Model.FromGrid(new[,]
            {
                { null, Yellow(), Yellow() },
                   { Yellow(), Black(), Yellow() },
                      { Yellow(), Yellow(), null },
            });

            Assert.AreEqual(m.Cells.Count, 7);

            var b = m.Cells.First(c => c.State == CellState.Black);
            Assert.AreEqual(b.AllNeighbors.Count, 6);
            foreach (var c in b.AllNeighbors)
                Assert.AreEqual(c.AllNeighbors.Count, 3);
        }

        [Test]
        public void BasicFulls()
        {
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Yellow()},
                    {Yellow(), Black("6"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 6);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Blue);
            }

            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Yellow()},
                    {Yellow(), Black("5"), Yellow()},
                    {Yellow(), null, null},
                });

                Assert.AreEqual(m.Steps.Count, 5);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Blue);
            }

            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Yellow(), Yellow()},
                }, Column(0, 0, "4", 0));

                Assert.AreEqual(m.Steps.Count, 4);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Blue);
            }

            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Yellow(), Yellow()},
                });
                m.RemainingCount = 4;
                m.Resolve();

                Assert.AreEqual(m.Steps.Count, 4);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Blue);
            }
        }

        [Test]
        public void BasicEmpties()
        {
            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Yellow()},
                    {Yellow(), Black("0"), Yellow()},
                    {Yellow(), Yellow(), null},
                });

                Assert.AreEqual(m.Steps.Count, 6);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Black);
            }

            {
                var m = Model.FromGrid(new[,]
                {
                    {null, Yellow(), Yellow()},
                    {Yellow(), Black("0"), Yellow()},
                    {Yellow(), null, null},
                });

                Assert.AreEqual(m.Steps.Count, 5);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Black);
            }

            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Yellow(), Yellow()},
                }, Column(0, 0, "0", 0));

                Assert.AreEqual(m.Steps.Count, 4);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Black);
            }

            {
                var m = Model.FromGrid(new[,]
                {
                    {Yellow(), Yellow(), Yellow(), Yellow()},
                });
                m.RemainingCount = 0;
                m.Resolve();

                Assert.AreEqual(m.Steps.Count, 4);
                foreach (var s in m.Steps)
                    Assert.AreEqual(s.NewState, CellState.Black);
            }
        }

    }
}
