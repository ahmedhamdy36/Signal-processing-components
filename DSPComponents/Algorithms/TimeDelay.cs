using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            DirectCorrelation directCorrelation = new DirectCorrelation();
            List<float> CorrelationValues = new List<float>();

            for (int i = 0; i < InputSignal1.Samples.Count(); i++)
                CorrelationValues.Add(directCorrelation.OutputNormalizedCorrelation[i]);

            float maxValue = Math.Abs(CorrelationValues.Max());
            float minValue = Math.Abs(CorrelationValues.Min());
            
            if (maxValue > minValue)
                OutputTimeDelay = CorrelationValues.IndexOf(maxValue) * InputSamplingPeriod;
            else
                OutputTimeDelay = CorrelationValues.IndexOf(minValue) * InputSamplingPeriod;

        }
    }
}
