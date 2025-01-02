using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DS
{
    public class SoulCountBar : MonoBehaviour
    {
        public TextMeshProUGUI soulCountText;

        public void SetSoulCountText(int soulCount)
        {
            soulCountText.text = soulCount.ToString();
        }
    }
}
