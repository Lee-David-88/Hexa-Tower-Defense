using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGridDrawer))]
public class HexCoordinatesDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        HexGridDrawer map = target as HexGridDrawer;

        if (DrawDefaultInspector())
        {
            map.DrawHexCoordinateInWorldPosition();
        }

        if (GUILayout.Button("Generate Map"))
        {
            map.DrawHexCoordinateInWorldPosition();
        }
    }
}
