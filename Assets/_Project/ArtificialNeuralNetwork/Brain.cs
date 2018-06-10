using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates and trains the NN.
public class Brain : MonoBehaviour 
{
    // Statistic value to compare how closely the constructed model fits the data.
    private double sumSquareError = 0;

    private ArtificialNeuralNetwork artificialNeuralNetwork;

	// Use this for initialization
	private void Start () 
	{
        artificialNeuralNetwork = new ArtificialNeuralNetwork(2, 1, 1, 2, 0.8);

        List<double> results;

        var epochs = 5000;
        var desiredOutput = 0;

        for (var i = 0; i < epochs; i++)
        {
            sumSquareError = 0;

            // Training set for an XOR operator.
            desiredOutput = 0;
            results = Train(1, 1, desiredOutput);
            sumSquareError += Mathf.Pow((float)results[0] - desiredOutput, 2);

            desiredOutput = 1;
            results = Train(1, 0, desiredOutput);
            sumSquareError += Mathf.Pow((float)results[0] - desiredOutput, 2);

            desiredOutput = 1;
            results = Train(0, 1, desiredOutput);
            sumSquareError += Mathf.Pow((float)results[0] - desiredOutput, 2);

            desiredOutput = 0;
            results = Train(0, 0, desiredOutput);
            sumSquareError += Mathf.Pow((float)results[0] - desiredOutput, 2);

            // Training set for an XNOR operator:
            // Switch all desiredOutput values of the XOR training set
            // from 1 to 0 and from 0 to 1, respectively.
        }
        Debug.LogFormat("SumSquareError: {0}", sumSquareError);

        // Run training set again.
        // Side-effect: updates weights as well inside GO().

        double input1 = 1;
        double input2 = 1;
        desiredOutput = 0;
        results = Train(input1, input2, desiredOutput);
        Debug.LogFormat("{0}, {1}, {2} (Input values)", input1, input2, desiredOutput);
        Debug.LogFormat("{0}, {1}, {2} (Output value rounded)\n", input1, input2, Mathf.Round((float)results[0]));
        Debug.LogFormat("{0}, {1}, {2} (Output value)", input1, input2, results[0]);

        input1 = 1;
        input2 = 0;
        desiredOutput = 1;
        results = Train(input1, input2, desiredOutput);
        Debug.LogFormat("{0}, {1}, {2} (Input values)", input1, input2, desiredOutput);
        Debug.LogFormat("{0}, {1}, {2} (Output value rounded)\n", input1, input2, Mathf.Round((float)results[0]));
        Debug.LogFormat("{0}, {1}, {2} (Output value)", input1, input2, results[0]);

        input1 = 0;
        input2 = 1;
        desiredOutput = 1;
        results = Train(input1, input2, desiredOutput);
        Debug.LogFormat("{0}, {1}, {2} (Input values)", input1, input2, desiredOutput);
        Debug.LogFormat("{0}, {1}, {2} (Output value rounded)\n", input1, input2, Mathf.Round((float)results[0]));
        Debug.LogFormat("{0}, {1}, {2} (Output value)", input1, input2, results[0]);

        input1 = 0;
        input2 = 0;
        desiredOutput = 0;
        results = Train(input1, input2, desiredOutput);
        Debug.LogFormat("{0}, {1}, {2} (Input values)", input1, input2, desiredOutput);
        Debug.LogFormat("{0}, {1}, {2} (Output value rounded)\n", input1, input2, Mathf.Round((float)results[0]));
        Debug.LogFormat("{0}, {1}, {2} (Output value)", input1, input2, results[0]);
    }

    private List<double> Train(double input1, double input2, double desiredOutput)
    {
        var inputs = new List<double>();
        var desiredOutputs = new List<double>();

        inputs.Add(input1);
        inputs.Add(input2);

        desiredOutputs.Add(desiredOutput);

        return (artificialNeuralNetwork.Go(inputs, desiredOutputs));
    }
}
