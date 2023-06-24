using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int LeftSideScore = 0;
    private int RightSideScore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Left Side: {LeftSideScore}, Right Side: {RightSideScore}");
    }

    public void OnScore(string side)
    {
        switch (side)
        {
            case "Left Trigger":
                RightSideScore += 1;
                break;
            case "Right Trigger":
                LeftSideScore += 1;
                break;
        }
    }
}
