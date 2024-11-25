using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GoalDetector : MonoBehaviour
{
    [SerializeField] private Player scriptPlayer;
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ball>() != null)
        {
            if (name.Equals("GoalDetector1"))
            {
                scriptPlayer.IncreaseMyScore();
            }
            else if (name.Equals("GoalDetector2"))
            {
                scriptPlayer.IncreaseOtherScore();
            }
            else if (name.Equals("LinhaDeFundo1"))
            {
                scriptPlayer.ResetarBola();
            }
            else if (name.Equals("LinhaDeFundo2"))
            {
                scriptPlayer.ResetarBola();
            }
        }
    }
}