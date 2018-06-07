using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron 
{
    #region PROPERTIES
    public int InputsCount { get; private set; }
    public double Bias { get; private set; }
    public double Output { get; set; }

    public List<double> Inputs { get; private set; } = new List<double>(); 
    public List<double> Weights { get; private set; } = new List<double>();
    #endregion

    #region FIELDS
    private readonly double errorGradient;

    #endregion
    
    #region CONSTRUCTORS
    public Neuron (int numberOfInputs)
    {
        Bias = Random.Range(-1f, 1f);
        InputsCount = numberOfInputs;
        for (var i = 0; i < numberOfInputs; i++)
        {
            Weights.Add(Random.Range(-1f, 1f));
        }
    } 
    #endregion
}
