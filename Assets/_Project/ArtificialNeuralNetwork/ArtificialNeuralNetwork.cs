using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// in nested for loops of Go() and UpdateWeights().
public class ArtificialNeuralNetwork 
{
    private readonly int inputsCount;
    private readonly int outputsCount;
    private readonly int hiddenLayersCount;
    private readonly int neuronsPerHiddenLayer;
    // The learning rate. A percentage of the training set that is integrated each time it learns (weights are adjusted).
    // The higher the value, the quicker the NN will run, but it will also introduce issues.
    // Should be kept between 0 and 1.
    private readonly double alpha; 

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
        for (var i = 0; i < hiddenLayersCount + 1; i++)
        {
            // Layer is not input layer.
            if (i > 0)
            {
                // Takes the outputs from the previous layer.
                inputs = new List<double>(outputs);
            }
            outputs.Clear();

            // Loop through all neurons in the current layer.
            for (var j = 0; j < layers[i].NeuronsCount; j++)
            {
                double dotProduct = 0;
                layers[i].Neurons[j].Inputs.Clear();

                // Loop through all inputs of the current neuron.
                for (var k = 0; k < layers[i].Neurons[j].InputsCount; k++)
                {
                    // For each neuron's input, add in the input from the one before. 
                    layers[i].Neurons[j].Inputs.Add(inputs[k]);

                    // Dotproduct.
                    dotProduct += layers[i].Neurons[j].Weights[k] * inputs[k];
                }

                dotProduct -= layers[i].Neurons[j].Bias;

                // The current layer is the output layer.
                if (i == hiddenLayersCount)
                {
                    layers[i].Neurons[j].Output = ActivationFunctionOutputLayer(dotProduct);
                }
                else
                {
                    layers[i].Neurons[j].Output = ActivationFunctionHiddenLayers(dotProduct);
                }
                // Serves as the input for the next layer.
                outputs.Add(layers[i].Neurons[j].Output);
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
    private double ActivationFunctionHiddenLayers (double value)
    {
        // TODO: XOR Works with sigmoid in both places, but NOT with ReLu in hiddenlayer. >> ReLu might be the problem?
        return ReLu(value);
    }

    // A separate activation function used by the output layer.
    private double ActivationFunctionOutputLayer (double value)
    {
        return Sigmoid(value);
    }

    #region ACTIVATION FUNCTIONS
    // Aka. binary step.
    // Output values: 0 OR 1.
    private double Step (double value)
    {
        if (value < 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    // Aka. logistic softstep.
    // Output values: Between 0 and 1.
    private double Sigmoid (double value)
    {
        double k = System.Math.Exp(value);
        return k / (1f + k);
    }

    // Output values: Between -1 and 1.
    private double TanH (double value)
    {
        return (2 * (Sigmoid(2 * value)) - 1);
    } 

    // Output values: 0 OR (positive) value.
    private double ReLu (double value)
    {
        if (value > 0)
        {
            return value;
        }
        else
        {
            return 0;
        }
    }

    private double LeakyRelu (double value)
    {
        if (value < 0)
        {
            return 0.01 * value;
        }
        else
        {
            return value;
        }
    }

    private double Sinusoid (double value)
    {
        return System.Math.Sin(value);
    }

    private double ArcTan (double value)
    {
        return System.Math.Atan(value);
    }

    private double SoftSign (double value)
    {
        return value / (1 + System.Math.Abs(value));
    }
    #endregion
}
