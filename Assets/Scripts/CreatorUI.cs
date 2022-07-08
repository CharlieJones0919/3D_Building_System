using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem
{
    public class CreatorUI : MonoBehaviour
    {
        public void Initialize(PrimitiveType createType, float xPosition, Sprite buttonSprite, RectTransform hierContent, ShapeInstance shapeInstancePrefab, string UILabel = null)
        {
            type = createType;

            if (UILabel == null) { createLbl.text = type.ToString(); }
            else { createLbl.text = UILabel; }

            createBttn.image.sprite = buttonSprite;

            Vector3 bttnPos = Vector3.zero;
            bttnPos.x = xPosition;
            transform.localPosition += bttnPos;
        }

        public void MakeShapeAndSelector()
        {
            ShapeInstance newInstance = Instantiate(UIController.shapeInstPrefab, UIController.hierPanel.transform);
            UIController.AddToInstances(type, newInstance);
        }

        public PrimitiveType type { get; private set; }
        [SerializeField] private Text createLbl;
        [SerializeField] private Button createBttn;
    }
}