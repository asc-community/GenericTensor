using System;
using System.Linq;
using GenericTensor.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Transposition
    {
        private TensorInt GetT()
        {
            var res = new TensorInt(2, 3, 4);
            foreach (var (index, _) in res.Iterate())
            {
                res[index] = index[0] + index[1] + index[2];
            }
            return res;
        }


        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
