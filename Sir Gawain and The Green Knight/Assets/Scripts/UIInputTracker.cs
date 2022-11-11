using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputTracker : MonoBehaviour
{
    public Text text;

    private List<KeyCode> inputs;

    public KeyCode[] posibleImputs = { KeyCode.Q , KeyCode.E, KeyCode.F, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };

    // Start is called before the first frame update
    void Awake()
    {
        inputs = new List<KeyCode>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectInputs();
        text.text = InputsString();
        
    }

    private void DetectInputs()
    {
        foreach(KeyCode k in posibleImputs)
        {
            if (Input.GetKeyDown(k))
            {
                if (!inputs.Contains(k))
                {
                    inputs.Add(k);
                }
            }
            if (Input.GetKeyUp(k))
            {
                inputs.Remove(k);
            }
        }
    }

    private string InputsString()
    {
        string s = "";

        foreach(KeyCode k in inputs)
        {
            s += k.ToString() + " ";
        }

        return s;
    }
}
