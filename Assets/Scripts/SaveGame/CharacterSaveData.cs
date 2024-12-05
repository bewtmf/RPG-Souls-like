using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("World Position")]
        public float xPosition;
        public float yPosition;
        public float zPosition;
    }
}
