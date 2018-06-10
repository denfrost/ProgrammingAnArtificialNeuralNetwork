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
    private readonly double alpha; // The learning rate. A percentage of the training set that is integrated each time it learns (weights are adjusted).

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

            // Loop through all neurons in the current layer.
            for (var currentNeuron = 0; currentNeuron < layers[currentLayer].NeuronsCount; currentNeuron++) //TODO: replace j with currentNeuron?
            {
                double dotProduct = 0;
                layers[currentLayer].Neurons[currentNeuron].Inputs.Clear();

                // Loop through all inputs of the current neuron.
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
        // The result of this iteration based on the given inputs.
        return outputs;
    }

    private void UpdateWeights (List<double> outputs, List<double> desiredOutputs)
    {
        // Error between NN's output and the desired output.
        double error;

        // Loop through all layers (input layer + hidden layers + output layer).
        // Back propagation: Looping backwards, taking the error at the end
        // and feeding it back up through the layers.
        for (var i = hiddenLayersCount; i >= 0; i--)
        {
            // Loop through all neurons in the current layer.
            for (var j = 0; j < layers[i].NeuronsCount; j++)
            {
                // If we are in the last (output) layer.
                if (i == hiddenLayersCount)
                {
                    // Calculate the error from the actual output of the NN.
                    error = desiredOutputs[j] - outputs[j];
                    // Error gradient calculated with Delta Rule: en.wikipedia.org/wiki/Delta_rule
                    // Error gradient expresses how much the respective neuron is responsible for the whole error.
                    // All error gradients sum up to the total error.
                    layers[i].Neurons[j].ErrorGradient = outputs[j] * (1 - outputs[j]) * error;
                }
                else
                {
                    layers[i].Neurons[j].ErrorGradient = layers[i].Neurons[j].Output * (1 - layers[i].Neurons[j].Output);
                    // Errors in the layer above the current layer.
                    double errorGradientSum = 0;

                    // Loop through the neurons in the layer after it.
                    for (var p = 0; p < layers[i + 1].NeuronsCount; p++)
                    {
                        // Add up those error gradients.
                        errorGradientSum += layers[i + 1].Neurons[p].ErrorGradient * layers[i + 1].Neurons[p].Weights[j];
                    }
                    // Add to the gradient of the current neuron.
                    layers[i].Neurons[j].ErrorGradient *= errorGradientSum; //TODO: Really *=?
                }

                // Loop through all inputs of the current neuron.
                for (var k = 0; k < layers[i].Neurons[j].InputsCount; k++)
                {
                    // If we are in the last (output) layer.
                    if (i == hiddenLayersCount)
                    {
                        error = desiredOutputs[j] - outputs[j];
                        layers[i].Neurons[j].Weights[k] += alpha * layers[i].Neurons[j].Inputs[k] * error;
                    }
                    else
                    {
                        layers[i].Neurons[j].Weights[k] += alpha * layers[i].Neurons[j].Inputs[k] * layers[i].Neurons[j].ErrorGradient; 
                    }
                }
                layers[i].Neurons[j].Bias += alpha * -1 * layers[i].Neurons[j].ErrorGradient;
            }

        }
    }

    // A container, which calls the actual activation function used.
    // For a full list of activation functions, see
    // en.wikipedia.org/wiki/Activation_function
    private double ActivationFunction (double dotProduct)
    {
        // Pass the call to the desired activation function.
        return Sigmoid(dotProduct);
    }

    // Activation function "Step" (aka. binary step).
    double Step (double dotProduct)
    {
        if (dotProduct < 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    // Activation function "Sigmoid" (aka. logistic softstep).
    double Sigmoid (double dotProduct)
    {
        double k = System.Math.Exp(dotProduct);
        return k / (1f + k);
    }
}
