using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlatformPopulationManager : MonoBehaviour
{
    #region Variables
    public static float elapsed = 0;
    public GameObject botPrefab;
    public int populationSize = 50;
    public float trialTime = 5;

    private List<GameObject> population = new List<GameObject>();
    private int generation = 1;
    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2),
                                                this.transform.position.y,
                                                this.transform.position.z + Random.Range(-2, 2));

            GameObject b = Instantiate(botPrefab, startingPos, this.transform.rotation);
            b.GetComponent<PlatformerBrain>().Init();
            population.Add(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), $"Gen: {generation}", guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0:00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), $"Population: {population.Count}", guiStyle);
        GUI.EndGroup();
    }

    #region Methods
    /// <summary>
    /// Breed two brains together
    /// </summary>
    /// <param name="parent1"></param>
    /// <param name="parent2"></param>
    /// <returns></returns>
    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2),
                                                this.transform.position.y,
                                                this.transform.position.z + Random.Range(-2, 2));
        GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
        PlatformerBrain b = offspring.GetComponent<PlatformerBrain>();

        // Attempt to mutate 1% of the population
        if (Random.Range(0, 100) == 1)
        {
            b.Init();
            b.dna.Mutate();
        }
        else
        {
            b.Init();
            b.dna.Combine(parent1.GetComponent<PlatformerBrain>().dna, parent2.GetComponent<PlatformerBrain>().dna);
        }

        return offspring;
    }

    /// <summary>
    /// Breeds a new population for the next generation
    /// This includes the fitness function.
    /// </summary>
    private void BreedNewPopulation()
    {
        List<GameObject> sortedList = 
            population.OrderBy(o => 
            (o.GetComponent<PlatformerBrain>().timeWalking*5 + o.GetComponent<PlatformerBrain>().timeAlive)).ToList();

        population.Clear();

        // Breed upper half of the sorted list
        for (int i = (int)(sortedList.Count / 2.0f)-1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // Destroy all parents and previous population
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        generation++;
    }
    #endregion Methods
}
