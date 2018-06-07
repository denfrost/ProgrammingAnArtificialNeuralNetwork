using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer
{
    private int numberOfNeurons;
    private List<Neuron> neurons = new List<Neuron>();

    public Layer (int numberOfNeurons, int numberOfNeuronInputs)
    {
        this.numberOfNeurons = numberOfNeurons;
        for (var i = 0; i < numberOfNeurons; i++)
        {
            neurons.Add(new Neuron(numberOfNeuronInputs));
        }
    }
}
