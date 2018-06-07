using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialNeuralNetwork : MonoBehaviour 
{
    private readonly int inputsCount;
    private readonly int outputsCount;
    private readonly int hiddenLayersCount;
    private readonly int neuronsPerHiddenLayer;
    private readonly double alpha; // A percentage of the training set that is integrated each time it learns (weights are adjusted).

    private readonly List<Layer> layers = new List<Layer>();

    #region CONSTRUCTORS
    public ArtificialNeuralNetwork (int inputsCount, int outputsCount, int hiddenLayersCount, int neuronsPerHiddenLayer, double alpha)
    {
        this.inputsCount = inputsCount;
        this.outputsCount = outputsCount;
        this.hiddenLayersCount = hiddenLayersCount;
        this.neuronsPerHiddenLayer = neuronsPerHiddenLayer;
        this.alpha = alpha;

        if (this.hiddenLayersCount > 0)
        {
            // Input layer.
            layers.Add(new Layer(this.neuronsPerHiddenLayer, this.inputsCount));

            // Hidden layers.
            for (var i = 0; i < this.hiddenLayersCount - 1; i++)
            {
                layers.Add(new Layer(this.neuronsPerHiddenLayer, this.neuronsPerHiddenLayer));
            }

            // Output layer.
            layers.Add(new Layer(this.outputsCount, this.neuronsPerHiddenLayer));
        }
        // No hidden layers.
        else
        {
            // Output layer.
            layers.Add(new Layer(this.outputsCount, this.inputsCount));
        }
    } 
    #endregion

    // Use this for initialization
    private void Start () 
	{
		
	}
	
	// Update is called once per frame
	private void Update () 
	{
		
	}

    // Run through NN in order to get an output.
    public List<double> Go (List<double> inputValues, List<double> desiredOutputs)
    {
        List<double> inputs = new List<double>();
        List<double> outputs= new List<double>();

        if (inputValues.Count != inputsCount)
        {
            Debug.LogError("Number of inputs must be " + inputsCount);
        }

        inputs = new List<double>(inputValues);

        // Loop through all layers (input layer + hidden layers + output layer).
        for (var currentLayer = 0; currentLayer < hiddenLayersCount + 1; currentLayer++) //TODO: replace i with currentLayer?
        {
            // Layer is not input layer.
            if (currentLayer > 0)
            {
                // Takes the outputs from the previous layer.
                inputs = new List<double>(outputs);
            }
            outputs.Clear();

            // Loop through all neurons in that layer.
            for (var currentNeuron = 0; currentNeuron < layers[currentLayer].NeuronsCount; currentNeuron++) //TODO: replace j with currentNeuron?
            {
                double dotProduct = 0;
                layers[currentLayer].Neurons[currentNeuron].Inputs.Clear();

                // Loop through all inputs of the neuron.
                for (var currentInput = 0; currentInput < layers[currentLayer].Neurons[currentNeuron].InputsCount; currentInput++) //TODO: replace k with currentInput?
                {
                    // For each neuron's input, add in the input from the one before. 
                    layers[currentLayer].Neurons[currentNeuron].Inputs.Add(inputs[currentInput]);

                    // Dotproduct.
                    dotProduct += layers[currentLayer].Neurons[currentNeuron].Weights[currentInput] * inputs[currentInput];
                }

                dotProduct -= layers[currentLayer].Neurons[currentNeuron].Bias;
                layers[currentLayer].Neurons[currentNeuron].Output = ActivationFunction(dotProduct);
                // Serves as the input for the next layer.
                outputs.Add(layers[currentLayer].Neurons[currentNeuron].Output);
            }
        }
        UpdateWeights(outputs, desiredOutputs);
        return outputs;
    }

    private void UpdateWeights (List<double> outputs, List<double> desiredOutputs)
    {
        double error;

        for ()
        {

        }
    }

    private double ActivationFunction (double dotProduct)
    {
        //TODO: Implementation.
        throw new System.NotImplementedException();
    }
}
