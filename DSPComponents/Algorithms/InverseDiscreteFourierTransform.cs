using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<Tuple<float, float>> Final_number = new List<Tuple<float, float>>();
            List<float> samples = new List<float>();

            for (int i = 0; i < InputFreqDomainSignal.Frequencies.Count; i++)
            {
                float amp = InputFreqDomainSignal.FrequenciesAmplitudes[i];
                float p_h = (float)Math.Tan(InputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                float Real_number = (float)Math.Sqrt((Math.Pow(amp, 2)) / (1 + Math.Pow(p_h, 2)));
                float Amgend_number = Real_number * p_h;
                Final_number.Add(new Tuple<float, float>(Real_number, Amgend_number));
            }

            for (int i = 0; i < InputFreqDomainSignal.Frequencies.Count; i++)
            {
                float sum = 0;
                for (int j = 0; j < InputFreqDomainSignal.Frequencies.Count; j++)
                {
                    float real_number = Final_number[j].Item1 * (float)(Math.Cos((2 * (Math.PI) * i * j) / InputFreqDomainSignal.FrequenciesAmplitudes.Count));
                    float amgend_number = Final_number[j].Item2 * -(float)(Math.Sin((2 * (Math.PI) * i * j) / InputFreqDomainSignal.FrequenciesAmplitudes.Count));
                    sum = sum + (real_number + amgend_number);
                }
                samples.Add(sum / InputFreqDomainSignal.Frequencies.Count);
            }

            samples.Reverse();
            OutputTimeDomainSignal = new Signal(samples, false);
        }
    }
}
