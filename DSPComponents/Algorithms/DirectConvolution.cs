using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> signal = new List<float>();
            List<int> Index = new List<int>();

            int start_X = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            int end_H = InputSignal1.SamplesIndices[InputSignal1.Samples.Count() - 1] + InputSignal2.SamplesIndices[InputSignal2.Samples.Count() - 1];


            for (int i = start_X; i <= end_H; i++)
            {
                float temp = 0;
                for (int j = InputSignal1.SamplesIndices[0]; j < InputSignal1.Samples.Count(); j++)
                {
                    float temp2;
                    if (j > InputSignal1.SamplesIndices[InputSignal1.Samples.Count() - 1] || (i - j) < InputSignal2.SamplesIndices[0])
                        break;
                    if ((i - j) > InputSignal2.SamplesIndices[InputSignal2.Samples.Count() - 1])
                        continue;

                    temp2 = InputSignal1.Samples[InputSignal1.SamplesIndices.IndexOf(j)] * InputSignal2.Samples[InputSignal2.SamplesIndices.IndexOf(i - j)];
                    temp += temp2;
                }
                if (i == end_H && temp == 0)
                    continue;

                signal.Add(temp);
                Index.Add(i);
            }

            OutputConvolvedSignal = new Signal(signal, Index, false);
        }
    }
}