using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> signal = new List<float>();

            for (int i = 0; i < InputSignals[0].Samples.Count; i++)
            {
                float var = 0;
                for (int ii = 0; ii < InputSignals.Count; ii++)
                {
                    var += InputSignals[ii].Samples[i];
                }
                signal.Add(var);
            }

            OutputSignal = new Signal(signal, false);
        }
    }
}