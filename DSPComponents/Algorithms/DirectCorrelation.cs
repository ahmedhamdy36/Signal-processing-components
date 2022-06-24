using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            List<float> outputNonNormalizedCorrelation = new List<float>();
            List<float> outputNormalizedCorrelation = new List<float>();
            List<float> shiftedsignal = new List<float>();
            List<float> NonNormalized = new List<float>();
            List<float> Normalized = new List<float>();

            float sumofsignal1 = 0;
            float sumofsignal2 = 0;
            
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
                sumofsignal1 = sumofsignal1 + (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
      
            float normalizationresult;
            if (InputSignal2 == null)
            {
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    shiftedsignal.Add(InputSignal1.Samples[i]);

                normalizationresult = (float)Math.Sqrt(sumofsignal1 * sumofsignal1) / InputSignal1.Samples.Count();
            }
            else
            {
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    shiftedsignal.Add(InputSignal2.Samples[i]);

                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    sumofsignal2 = sumofsignal2 + (InputSignal2.Samples[i] * InputSignal2.Samples[i]);

                normalizationresult = (float)Math.Sqrt(sumofsignal1 * sumofsignal2) / InputSignal1.Samples.Count;
            }

            int k = 0;
            while (k <= InputSignal1.Samples.Count - 1)
            {
                float result = 0;

                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    result = result + (InputSignal1.Samples[i] * shiftedsignal[i]);
                result = result / InputSignal1.Samples.Count;
                
                NonNormalized.Add(result);
                Normalized.Add(result / normalizationresult);

                if (InputSignal1.Periodic == true)
                {
                    float temp = 0;
                    for (int j = 0; j < shiftedsignal.Count; j++)
                    {
                        if (j == 0)
                            temp = shiftedsignal[j];
                        else
                            shiftedsignal[j - 1] = shiftedsignal[j];
                    }
                    if (InputSignal2 != null)
                        shiftedsignal[InputSignal2.Samples.Count - 1] = temp;
                    else
                        shiftedsignal[InputSignal1.Samples.Count - 1] = temp;
                }
                if (InputSignal1.Periodic == false)
                {
                    for (int j = 1; j < shiftedsignal.Count; j++)
                        shiftedsignal[j - 1] = shiftedsignal[j];

                    shiftedsignal[InputSignal1.Samples.Count - 1] = 0;
                }
                k++;
            }
            OutputNonNormalizedCorrelation = NonNormalized;
            OutputNormalizedCorrelation = Normalized;
        }
    }
}