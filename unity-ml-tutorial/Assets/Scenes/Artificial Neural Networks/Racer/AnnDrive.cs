using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AnnDrive : MonoBehaviour
{
    public float visibleDistance = 50;
    public int epochs = 1000;
    public float speed = 50.0f;
    public float rotationSpeed = 100.0f;
    public float translation;
    public float rotation;
    public bool loadFromFile = false;

    ANN ann;
    bool trainingDone = false;
    float trainingProgress = 0;
    double sse = 0;
    double lastSse = 1;

    // Start is called before the first frame update
    void Start()
    {
        ann = new ANN(5, 2, 1, 10, 0.5);

        if (loadFromFile)
        {
            LoadWeightsFromFile();
            trainingDone = true;
        }
        else
            StartCoroutine(LoadTrainingSet());
    }

    // Update is called once per frame
    void Update()
    {
        if (!trainingDone) return;

        List<double> calcOutputs = new List<double>();
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();

        RaycastHit hit;
        float fDist = 0, rDist = 0, lDist = 0, r45Dist = 0, l45Dist = 0;
        // These values will become the inputs into the neural network //

        Debug.DrawRay(transform.position, this.transform.forward * visibleDistance, Color.green); // forward
        Debug.DrawRay(transform.position, this.transform.right * visibleDistance, Color.green); // right
        Debug.DrawRay(transform.position, -this.transform.right * visibleDistance, Color.green); // left
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-45, Vector3.up) * transform.right * visibleDistance, Color.green); // r45
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -transform.right * visibleDistance, Color.green); // l45

        //forward
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance))
        {
            fDist = 1 - Round(hit.distance / visibleDistance);
        }
        //right
        if (Physics.Raycast(transform.position, this.transform.right, out hit, visibleDistance))
        {
            rDist = 1 - Round(hit.distance / visibleDistance);
        }
        //left
        if (Physics.Raycast(transform.position, -this.transform.right, out hit, visibleDistance))
        {
            lDist = 1 - Round(hit.distance / visibleDistance);
        }
        //right 45
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-45, Vector3.up) * transform.right, out hit, visibleDistance))
        {
            r45Dist = 1 - Round(hit.distance / visibleDistance);
        }
        //left 45
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -transform.right, out hit, visibleDistance))
        {
            l45Dist = 1 - Round(hit.distance / visibleDistance);
        }

        // send the inputs to the neural network
        inputs.Add(fDist);
        inputs.Add(rDist);
        inputs.Add(lDist);
        inputs.Add(r45Dist);
        inputs.Add(l45Dist);
        outputs.Add(0);
        outputs.Add(0);

        calcOutputs = ann.CalcOutput(inputs,outputs);

        // map the values back to -1 and 1
        float tInput = Map(-1, 1, 0, 1, (float)calcOutputs[0]);
        float rInput = Map(-1, 1, 0, 1, (float)calcOutputs[1]);

        // send the outputs
        translation = tInput * speed * Time.deltaTime;
        rotation = rInput * rotationSpeed * Time.deltaTime;
        this.transform.Translate(0, 0, translation);
        this.transform.Rotate(0, rotation, 0);

        // if we found a good kart, save it
        if (Input.GetKeyUp("l"))
        {
            SaveWeightsToFile();
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 250, 30), "SSE: " + lastSse);
        GUI.Label(new Rect(25, 40, 250, 30), "Alpha: " + ann.alpha);
        GUI.Label(new Rect(25, 55, 250, 30), "Trained: " + trainingProgress);
    }

    void SaveWeightsToFile()
    {
        string path = Application.dataPath + "/weights.txt";
        StreamWriter wf = File.CreateText(path);
        wf.WriteLine(ann.PrintWeights());
        wf.Close();
    }

    void LoadWeightsFromFile()
    {
        string path = Application.dataPath + "/weights.txt";
        StreamReader wf = File.OpenText(path);

        if (File.Exists(path))
        {
            string line = wf.ReadLine();
            ann.LoadWeights(line);
        }
    }

    public IEnumerator LoadTrainingSet()
    {
        string path = Application.dataPath + "/trainingData.txt";
        string line;

        if (File.Exists(path))
        {
            int lineCount = File.ReadAllLines(path).Length;
            StreamReader tdf = File.OpenText(path);
            List<double> calcOutputs = new List<double>();
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();

            for(int i=0; i<epochs; i++)
            {
                sse = 0;
                tdf.BaseStream.Position = 0;
                string currentWeights = ann.PrintWeights();
                while((line = tdf.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    float thisError = 0;

                    // this if gets rid of any training data where the car isn't moving because
                    // the player is afk or being silly
                    if(System.Convert.ToDouble(data[5]) != 0 && System.Convert.ToDouble(data[6]) != 0)
                    {
                        inputs.Clear();
                        outputs.Clear();
                        inputs.Add(System.Convert.ToDouble(data[0]));
                        inputs.Add(System.Convert.ToDouble(data[1]));
                        inputs.Add(System.Convert.ToDouble(data[2]));
                        inputs.Add(System.Convert.ToDouble(data[3]));
                        inputs.Add(System.Convert.ToDouble(data[4]));

                        double o1 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[5])); // normalizes them to 0 -> 1
                        outputs.Add(o1);
                        double o2 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[6])); // normalizes them to 0 -> 1
                        outputs.Add(o2);

                        // train the output
                        calcOutputs = ann.Train(inputs, outputs);
                        
                        // sum of the squares of the errors
                        thisError = ((Mathf.Pow((float)(outputs[0] - calcOutputs[0]), 2) +
                            Mathf.Pow((float)(outputs[1] - calcOutputs[1]), 2))) / 2.0f;
                    }
                    sse += thisError;
                }

                trainingProgress = (float)i / (float)epochs;
                sse /= lineCount;
                
                // if sse isn't better then reload previous set of weights and decrease alpha
                if(lastSse < sse)
                {
                    ann.LoadWeights(currentWeights);
                    ann.alpha = Mathf.Clamp((float)ann.alpha - 0.001f, 0.01f, 0.9f);
                }
                else // increase alpha
                {
                    ann.alpha = Mathf.Clamp((float)ann.alpha + 0.001f, 0.01f, 0.9f);
                    lastSse = sse;
                }

                yield return null;
            }
        }

        trainingDone = true;
    }

    /// <summary>
    /// Maps a value into a new range
    /// </summary>
    /// <param name="newFrom"></param>
    /// <param name="newTo"></param>
    /// <param name="origFrom"></param>
    /// <param name="origTo"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public float Map(float newFrom, float newTo, float origFrom, float origTo, float value)
    {
        if (value <= origFrom)
            return newFrom;
        else if (value >= origTo)
            return newTo;
        return (newTo - newFrom) * ((value - origFrom) / (origTo - origFrom)) + newFrom;
    }

    /// <summary>
    /// Round a number
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
}
