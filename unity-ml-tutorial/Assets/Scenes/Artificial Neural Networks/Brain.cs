using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNet
{
    public class Brain : MonoBehaviour
    {
        ANN ann;
        double sumSquareError = 0;

        // Start is called before the first frame update
        void Start()
        {
            ann = new ANN(2, 1, 1, 2, 0.8); // keep alpha between 0 and 1

            List<double> result;

            for(int i=0; i<5000; i++)
            {
                //Train an XOR operation
                sumSquareError = 0;
                result = Train(1, 1, 0);
                sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
                result = Train(1, 0, 1);
                sumSquareError += Mathf.Pow((float)result[0] - 1, 2);
                result = Train(0, 1, 1);
                sumSquareError += Mathf.Pow((float)result[0] - 1, 2);
                result = Train(0, 0, 0);
                sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
            }
            Debug.Log("SSE: " + sumSquareError);

            result = Train(1, 1, 0);
            Debug.Log(" 1 1 " + result[0]);
            result = Train(1, 0, 1);
            Debug.Log(" 1 0 " + result[0]);
            result = Train(0, 1, 1);
            Debug.Log(" 0 1 " + result[0]);
            result = Train(0, 0, 0);
            Debug.Log(" 0 0 " + result[0]);
        }

        public List<double> Train(int i1, int i2, int o)
        {
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();
            inputs.Add(i1);
            inputs.Add(i2);
            outputs.Add(o);
            return (ann.Go(inputs,outputs));
        }
    }
}


