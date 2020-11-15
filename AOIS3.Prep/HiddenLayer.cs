using System;
using System.Collections.Generic;
using System.Text;
using StarMathLib;
namespace AOIS3.Prep
{
    public class HiddenLayer
    {
        private double[,] _weights;
        public HiddenLayer(int curLayerSize, int prevLayerSize)
        {
            _weights = new double[curLayerSize, prevLayerSize];
            Random random = new Random();
            for (int i = 0; i < curLayerSize; i++)
            {
                for (int j = 0; j < prevLayerSize; j++)
                {

                    double u1 = 1.0 - random.NextDouble(); 
                    double u2 = 1.0 - random.NextDouble();
                    double randNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                 Math.Sin(2.0 * Math.PI * u2) * 
                                 Math.Sqrt(curLayerSize);
                    _weights[i, j] = randNormal;
                }
            }
        }
        public double[,] FeedForward(double[,] input)
        {
            double[,] output =
                _weights.multiply(input);

            return output;
        }

        public void UpdateWeights(double[,] error, double learningRate)
        {
            _weights = _weights.subtract(error.multiply(learningRate));
        }

        public int GetSize()
        {
            return _weights.GetLength(0);
        }

        public double[,] GetWeights()
        {
            return _weights;
        }
    }
}
