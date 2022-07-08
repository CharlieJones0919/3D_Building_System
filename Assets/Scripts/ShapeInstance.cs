using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem
{
    public class ShapeInstance : MonoBehaviour
    {
        public void Initialize(PrimitiveType shapeType)
        {
            type = shapeType;
            typeID = UIController.GetAvailableTypeIDNum(type);
            SetLabelText(string.Format("{0} [{1}]", type.ToString(), typeID));
            shape = GameObject.CreatePrimitive(type);
        }

        public string GetLabelText() => label.text;

        public void MoveHierarchyPosition(float heightOffset)
        {
            Vector3 selectorPos = Vector3.zero;
            selectorPos.y -= heightOffset * totalID;
            selector.gameObject.transform.localPosition = selectorPos;
        }

        public void SetLabelText(string newLabel)
        {
            label.text = newLabel;
        }

        public void SelectShape()
        {
            UIController.SelectShape(this);
        }

        //public void RotateShape(Vector3 newRot)
        //{
        //    shape.transform.eulerAngles = newRot;
        //}

        //public void TransformShape(Vector3 newPos)
        //{
        //    shape.transform.position = newPos;
        //}

        public void Destroy()
        {
            DestroyImmediate(shape);
            DestroyImmediate(selector);
            DestroyImmediate(gameObject);
        }

        public PrimitiveType type { get; private set; }
        public int typeID { get; private set; }
        public int totalID;
        public GameObject shape { get; private set; }
        [SerializeField] private Button selector;
        [SerializeField] private Text label;
    }
}