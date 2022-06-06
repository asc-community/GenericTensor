using GenericTensor.Core;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonkPerf.NET.Core;

namespace UnitTests
{
    [TestClass]
    public class ForEach
    {
        [TestMethod]
        public void Test1D()
        {
            var tensor = GenTensor<int, IntWrapper>.CreateTensor(new [] { 1, 2, 3 });
            tensor.ForEach(new CapturingValueAction<int[], int, GenTensor<int, IntWrapper>>((index, value, t) => t[index] = value * value, tensor));
            var expected = GenTensor<int, IntWrapper>.CreateTensor(new[] { 1, 4, 9 });
            Assert.AreEqual(expected, tensor);
        }

        [TestMethod]
        public void Test2D()
        {
            var tensor = GenTensor<int, IntWrapper>.CreateTensor(new[,] { { 1, 2 }, { 3, 4 } });
            tensor.ForEach(new CapturingValueAction<int[], int, GenTensor<int, IntWrapper>>((index, value, t) => t[index] = value * value, tensor));
            var expected = GenTensor<int, IntWrapper>.CreateTensor(new[,] { { 1, 4 }, { 9, 16 } });
            Assert.AreEqual(expected, tensor);
        }

        [TestMethod]
        public void Test3D()
        {
            var tensor = GenTensor<int, IntWrapper>.CreateTensor(new[,,] { { { 1, 2, 3 }, { 4, 5, 6 } }, { { 7, 8, 9 }, { 10, 11, 12 } } });
            tensor.ForEach(new CapturingValueAction<int[], int, GenTensor<int, IntWrapper>>((index, value, t) => t[index] = value * value, tensor));
            var expected = GenTensor<int, IntWrapper>.CreateTensor(new[,,] { { { 1, 4, 9 }, { 16, 25, 36 } }, { { 49, 64, 81 }, { 100, 121, 144 } } });
            Assert.AreEqual(expected, tensor);
        }

        [TestMethod]
        public void Test4D()
        {
            var tensor = GenTensor<int, IntWrapper>.CreateTensor(new[,,,] {
                { { { 1, 2, 3 }, { 4, 5, 6 } }, { { 7, 8, 9 }, { 10, 11, 12 } } },
                { { { 13, 14, 15 }, { 16, 17, 18 } }, { { 19, 20, 21 }, { 22, 23, 24 } } },
                });
            tensor.ForEach(new CapturingValueAction<int[], int, GenTensor<int, IntWrapper>>((index, value, t) => t[index] = value * value, tensor));
            var expected = GenTensor<int, IntWrapper>.CreateTensor(new[, , ,] {
                { { { 1, 4, 9 }, { 16, 25, 36 } }, { { 49, 64, 81 }, { 100, 121, 144 } } },
                { { { 169, 196, 225 }, { 256, 289, 324 } }, { { 361, 400, 441 }, { 484, 529, 576 } } },
                });
            Assert.AreEqual(expected, tensor);
        }

        [TestMethod]
        public void Test5D()
        {
            var tensor = GenTensor<int, IntWrapper>.CreateTensor(new[,,,,] {
                {
                    { { { 1, 2, 3 }, { 4, 5, 6 } }, { { 7, 8, 9 }, { 10, 11, 12 } } },
                    { { { 13, 14, 15 }, { 16, 17, 18 } }, { { 19, 20, 21 }, { 22, 23, 24 } } },
                },
                {
                    { { { 10, 20, 30 }, { 40, 50, 60 } }, { { 70, 80, 90 }, { 100, 110, 120 } } },
                    { { { 130, 140, 150 }, { 160, 170, 180 } }, { { 190, 200, 210 }, { 220, 230, 240 } } },
                }
                });
            tensor.ForEach(new CapturingValueAction<int[], int, GenTensor<int, IntWrapper>>((index, value, t) => t[index] = value * value, tensor));
            var expected = GenTensor<int, IntWrapper>.CreateTensor(new[,,,,] {
                {
                    { { { 1, 4, 9 }, { 16, 25, 36 } }, { { 49, 64, 81 }, { 100, 121, 144 } } },
                    { { { 169, 196, 225 }, { 256, 289, 324 } }, { { 361, 400, 441 }, { 484, 529, 576 } } },
                },
                {
                    { { { 100, 400, 900 }, { 1600, 2500, 3600 } }, { { 4900, 6400, 8100 }, { 10000, 12100, 14400 } } },
                    { { { 16900, 19600, 22500 }, { 25600, 28900, 32400 } }, { { 36100, 40000, 44100 }, { 48400, 52900, 57600 } } },
                },
                });
            Assert.AreEqual(expected, tensor);
        }
    }
}