using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> signal = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                if (i == 0)
                    signal.Add(InputSignal.Samples[i]);
                else
                    signal.Add(InputSignal.Samples[i] + signal[signal.Count() - 1]);
            }

            OutputSignal = new Signal(signal, false);
        }
    }
}
