using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron 
{
    #region FIELDS
    int numberOfInputs;
    double bias;
    double desiredOutput;
    double errorGradient;

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
