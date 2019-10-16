/* PopulationManager.cs
 * 
 * Brief:           This manages the population during game execution.
 * Author:          Drew Massey
 * Date Created:    10/15/2019
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    #region Variables
    public static float elapsed = 0;
    public GameObject personPrefab;
    public int populationSize = 10;

    private List<GameObject> population = new List<GameObject>();
    private int trialTime = 10;
    private int generation = 1;
    #endregion Variables

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-7, 7), Random.Range(-4.5f, 4.5f), 0);
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
            go.GetComponent<Dna>().r = Random.Range(0.0f, 1.0f);
            go.GetComponent<Dna>().g = Random.Range(0.0f, 1.0f);
            go.GetComponent<Dna>().b = Random.Range(0.0f, 1.0f);
            population.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 20), $"Generation: {generation}", guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), $"Trial Time: {(int)elapsed}", guiStyle);
    }
    #endregion Unity Methods

    #region Methods
    /// <summary>
    /// Creates a new population of people
    /// </summary>
    public void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        // Sort by how long they lived
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Dna>().timeToDie).ToList();

        population.Clear();
        // Breed the upper half of the population
        for(int i= (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
        }

        // Destroy all parents and previous population
        for(int i = 0; i<sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        generation++;
    }

    /// <summary>
    /// Combines the color of two people and returns a new one
    /// </summary>
    /// <param name="parent1"></param>
    /// <param name="parent2"></param>
    /// <returns> The new person </returns>
    public GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-7, 7), Random.Range(-4.5f, 4.5f), 0);
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);
        Dna dna1 = parent1.GetComponent<Dna>();
        Dna dna2 = parent2.GetComponent<Dna>();

        // Swap parent dna
        // ** This is the guts of the genetic algorithm system **
        if (Random.Range(0, 20) < 1)
        {
            offspring.GetComponent<Dna>().r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
            offspring.GetComponent<Dna>().g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
            offspring.GetComponent<Dna>().b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
        }
        else // mutate 5% of the time
        {
            offspring.GetComponent<Dna>().r = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<Dna>().g = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<Dna>().b = Random.Range(0.0f, 1.0f);
        }
        

        return offspring;
    }
    #endregion Methods
}
