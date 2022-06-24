using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }   // signal
        public FILTER_TYPES InputFilterType { get; set; }   // type
        public float InputFS { get; set; }                  // sampling frequancy
        public float? InputCutOffFrequency { get; set; }    // cut off frequency in high or low
        public float? InputF1 { get; set; }                 // cut off frequency band pass and band stop filters
        public float? InputF2 { get; set; }                 // cut off frequency band pass and band stop filters
        public float InputStopBandAttenuation { get; set; } // Stop Band Attenuation
        public float InputTransitionBand { get; set; }      // Transition width
        public Signal OutputHn { get; set; }                // output h
        public Signal OutputYn { get; set; }                // output y
        

        string type;
        double N;

        public override void Run()
        {
            if (InputFilterType == FILTER_TYPES.HIGH)
                InputCutOffFrequency = InputCutOffFrequency - (InputTransitionBand / 2);
            else
                InputCutOffFrequency = InputCutOffFrequency + (InputTransitionBand / 2);
            
            InputCutOffFrequency = InputCutOffFrequency / InputFS;

            float fc1 = (float)0;
            float fc2 = (float)0;

            if (InputF1 != null && InputF2 != null)
            {
                if (InputFilterType == FILTER_TYPES.BAND_PASS) 
                {
                    fc1 = (float)(InputF1 - (InputTransitionBand / 2)) / InputFS;
                    fc2 = (float)(InputF2 + (InputTransitionBand / 2)) / InputFS; 
                }
                else 
                {
                    fc1 = (float)(InputF1 + (InputTransitionBand / 2)) / InputFS;
                    fc2 = (float)(InputF2 - (InputTransitionBand / 2)) / InputFS;
                }
            }

            InputTransitionBand = InputTransitionBand / InputFS;

            if (InputStopBandAttenuation <= 21)
            {
                type = "Rectangular";
                N = 0.9 / InputTransitionBand;
            }
            else if (InputStopBandAttenuation <= 44)
            {
                type = "Hanning";
                N = 3.1 / InputTransitionBand;
            }
            else if (InputStopBandAttenuation <= 53)
            {
                type = "Hamming";
                N = 3.3 / InputTransitionBand;
            }
            else
            {
                type = "Blackman";
                N = 5.5 / InputTransitionBand;
            }

            N = Math.Ceiling(N);
            if (N % 2 == 0)
                N++;

            double[] hdelta = new double[(int)((N - 1) / 2) + 1];
            double[] w = new double[(int)((N - 1) / 2) + 1];
            List<float> h = new List<float>((int)((N - 1) / 2) + 1);
            List<float> reverce_h = new List<float>(h);
            List<float> all_h = new List<float>(h);
            List<int> index1 = new List<int>((int)N);
            
            for (int i = 0; i <= (N - 1) / 2; i++)
            {
                if (InputFilterType.Equals(FILTER_TYPES.LOW))
                    hdelta[i] = calculate_HDelta("LOW", i, fc1, fc2);
                else if (InputFilterType.Equals(FILTER_TYPES.HIGH))
                    hdelta[i] = calculate_HDelta("HIGH", i, fc1, fc2);
                else if (InputFilterType.Equals(FILTER_TYPES.BAND_PASS))
                    hdelta[i] = calculate_HDelta("BAND_PASS", i, fc1, fc2);
                else
                    hdelta[i] = calculate_HDelta("BAND_STOP", i, fc1, fc2);
            }

            for (int i = 0; i <= (N - 1) / 2; i++)
                w[i] = calculate_Window(type, i);

            for (int i = 0; i <= (N - 1) / 2; i++)
                h.Add((float)(hdelta[i] * w[i]));

            for (int i = 0; i <= (N - 1) / 2; i++)
                reverce_h.Add(h[i]);

            reverce_h.Reverse();

            for (int i = 0; i <= (N - 1) / 2; i++)
                all_h.Add(reverce_h[i]);
            
            for (int i = 1; i <= (N - 1) / 2; i++)
                all_h.Add(h[i]);

            for (int i = (int)(-1 * (N - 1) / 2); i <= (int)((N - 1) / 2); i++)
                index1.Add(i);

            OutputHn = new Signal(all_h, false);
            OutputHn.SamplesIndices = index1;

            DirectConvolution dc = new DirectConvolution();
            dc.InputSignal1 = InputTimeDomainSignal;
            dc.InputSignal2 = OutputHn;
            dc.Run();
            for (int i = 0; i < dc.OutputConvolvedSignal.Samples.Count; i++)
                dc.OutputConvolvedSignal.Samples[i] = dc.OutputConvolvedSignal.Samples[i];
            
            OutputYn = dc.OutputConvolvedSignal;
        }

        public double calculate_HDelta(string type, int n, float fc1, float fc2)
        {
            if (type == "LOW")
            {
                if (n == 0)
                    return (float)(2 * InputCutOffFrequency);
                else
                    return (float)(2 * InputCutOffFrequency * (Math.Sin((double)(n * (2 * Math.PI * InputCutOffFrequency))) / (n * (2 * Math.PI * InputCutOffFrequency))));
            }
            else if (type == "HIGH")
            {
                if (n == 0)
                    return (float)(1 - (2 * InputCutOffFrequency));
                else
                    return (float)(-2 * InputCutOffFrequency * (Math.Sin((double)(n * (2 * Math.PI * InputCutOffFrequency))) / (n * (2 * Math.PI * InputCutOffFrequency))));
            }
            else if (type == "BAND_PASS")
            {
                if (n == 0)
                    return (float)(2 * (fc2 - fc1));
                else
                {
                    double theta = (double)(n * 2 * Math.PI * (double)fc2);
                    float x = (float)((2 * fc2) * (Math.Sin(theta) / (n * 2 * Math.PI * fc2)));
                    return x - (float)((2 * fc1) * (Math.Sin((double)(n * 2 * Math.PI * (double)fc1)) / (n * (2 * Math.PI * fc1))));
                }
            }
            else if (type == "BAND_STOP")
            {
                if (n == 0)
                    return (float)(1 - (2 * (fc2 - fc1)));
                else
                {
                    float x = (float)((2 * fc2) * (Math.Sin((double)(n * 2 * Math.PI * (double)fc2)) / (n * 2 * Math.PI * fc2)));
                    return (float)((2 * fc1) * (Math.Sin((double)(n * 2 * Math.PI * (double)fc1)) / (n * (2 * Math.PI * fc1)))) - x;
                }
            }
            return -1;
        }

        public double calculate_Window (string type, int n)
        {
            if (type == "Rectangular")
                return (float)1;
            else if (type == "Hanning")
                return (float)(0.5 + 0.5 * (Math.Cos(((2 * 180 * n) / N) * (Math.PI / 180f))));
            else if (type == "Hamming")
                return (float)(0.54 + 0.46 * (Math.Cos(((2 * 180 * n) / N) * (Math.PI / 180f))));
            else if (type == "Blackman")
                return (float)(0.42 + 0.5 * (Math.Cos(((2 * 180 * n) / (N - 1)) * (Math.PI / 180f))) + 0.08 * (Math.Cos(((4 * 180 * n) / (N - 1)) * (Math.PI / 180f))));
            return 0.1;
        }
    }
}