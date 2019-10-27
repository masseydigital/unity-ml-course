using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public float speed = 50;
    public float rotationSpeed = 100.0f;
    public float visibleDistance = 50.0f;
    List<string> collectedTrainingData = new List<string>();
    StreamWriter tdf;

    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/trainingData.txt";
        tdf = File.CreateText(path);
    }

    // Update is called once per frame
    void Update()
    {
        float tInput = Input.GetAxis("Vertical");
        float rInput = Input.GetAxis("Horizontal");

        float translation =  tInput * speed * Time.deltaTime;
        float rotation =  rInput * rotationSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        Debug.DrawRay(transform.position, this.transform.forward * visibleDistance, Color.red); // forward
        Debug.DrawRay(transform.position, this.transform.right * visibleDistance, Color.red); // right
        Debug.DrawRay(transform.position, -this.transform.right * visibleDistance, Color.red); // left
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-45, Vector3.up) * transform.right * visibleDistance, Color.red); // r45
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -transform.right * visibleDistance, Color.red); // l45

        RaycastHit hit;
        float fDist = 0, rDist = 0, lDist = 0, r45Dist = 0, l45Dist = 0;

        // These values will become the inputs into the neural network //

        //forward
        if(Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance))
        {
            fDist = 1 - Round(hit.distance/visibleDistance);
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

        // Output to a CSV //

        string td = fDist + "," + rDist + "," + lDist + "," + r45Dist + "," + l45Dist + "," + Round(tInput) + "," + Round(rInput);

        if(!collectedTrainingData.Contains(td))
        {
            collectedTrainingData.Add(td);
        }
    }

    private void OnApplicationQuit()
    {
        foreach(string td in collectedTrainingData)
        {
            tdf.WriteLine(td);
        }

        tdf.Close();
    }

    public float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
}
