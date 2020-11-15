using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarMathLib;

namespace AOIS3.Prep
{
    public class NeuralNetwork
    {
        private double[,] _input;
        private double[,] _output;

        private List<double[,]> _layerOutputs;
        private List<double[,]> _activatedOutputs;

        private List<HiddenLayer> _hiddenLayers = new List<HiddenLayer>();
        public NeuralNetwork() { }
        public NeuralNetwork AddInputLayer(int size)
        {
            if (_input != null)
                throw new ApplicationException("Input layer already exists.");

            _input = new double[size, 1];
            return this;
        }
        public NeuralNetwork AddHiddenLayer(int outputSize)
        {
            if(_input == null)
                throw new ApplicationException("you should first add the input layer.");
            if (_hiddenLayers.Any())
            {
                _hiddenLayers.Add(
                    new HiddenLayer(outputSize, _hiddenLayers.Last().GetSize())
                );
            }
            else
            {
                _hiddenLayers.Add(
                    new HiddenLayer(outputSize, _input.GetLength(0))
                );
            }
            return this;
        }
        public NeuralNetwork AddOutputLayer(int size)
        {
            if (_output != null)
                throw new ApplicationException("Input layer already exists.");
            if (!_hiddenLayers.Any() || 
                _hiddenLayers.Last().GetSize() != size)
            {
                throw new ApplicationException("Input layer already exists.");
            }
            _output = new double[size, 1];

            return this;
        }

        private void ForwardPass(double[,] input) 
        {
            _input = input;
            for(int i = 0; i < _hiddenLayers.Count; i ++)
            {
                input = _hiddenLayers[i].FeedForward(input);
                _layerOutputs.Add(input);
                input = Sigmoid(input);
                _activatedOutputs.Add(input);
            }
            input = SoftMax((double[,])_layerOutputs.Last().Clone());
            _activatedOutputs[_activatedOutputs.Count - 1] = input;

        }

        private List<double[,]> BackwardPass(double[,] output) 
        {
            List<double[,]> weightChanges = new List<double[,]>(_hiddenLayers.Count);
            _output = output;
            double[,] error = (_activatedOutputs.Last().subtract(output))
                .multiply(2)
                .divide(output.Length)
                .multiply(SoftMaxDerivative(_layerOutputs.Last()));
            weightChanges[weightChanges.Count - 1] =
                error.multiply(_activatedOutputs[_activatedOutputs.Count - 2].transpose());
            for(int i = weightChanges.Count - 2; i >= 0; i--)
            {
                error = _hiddenLayers[i + 1].GetWeights()
                    .transpose()
                    .multiply(error)
                    .multiply(SigmoidDerivative(_layerOutputs[i]));
                weightChanges[i] =
                    error.multiply(_activatedOutputs[i].transpose());
            }
            return weightChanges;

        }
        private double[,] GetMatrixFromArray(double[] array)
        {
            double[,] matrix = new double[array.Length, 1];
            for(int i = 0; i < array.Length; i++)
            {
                matrix[i, 0] = array[i];
            }
            return matrix;
        }
        private double[,] Sigmoid(double[,] array)
        {
            for( int i = 0; i < array.Length; i++)
            {
                array[i, 0] = 1 / (1 + Math.Exp(-array[i, 0]));
            }
            return array;
        }
        private double[,] SigmoidDerivative(double[,] array)
        {
            double[,] exps = new double[array.Length, 1];
            for (int i = 0; i < exps.Length; i++)
            {
                exps[i, 0] = Math.Exp(-array[i, 0]);
            }
            double[,] result = new double[array.Length, 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i, 0] = exps[i, 0] / Math.Pow(exps[i, 0] + 1, 2);
            }
            return array;
        }

        private double[,] SoftMax(double[,] array)
        {
            double[,] exps = new double[array.Length, 1];
            for (int i = 0; i < exps.Length; i++)
            {
                exps[i, 0] = Math.Exp(array[i, 0] - array.Max());
            }
            double[,] result = new double[array.Length, 1];
            for (int i = 0; i < exps.Length; i++)
            {
                result[i, 0] = Math.Exp(exps[i, 0] / exps.SumAllElements());
            }
            return result;
        }

        private double[,] SoftMaxDerivative(double[,] array)
        {
            double[,] exps = new double[array.Length, 1];
            for (int i = 0; i < exps.Length; i++)
            {
                exps[i, 0] = Math.Exp(array[i, 0] - array.Max());
            }
            double[,] temp1 = new double[array.Length, 1];
            for (int i = 0; i < temp1.Length; i++)
            {
                temp1[i, 0] = Math.Exp(exps[i, 0] / exps.SumAllElements());
            }
            double[,] temp2 = new double[array.Length, 1];
            for (int i = 0; i < temp2.Length; i++)
            {
                temp2[i, 0] = 1 - temp1[i,0];
            }
            double[,] result = new double[array.Length, 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i, 0] = temp1[i, 0] * temp2[i, 0];
            }
            return result;
        }
    }
}
