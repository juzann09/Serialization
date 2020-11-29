using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Serialization;
using System;
using System.Collections.Generic;

namespace Serialization
{
    [TestClass]
    public class SerialzerTest
    {
        [TestMethod]
        public void InputJsonTest()
        {
            var serializer = new JsonSerializer();

            var input = CreateInput();
            //var inputStr = JsonConvert.SerializeObject(input);
            var inputStr = serializer.Serialize<Input>(input);
            var actualInput = serializer.Deserialize<Input>(inputStr);

            Assert.AreEqual(input.K, actualInput.K);
            Assert.AreEqual(input.Sums.Length, actualInput.Sums.Length);
            Assert.AreEqual(input.Muls.Length, actualInput.Muls.Length);
            for (int i = 0; i < input.Sums.Length; i++)
            {
                Assert.AreEqual(input.Sums[i], actualInput.Sums[i]);
            }
            for (int i = 0; i < input.Muls.Length; i++)
            {
                Assert.AreEqual(input.Muls[i], actualInput.Muls[i]);
            }
        }

        [TestMethod]
        public void OutputJsonTest()
        {
            var serializer = new JsonSerializer();

            var output = CreateOutput();
            var outputStr = serializer.Serialize(output);
            var actualOutput = serializer.Deserialize<Output>(outputStr);

            Assert.AreEqual(output.SumResult, actualOutput.SumResult);
            Assert.AreEqual(output.MulResult, actualOutput.MulResult);
            Assert.AreEqual(output.SortedInputs.Length, actualOutput.SortedInputs.Length);
            for (int i = 0; i < output.SortedInputs.Length; i++)
            {
                Assert.AreEqual(output.SortedInputs[i], actualOutput.SortedInputs[i]);
            }
        }

        [TestMethod]
        public void RandomOutputs()
        {
            var output1 = CreateOutput();
            var output2 = CreateOutput();
            Assert.AreEqual(output1.SumResult, output2.SumResult);
            Assert.AreEqual(output1.MulResult, output2.MulResult);
            Assert.AreEqual(output1.SortedInputs.Length, output2.SortedInputs.Length);
            for (int i = 0; i < output1.SortedInputs.Length; i++)
            {
                Assert.AreEqual(output1.SortedInputs[i], output2.SortedInputs[i]);
            }
        }

        [TestMethod]
        public void EqualOutputs()
        {
            var output1 = CreateOutput();
            var output2 = new Output
            {
                SumResult = output1.SumResult,
                MulResult = output1.MulResult,
                SortedInputs = output1.SortedInputs
            };
            Assert.AreEqual(output1.SumResult, output2.SumResult);
            Assert.AreEqual(output1.MulResult, output2.MulResult);
            Assert.AreEqual(output1.SortedInputs.Length, output2.SortedInputs.Length);
            for (int i = 0; i < output1.SortedInputs.Length; i++)
            {
                Assert.AreEqual(output1.SortedInputs[i], output2.SortedInputs[i]);
            }
        }

        [TestMethod]
        public void SimpleJsonTest()
        {
            var serializer = new JsonSerializer();

            var input = CreateInput();
            
            // calculate right output
            var output = new Output();
            output.SumResult = 0;
            output.MulResult = 1;
            foreach (var item in input.Sums) output.SumResult += item;
            output.SumResult *= input.K;
            foreach (var item in input.Muls) output.MulResult *= item;
            output.SortedInputs = new decimal[] { };
            var list = new List<decimal>();
            foreach (var item in input.Muls) list.Add(Convert.ToDecimal(item));
            list.AddRange(input.Sums);
            list.Sort();
            output.SortedInputs = list.ToArray();
            var outpStr = serializer.Serialize(output);

            // calculate actual output
            var inputStr = serializer.Serialize(input);
            var actualInput = serializer.Deserialize<Input>(inputStr);
            var actualOutput = Program.CalculateOutput(actualInput);

            // compare
            Assert.AreEqual(output.SumResult, actualOutput.SumResult);
            Assert.AreEqual(output.MulResult, actualOutput.MulResult);
            Assert.AreEqual(output.SortedInputs.Length, actualOutput.SortedInputs.Length);
            for (int i = 0; i < output.SortedInputs.Length; i++)
            {
                Assert.AreEqual(output.SortedInputs[i], actualOutput.SortedInputs[i]);
            }
        }

        //[TestMethod]
        //public void SimpleXmlTest()
        //{
        //    var serializer = new XmlSerializer();

        //    var catalog = CreateCatalog();

        //    var catalogStr = serializer.Serialize(catalog);
        //    var actualCatalog = serializer.Deserialize<Catalog>(catalogStr);

        //    Assert.AreEqual(catalog.RecylcePoints.Length, actualCatalog.RecylcePoints.Length);
        //    var firstGood = catalog.RecylcePoints[0];
        //    var actualFirstGood = actualCatalog.RecylcePoints[0];

        //    Assert.AreEqual(firstGood.Address, actualFirstGood.Address);
        //    //Assert.Equal(firstGood.Count, actualFirstGood.Count);
        //    // Assert.Equal(firstGood.Price, actualFirstGood.Price);
        //    // Assert.Equal(firstGood.Sex, actualFirstGood.Sex);
        //}

        private static Input CreateInput()
        {
            var rnd = new Random();
            var k = rnd.Next(1000);
            var sums = new List<Decimal>();
            var muls = new List<int>();
            return new Input
            {
                K = k,
                Sums = sums.ToArray(),
                Muls = muls.ToArray()
            };
        }

        private static Output CreateOutput()
        {
            var rnd = new Random();
            var sumResult = Convert.ToDecimal(rnd.NextDouble()*10000);
            var mulResult = rnd.Next(10000);
            var sortedInputs = new List<Decimal>();
            return new Output
            {
                SumResult = sumResult,
                MulResult = mulResult,
                SortedInputs = sortedInputs.ToArray()
            };
        }

    }
}