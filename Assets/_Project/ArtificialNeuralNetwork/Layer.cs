using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer
{
    public int NeuronsCount { get; private set; }
    private List<Neuron> neurons = new List<Neuron>();

    public Layer (int neuronsCount, int neuronInputsCount)
    {
        NeuronsCount = neuronsCount;
        for (var i = 0; i < neuronsCount; i++)
        {
            neurons.Add(new Neuron(neuronInputsCount));
        }
    }
}
