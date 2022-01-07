using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    public class ColorChanger : MonoBehaviour
    {
        [SerializeField]
        private Color color;

        private Renderer render;
        // Start is called before the first frame update
        void Start()
        {
            render = GetComponent<Renderer>();
            render.material.color = color;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}