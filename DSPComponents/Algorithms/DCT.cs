using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            int N = InputSignal.Samples.Count;
            double c;
            double sum;
            List<float> Samples_values = new List<float>();
            for (int i = 0; i < N; i++)
            {
                if (i == 0)
                    c = 1 / Math.Sqrt(N);
                else
                    c = Math.Sqrt(2) / Math.Sqrt(N);
                sum = 0;
                for (int j = 0; j < N; j++)
                    sum += InputSignal.Samples[j] * Math.Cos((2 * j + 1) * i * Math.PI / (2 * N));
                Samples_values.Add((float)(c * sum));
            }
            OutputSignal = new Signal(Samples_values, false);
        }
    }
}
