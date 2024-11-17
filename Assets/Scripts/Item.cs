using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS 
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;
    }
}

