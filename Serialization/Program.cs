using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Serialization
{
    public class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string type;
            string inpStr;
            string outpStr;
            Input inp;
            Output outp;

            var jsonSerializer = new JsonSerializer();
            var xmlSerializer = new XmlSerializer();

            // read input
            type = Console.ReadLine();
            inpStr = Console.ReadLine();

            // deserialize input
            if (type == "Json") inp = jsonSerializer.Deserialize<Input>(inpStr);
            else inp = xmlSerializer.Deserialize<Input>(inpStr);

            // calculate output
            outp = CalculateOutput(inp);

            // serialize output
            if (type == "Json") outpStr = jsonSerializer.Serialize(outp);
            else outpStr = xmlSerializer.Serialize(outp);

            // write output
            Console.WriteLine(outpStr);
            //Console.ReadKey();
        }
        public static Output CalculateOutput(Input input)
        {
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
            return output;
        }
    }
}
