using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem
{
    public class CreatorUI : MonoBehaviour
    {
        public void Initialize(PrimitiveType createType, float xPosition, Sprite buttonSprite)
        {
            type = createType;
            createLbl.text = type.ToString(); 

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