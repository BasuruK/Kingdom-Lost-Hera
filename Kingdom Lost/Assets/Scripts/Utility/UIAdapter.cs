using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIAdapter : MonoBehaviour {
    public abstract void viewGameOver(int level, bool state);
    public abstract void changePoints(int value);
}
