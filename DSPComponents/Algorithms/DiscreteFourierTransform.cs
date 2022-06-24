using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<float> number = new List<float>();
            List<float> pa_shift = new List<float>();
            List<float> amplitude = new List<float>();
            List<Tuple<double, double>> index = new List<Tuple<double, double>>();
            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
            {
                number.Add(i * (1 / InputSamplingFrequency));
                double R_freq = 0;
                double Am_freq = 0;

                for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++)
                {
                    R_freq = R_freq + (((double)(InputTimeDomainSignal.Samples[j])) * (Math.Cos((2 * (Math.PI) * i * j) / InputTimeDomainSignal.Samples.Count)));
                    Am_freq = Am_freq - ((double)(InputTimeDomainSignal.Samples[j]) * (Math.Sin((2 * (Math.PI) * i * j) / InputTimeDomainSignal.Samples.Count)));
                }

                index.Add(new Tuple<double, double>(R_freq, Am_freq));
                double p_sh = Math.Atan2(index[i].Item2, index[i].Item1);
                float p_sh2 = (float)p_sh;
                float amplitude2 = (float)Math.Sqrt(Math.Pow(index[i].Item1, 2) + Math.Pow(index[i].Item2, 2));
                pa_shift.Add(p_sh2);
                amplitude.Add(amplitude2);
            }

            OutputFreqDomainSignal = new Signal(false, number, amplitude, pa_shift);
        }
    }
}
