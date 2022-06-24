using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<float> signal = new List<float>();
            List<int> Index = new List<int>();

            for (int i = InputSignal.Samples.Count - 1; i >= 0; i--)
            {
                signal.Add(InputSignal.Samples[i]);
                Index.Add(InputSignal.SamplesIndices[i] * -1);
            }

            if (InputSignal.Periodic)
                InputSignal.Periodic = false;
            else
                InputSignal.Periodic = true;

            OutputFoldedSignal = new Signal(signal, Index, InputSignal.Periodic);
        }
    }
}

