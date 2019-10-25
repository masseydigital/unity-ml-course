using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron : MonoBehaviour
{
    public TrainingSet[] ts;

    double[] weights = { 0, 0 };
    double bias = 0;
    double totalError = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Train 8 epochs
        Train(8);

        // Show what our perceptron has become (OR)
        Debug.Log("Test 0 0: " + CalcOutput(0, 0));
        Debug.Log("Test 0 1: " + CalcOutput(0, 1));
        Debug.Log("Test 1 0: " + CalcOutput(1, 0));
        Debug.Log("Test 1 1: " + CalcOutput(1, 1));

    }

    // Initialize the weights to random values
    private void InitializeWeights()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Random.Range(-1.0f, 1.0f);
        }

        bias = Random.Range(-1.0f, 1.0f);
    }

    // Train the perceptron based on the number of epochs
    private void Train(int epochs)
    {
        InitializeWeights();

        for(int e=0; e<epochs; e++)
        {
            totalError = 0;
            for(int t=0; t<ts.Length; t++)
            {
                UpdateWeights(t);
                Debug.Log("W1: " + (weights[0]) + " W2: " + (weights[1]) + " B: " + bias);
            }
            Debug.Log("TOTAL ERROR: " + totalError);
        }
    }

    // Update the weights with the error
    private void UpdateWeights(int j)
    {
        double error = ts[j].output - CalcOutput(j);
        totalError += Mathf.Abs((float)error);
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = weights[i] + error * ts[j].input[i];
        }
        bias += error;
    }

    // Utilize the dot product to introduce bias
    private double DotProductBias(double[] v1, double[] v2)
    {
        if(v1 == null || v2 == null)
        {
            return -1;
        }

        if(v1.Length != v2.Length)
        {
            return -1;
        }

        double d = 0;
        for(int x=0; x<v1.Length; x++)
        {
            d += v1[x] * v2[x];
        }

        d += bias;

        return d;
    }

    // Calculate the output
    private double CalcOutput(int i)
    {
        double dp = DotProductBias(weights, ts[i].input);
        if (dp > 0) return (1);
        return (0);
    }

    // Calculate the output
    private double CalcOutput(int i1, int i2)
    {
        double[] inp = new double[] { i1, i2 };
        double dp = DotProductBias(weights, inp);
        if (dp > 0) return (1);
        return (0);
    }
}

[System.Serializable]
public class TrainingSet
{
    public double[] input;
    public double output;
}
