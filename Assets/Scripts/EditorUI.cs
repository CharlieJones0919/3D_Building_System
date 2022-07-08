using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem
{
    public class EditorUI : MonoBehaviour
    {
        public InputField nameInput;
        public List<Vector3InputUI> vector3UI = new List<Vector3InputUI>(3); // Serializable list of the vector3 UI input elements (e.g. scale, position, etc). 

        public void Initialize(VectorInputFunction[] functions)
        {
            if (functions.Length != vector3UI.Count)
            {
                Debug.LogError(UIController.errorMsgs[1]);
                return;
            }

            for (int i = 0; i < vector3UI.Count; i++)
            {
                vector3UI[i].SetInputFunction(functions[i]); // Specifies the function in Builder to call at the end of input to set the values to the selected shape.
            }
        }

        public void SetSelectedValues()
        {
            nameInput.text = UIController.curSelection.GetLabelText();
            vector3UI[0].SetValues(UIController.curSelection.shape.transform.position);
            vector3UI[1].SetValues(UIController.curSelection.shape.transform.rotation.eulerAngles);
            vector3UI[2].SetValues(UIController.curSelection.shape.transform.localScale);
        }
    }
}