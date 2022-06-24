using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public List<float> output = new List<float>();
        public override void Run()
        {
            float mean = 0;

            for (int i = 0; i < InputSignal.Samples.Count; i++)
                mean = mean + InputSignal.Samples[i];

            mean /= InputSignal.Samples.Count;

            for (int i = 0; i < InputSignal.Samples.Count; i++)
                output.Add(InputSignal.Samples[i] - mean);

            OutputSignal = new Signal(output, false);
        }
    }
}
