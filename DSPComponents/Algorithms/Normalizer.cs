using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> signal = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float var1 = InputSignal.Samples[i] - InputSignal.Samples.Min();
                float var2 = InputSignal.Samples.Max() - InputSignal.Samples.Min();
                float val;

                if (InputMinRange == 0)
                    val = var1 / var2;
                else
                    val = 2 * (var1 / var2) - 1;
                
                signal.Add(val);
            }

            OutputNormalizedSignal = new Signal(signal, false);
        }
    }
}
