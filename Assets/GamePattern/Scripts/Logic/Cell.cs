using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

    private int indexHorse = -1;

    public int IndexHorse
    {
        get
        {
            return indexHorse;
        }

        set
        {
            indexHorse = value;
        }
    }

    public void RemoveHorse()
    {
        indexHorse = -1;
    }
}
