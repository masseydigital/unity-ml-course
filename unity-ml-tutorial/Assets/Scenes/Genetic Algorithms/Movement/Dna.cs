using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dna
{
    private List<int> genes = new List<int>();
    private int dnaLength = 0;
    private int maxValues = 0;

    public Dna(int l, int v)
    {
        dnaLength = l;
        maxValues = v;
        SetRandom();
    }
    
    /// <summary>
    /// Add a random gene value
    /// </summary>
    public void SetRandom()
    {
        genes.Clear();
        for(int i=0 ;i<dnaLength; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    /// <summary>
    /// Sets a gene to a value
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="value"></param>
    public void SetInt(int pos, int value)
    {
        genes[pos] = value;
    }

    /// <summary>
    /// Combines two genes together
    /// </summary>
    /// <param name="dna1"></param>
    /// <param name="dna2"></param>
    public void Combine(Dna dna1, Dna dna2)
    {
        for (int i = 0; i < dnaLength; i++)
        {
            if (i < dnaLength / 2.0)
            {
                int c = dna1.genes[i];
                genes[i] = c;
            }
            else
            {
                int c = dna2.genes[i];
                genes[i] = c;
            }
        }
    }

    /// <summary>
    /// Generate a random value for a gene
    /// </summary>
    public void Mutate()
    {
        genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
    }

    /// <summary>
    /// Get a gene at a position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetGene(int pos)
    {
        return genes[pos];
    }
}
