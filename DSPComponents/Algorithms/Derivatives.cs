using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> firstD = new List<float>();
            List<float> secoundD = new List<float>();

            firstD.Add(InputSignal.Samples[0]);
            secoundD.Add(InputSignal.Samples[1] - (2 * InputSignal.Samples[0]));

            for (int i = 1; i < InputSignal.Samples.Count - 1; i++)
            {
                firstD.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
                secoundD.Add(InputSignal.Samples[i + 1] - (2 * InputSignal.Samples[i]) + InputSignal.Samples[i - 1]);
            }

            FirstDerivative = new Signal(firstD, false);
            SecondDerivative = new Signal(secoundD, false);
        }
    }
}
