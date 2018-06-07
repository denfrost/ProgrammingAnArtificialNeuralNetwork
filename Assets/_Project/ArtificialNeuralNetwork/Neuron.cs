using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron 
{
    #region FIELDS
    private readonly int numberOfInputs;
    private readonly double bias;
    private readonly double errorGradient;
    private readonly double desiredOutput;

    List<double> weights = new List<double>();
    List<double> inputs = new List<double>();
    #endregion
    
    #region CONSTRUCTORS
    public Neuron (int numberOfInputs)
    {
        bias = Random.Range(-1f, 1f);
        this.numberOfInputs = numberOfInputs;
        for (var i = 0; i < numberOfInputs; i++)
        {
            weights.Add(Random.Range(-1f, 1f));
        }
    } 
    #endregion
}
