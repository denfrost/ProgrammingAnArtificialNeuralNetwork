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
        for (var i = 0; i < hiddenLayersCount + 1; i++) //TODO: replace i with currentLayer?
        {
            // Layer is not input layer.
            if (i > 0)
            {
                // Takes the outputs from the previous layer.
                inputs = new List<double>(outputs);
            }
            outputs.Clear();

            // Loop through the number of neurons.
            for (var j = 0; j < layers[i].NeuronsCount; j++) //TODO: replace j with currentNeuron?
            {
                double dotProduct = 0;
                layers[i].Neurons[j].Inputs.Clear();

                // Loop through the number of the neuron's inputs.
                for (var k = 0; k < layers[i].Neurons[j].InputsCount; k++) //TODO: replace k with currentInput?
                {
                    // For each neuron's input, add in the input from the one before. 
                    layers[i].Neurons[j].Inputs.Add(inputs[k]);

                    // Dotproduct.
                    dotProduct += layers[i].Neurons[j].Weights[k] * inputs[k];
                }

                dotProduct -= layers[i].Neurons[j].Bias;
                layers[i].Neurons[j].Output = ActivationFunction(dotProduct);
                // Serves as the input for the next layer.
                outputs.Add(layers[i].Neurons[j].Output);
            }
        }
    }

    private double ActivationFunction (double dotProduct)
    {
        //TODO: Implementation.
        throw new System.NotImplementedException();
    }
}
