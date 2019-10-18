using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LifeCounterDisplay : MonoBehaviour
{

    public ConwaysGameOfLife gameOfLife = null;
    private const string ActiveLifeLabel = "Active Life";
    private Text text { get { return gameObject.GetComponent<Text>(); } }

    // Update is called once per frame
    void Update()
    {
        if (gameOfLife != null)
        {
            text.text = "Initial Population: " + gameOfLife.InitialPopulationCount + "\n"
                + "Current Population: " + gameOfLife.CurrentPopulationCount + "\n"
                + "Life to be Born: " + gameOfLife.LifeToBeBorn + "\n"
                + "Life to die: " + gameOfLife.LifeToDieCount;
        }
    }
}
