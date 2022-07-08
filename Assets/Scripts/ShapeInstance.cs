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
            label.text = string.Format("{0} [{1}]", type.ToString(), typeID);

            shape = GameObject.CreatePrimitive(type);
            shape.GetComponent<Renderer>().material = UIController.shapeDefaultMat;
        }

        public void MoveHierarchyPosition(float heightOffset)
        {
            Vector3 selectorPos = Vector3.zero;
            selectorPos.y -= heightOffset * totalID;
            selector.gameObject.transform.localPosition = selectorPos;
        }

        public void SelectShape()
        {
            UIController.SelectShape(this);
        }

        public void Destroy()
        {
            DestroyImmediate(shape);
            DestroyImmediate(selector);
            DestroyImmediate(gameObject);
        }

        public GameObject shape { get; private set; }
        public PrimitiveType type { get; private set; }
        public int typeID { get; private set; }
        public int totalID;
  
        [SerializeField] private Button selector;
        public Text label;
    }
}