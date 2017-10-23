using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyListener : MonoBehaviour {
    // check opponent distance
    abstract public void checkOpponentDistance();
    // attack options
    abstract public void doAtack();
    // change health
    abstract public void changeHealth(float distance);
    // display health bar
    abstract public void displayHealthBar();
}
