using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Maze
{
    public class PopulationManager : MonoBehaviour
    {
        public static float elapsed = 0;
        public GameObject botPrefab;
        public GameObject startPos;
        public int populationSize = 50;
        public float trialTime = 5;

        private List<GameObject> population = new List<GameObject>();
        private int generation = 1;

        #region Unity Methods
        // Start is called before the first frame update
        void Start()
        {
            for(int i=0; i<populationSize; i++)
            {
                GameObject b = Instantiate(botPrefab, startPos.transform.position, this.transform.rotation);
                b.GetComponent<Brain>().Init();
                population.Add(b);
            }
        }

        // Update is called once per frame
        void Update()
        {
            elapsed += Time.deltaTime;
            if(elapsed >= trialTime)
            {
                BreedPopulation();
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
        #endregion Unity Methods


        #region Methods
        private GameObject Breed(GameObject parent1, GameObject parent2)
        {
            GameObject offspring = Instantiate(botPrefab, startPos.transform.position, this.transform.rotation);
            Brain b = offspring.GetComponent<Brain>();

            if (Random.Range(0,100) == 1) //mutate 1%
            {
                b.Init();
                b.dna.Mutate();
            }
            else
            {
                b.Init();
                b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
            }

            return offspring;
        }

        private void BreedPopulation()
        {
            List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().distTravelled).ToList();

            population.Clear();
            for(int i = (int)(sortedList.Count/2.0f) - 1; i < sortedList.Count-1; i++)
            {
                for(int j = (int)(sortedList.Count/2.0f) + 1; i < sortedList.Count -1; i++)
                {
                    population.Add(Breed(sortedList[i], sortedList[j]));
                    population.Add(Breed(sortedList[j], sortedList[i]));
                }
            }

            //destroy all parents
            for(int i=0; i<sortedList.Count; i++)
            {
                Destroy(sortedList[i]);
            }

            generation++;
        }
        #endregion Methods
    }
}


