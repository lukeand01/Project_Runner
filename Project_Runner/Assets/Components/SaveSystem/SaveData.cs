using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SaveData : ScriptableObject
{
    //just hold data.

    public float highestScore;
    public bool alreadyPlayed;

    public void SetHighScore(float highestScore) => this.highestScore = highestScore;

}
