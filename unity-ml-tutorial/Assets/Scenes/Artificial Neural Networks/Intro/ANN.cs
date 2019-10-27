using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNet
{
    public class ANN 
    {
        public int numInputs;
        public int numOutputs;
        public int numHidden;
        public int numNPerHidden;
        public double alpha;                        // this is a value that determines how fast the network learns
        List<Layer> layers = new List<Layer>();

        public ANN(int nI, int nO, int nH, int nPH, double a)
        {
            numInputs = nI;
            numOutputs = nO;
            numHidden = nH;
            numNPerHidden = nPH;
            alpha = a;              

            if(numHidden > 0) // if we have hidden layers
            {
                layers.Add(new Layer(numNPerHidden, numInputs));
                for(int i=0; i<numHidden-1; i++)
                {
                    layers.Add(new Layer(numNPerHidden, numNPerHidden));
                }

                layers.Add(new Layer(numOutputs, numNPerHidden));
            }
            else
            {
                layers.Add(new Layer(numOutputs, numInputs));
            }
        }

        public List<double> Go(List<double> inputValues, List<double> desiredOutput)
        {
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();

            if(inputValues.Count != numInputs)
            {
                Debug.Log("ERROR: Number of Inputs must be " + numInputs);
                return outputs;
            }

            // for each layer
            inputs = new List<double>(inputValues);
            for(int i=0; i<numHidden + 1; i++)
            {
                // if we aren't in the input layer
                if (i > 0)
                {
                    inputs = new List<double>(outputs);
                }
                outputs.Clear();

                // for each of the neurons in the layer
                for(int j=0; j<layers[i].numNeurons; j++)
                {
                    double N = 0;
                    layers[i].neurons[j].inputs.Clear();

                    // loop through each neurons input
                    for(int k=0; k<layers[i].neurons[j].numInputs; k++)
                    {
                        layers[i].neurons[j].inputs.Add(inputs[k]);
                        N += layers[i].neurons[j].weights[k] * inputs[k];
                    }

                    N -= layers[i].neurons[j].bias;
                    if(i == numHidden)
                    {
                        layers[i].neurons[j].output = ActivationFunctionO(N);
                    }
                    else
                    {
                        layers[i].neurons[j].output = ActivationFunction(N);
                    }
                    
                    outputs.Add(layers[i].neurons[j].output);
                }
            }

            UpdateWeights(outputs, desiredOutput);

            return outputs;
        }

        public void UpdateWeights(List<double> outputs, List<double> desiredOutputs)
        {
            // iterate through layers
            double error;
            for(int i=numHidden; i>= 0; i--) // we are moving backwards to backpropogate the error through the network
            {
                // iterate through neurons
                for(int j=0; j<layers[i].numNeurons; j++)
                {
                    if(i == numHidden)
                    {
                        error = desiredOutputs[j] - outputs[j];
                        layers[i].neurons[j].errorGradient = outputs[j] * (1 - outputs[j]) * error;
                        // errorGradient calculated with Delta Rule: en.wikipedia.org/wiki/Delta_rule
                    }
                    else
                    {
                        layers[i].neurons[j].errorGradient = layers[i].neurons[j].output * (1 - layers[i].neurons[j].output);
                        double errorGradSum = 0;
                        // iterate through weights
                        for(int p=0; p<layers[i+1].numNeurons; p++)
                        {
                            errorGradSum += layers[i + 1].neurons[p].errorGradient * layers[i + 1].neurons[p].weights[j];
                        }
                        layers[i].neurons[j].errorGradient *= errorGradSum;
                    }

                    // iteration through weights
                    for(int k=0; k<layers[i].neurons[j].numInputs; k++)
                    {
                        if(i == numHidden)
                        {
                            error = desiredOutputs[j] - outputs[j];
                            layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * error;
                        }
                        else
                        {
                            layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * layers[i].neurons[j].errorGradient;
                        }
                    }

                    // update the bias
                    layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;
                }
            }
        }

        // for full list of activation functions
        // see en.wikipedia.org/wiki/Activation_function
        public double ActivationFunction(double value)
        {
            return ReLu(value);
        }

        //Output activation function
        double ActivationFunctionO(double value)
        {
            return Sigmoid(value);
        }

        // binary step
        double Step(double value)
        {
            if (value < 0) return 0;
            else return 1;
        }

        // logistic soft step
        double Sigmoid(double value)
        {
            double k = (double)System.Math.Exp(value);
            return k / (1.0f + k);
        }

        // TanH allows us to return negative values
        double TanH (double value)
        {
            return (2 * (Sigmoid(2 * value)) - 1);
        }

        double ReLu(double value)
        {
            if (value > 0) return value;
            else return 0;
        }


        double LeakyReLu(double value)
        {
            if (value < 0) return 0.01 * value;
            else return value;
        }
    }
}

