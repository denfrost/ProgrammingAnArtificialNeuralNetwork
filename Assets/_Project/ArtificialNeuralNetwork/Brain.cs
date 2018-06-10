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

        var epochs = 1000;

        for (var i = 0; i < epochs; i++)
        {
            // Training set for an XOR operator.
            //TODO: implement Train() and copy training set.
            sumSquareError = 0;

            results = Train(1, 1, 0);
            sumSquareError += Mathf.Pow((float)results[0] - 0, 2);

            results = Train(1, 0, 0);
            sumSquareError += Mathf.Pow((float)results[0] - 1, 2);

            results = Train(0, 1, 1);
            sumSquareError += Mathf.Pow((float)results[0] - 1, 2);

            results = Train(0, 0, 0);
            sumSquareError += Mathf.Pow((float)results[0] - 0, 2);
        }
        Debug.LogFormat("SumSquareError: {0}", sumSquareError);

        // Run training set again.
        // Side-effect: updates weights as well inside GO().
        results = Train(1, 1, 0);
        Debug.LogFormat(" 1 1 {0}", results[0]);

        results = Train(1, 0, 1);
        Debug.LogFormat(" 1 0 {0}", results[0]);

        results = Train(0, 1, 1);
        Debug.LogFormat(" 0 1 {0}", results[0]);

        results = Train(0, 0, 0);
        Debug.LogFormat(" 0 0 {0}", results[0]);
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
