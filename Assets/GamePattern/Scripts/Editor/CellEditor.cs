using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections;
using System;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor {

    Cell cell;

    public override void OnInspectorGUI()
    {
        cell = (Cell)target;
        cell.name = (cell.transform.GetSiblingIndex() / Defs.BoardSize).ToString() + "_" + (cell.transform.GetSiblingIndex() % Defs.BoardSize).ToString();
    }
}
