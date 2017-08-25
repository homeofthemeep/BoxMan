using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    [Range(0, 6)]
    public int axisCount;
    [Range(0, 20)]
    public int buttonCount;


    public Controller controller;

    public void PassInput(ref InputData data)
    {
        //Debug.Log("Movement: " + data.axes[0] + ", " + data.axes[1]);
        controller.ReadInput(ref data);
    }

    private void Update()
    {
        //System.Threading.Thread.SpinWait(10000);
    }
}

public struct InputData
{
    public float[] axes;
    public bool[] buttons;

    public InputData(int axisCount, int buttonCount)
    {
        axes = new float[axisCount];
        buttons = new bool[buttonCount];
    }

    public void Reset()
    {   //Sets the axes adn buttons to default state

        for (int i = 0; i < axes.Length; i++)
        {
            axes[i] = 0f;
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = false;
        }

    }
}