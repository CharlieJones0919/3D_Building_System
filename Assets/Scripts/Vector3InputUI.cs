using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem
{
    public delegate void VectorInputFunction();

    public class Vector3InputUI : MonoBehaviour
    {
        [SerializeField] private InputField[] inputFields;

        public void SetInputFunction(VectorInputFunction func) 
        {
            foreach (InputField element in inputFields)
            {
                element.onEndEdit.AddListener(delegate { func(); });
            }
        }

        public void SetValues(Vector3 value)
        {
            for (int i = 0; i < 3; i++) 
            {
                inputFields[i].text = value[i].ToString();
            }
        }

        private List<float> GetValues()
        {
            List<float> values = new List<float>(inputFields.Length);

            foreach (InputField element in inputFields)
            {
                if (string.IsNullOrWhiteSpace(element.textComponent.text) || string.IsNullOrEmpty(element.textComponent.text))
                {
                    values.Add(0.0f);
                }
                else
                {
                    values.Add(float.Parse(element.textComponent.text));
                }       
            }
            return values;
        }

        public Vector3 GetVec3()
        {
            List<float> values = GetValues();

            Vector3 vValue = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                vValue[i] = values[i];
            }
            return vValue;
        }
    }
}