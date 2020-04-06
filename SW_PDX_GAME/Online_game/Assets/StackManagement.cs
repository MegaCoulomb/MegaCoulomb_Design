using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GoFish
{
    public class StackManagement : MonoBehaviour
    {
        Text Stack;
        // Start is called before the first frame update
        void Start()
        {
            Stack = GetComponent<Text>();
            Stack.text = "$40";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
