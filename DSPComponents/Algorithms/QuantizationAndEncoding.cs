using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();


            // if InputNumBits given 
            if (InputLevel == 0)
            {
                InputLevel = 1;
                for (int i = 0; i < InputNumBits; i++)
                    InputLevel *= 2;
            }


            // Calculate Dalta and Intervals
            float minimum_val = InputSignal.Samples.Min();
            float maximum_val = InputSignal.Samples.Max();
            decimal delta = ((decimal)maximum_val - (decimal)minimum_val) / InputLevel;


            // Calculate Intervals
            decimal pointer = (decimal)minimum_val;
            decimal[,] arr = new decimal[InputLevel, 2];
            for (int i = 0; i < InputLevel; i++)
            {
                arr[i, 0] = pointer;
                arr[i, 1] = pointer + delta;
                pointer = pointer + delta;
            }


            // Calculate Midpoint
            float[] mid_point = new float[InputLevel];
            for (int i = 0; i < InputLevel; i++)
                mid_point[i] = (float)(arr[i, 0] + arr[i, 1]) / 2;


            // List points in Intervals
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] >= (float)arr[j, 0] && InputSignal.Samples[i] <= (float)arr[j, 1])
                    {
                        OutputIntervalIndices.Add(j + 1);
                        break;
                    }
                }
            }


            // Calculate The real value of signal
            List<float> Qsignal = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
                Qsignal.Add(mid_point[OutputIntervalIndices[i] - 1]);


            // Calculate error
            OutputQuantizedSignal = new Signal(Qsignal, InputSignal.Periodic);
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                decimal e = (decimal)OutputQuantizedSignal.Samples[i] - (decimal)InputSignal.Samples[i];
                OutputSamplesError.Add((float)e);
            }


            // Encoding for the 4 levels 
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputNumBits == 0)
                    InputNumBits = (int)Math.Log(InputLevel, 2);

                string s = "";
                int t = OutputIntervalIndices[i] - 1;

                for (int ii = 0; ii < InputNumBits; ii++)
                {
                    if (t < 1)
                    {
                        s += '0';
                        continue;
                    }

                    if (t % 2 == 0)
                        s += '0';

                    else
                        s += '1';

                    t = t / 2;
                }

                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                string str = new string(charArray);
                OutputEncodedSignal.Add(str);
            }
        }
    }
}
