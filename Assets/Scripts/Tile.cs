using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using VSCodeEditor;

public struct Cell
    {
        public enum Type
        {
            Invalid,
            Empty,
            Bomb,
            Number,
            Unknown,
        }
        public Type type;
        public Type secretTile;
        public Vector3Int position;
        public int number;
        public bool revealed;
        public bool flagged;
        public bool exploded;

    }
