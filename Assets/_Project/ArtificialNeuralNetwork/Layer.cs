using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer
{
    public int NeuronsCount { get; private set; }
    public List<Neuron> Neurons { get; private set; } = new List<Neuron>();

    public Layer (int neuronsCount, int neuronInputsCount)
    {
        NeuronsCount = neuronsCount;
        for (var i = 0; i < neuronsCount; i++)
        {
            Neurons.Add(new Neuron(neuronInputsCount));
        }
    }
}
