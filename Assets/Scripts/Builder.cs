using UnityEngine;
using System;

namespace BuildingSystem
{
    [Serializable]
    public class Builder : MonoBehaviour
    {
        [SerializeField] private float cameraMoveSpeed = 3;
        [SerializeField] private float cameraRotateSpeed = 10;
        private Vector3 initialCamPos;

        [SerializeField] private RectTransform createPanel;
        [SerializeField] private RectTransform hierarchyPanel;
        [SerializeField] private EditorUI editPanel;

        [SerializeField] private CreatorUI creatorUIPrefab;
        [SerializeField] private Material defaultShapeMat;
        [SerializeField] private string shapeSpritesheetPath = "UI Icons/CreatorShapeSprites";
        [SerializeField] private ShapeInstance shapeInstancePrefab;


        private static PrimitiveType[] supportedShapes = (PrimitiveType[])Enum.GetValues(typeof(PrimitiveType));

        // ^ An alternative in case custom shapes other than the primitive shapes are desired.
        /*
        private enum SupportedShapes { Capsule, Cube, Cylinder, Plane, Quad, Sphere }
        private static SupportedShapes[] supportedShapes = (SupportedShapes[])Enum.GetValues(typeof(SupportedShapes));
        */

        private void Awake()
        {
            initialCamPos = Camera.main.transform.position;
            UIController.Initialize(supportedShapes, hierarchyPanel, shapeInstancePrefab, defaultShapeMat);
            editPanel.Initialize(new VectorInputFunction[] { TransformSelection, RotateSelection, ScaleSelection });

            if (shapeSpritesheetPath != null)
            {
                Sprite[] shapeSprites = Resources.LoadAll<Sprite>(shapeSpritesheetPath);

                if (shapeSprites.Length != 0)
                {
                    foreach (Sprite sprite in shapeSprites)
                    {
                        int creatorNum = 0;
                        foreach (PrimitiveType type in supportedShapes)
                        {
                            if (type.ToString() == sprite.name)
                            {
                                CreatorUI newCreatorUI = Instantiate(creatorUIPrefab, createPanel.transform);
                                newCreatorUI.Initialize(type, createPanel.rect.height * creatorNum, sprite);
                                break;
                            }
                            creatorNum++;
                        }
                    }

                    return;
                }
                Debug.LogErrorFormat(UIController.errorMsgs[0], shapeSpritesheetPath);
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }

            if (UIController.curSelection != null)
            {
                if (createPanel.gameObject.activeSelf) { CloseCreatorPanel(); SetSelectionToUI(); }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    TransformSelection(GetInputMovement(UIController.curSelection.shape.transform));
                    RotateSelection(GetInputRotation(UIController.curSelection.shape.transform));
                    ScaleSelection(GetInputScale(UIController.curSelection.shape.transform));
                }
            }
            else
            {
                if (!createPanel.gameObject.activeSelf) { OpenCreatorPanel(); }

                if (Input.anyKey)
                {
                    TransformSelection(Camera.main.gameObject, GetInputMovement(Camera.main.transform));
                    RotateSelection(Camera.main.gameObject, GetInputRotation(Camera.main.transform));
                }
            }
        }

        private Vector3 GetInputMovement(Transform subject)
        {
            Vector3 newPos = subject.transform.position;

            if (Input.GetKey(KeyCode.Tab)) // Up & Down
            {
                if (Input.GetKey(KeyCode.W)) { newPos += subject.transform.up * cameraMoveSpeed * Time.deltaTime; }
                if (Input.GetKey(KeyCode.S)) { newPos -= subject.transform.up * cameraMoveSpeed * Time.deltaTime; }
            }
            else // Forward & Back
            {
                if (Input.GetKey(KeyCode.W)) { newPos += subject.transform.forward * cameraMoveSpeed * Time.deltaTime; }
                if (Input.GetKey(KeyCode.S)) { newPos -= subject.transform.forward * cameraMoveSpeed * Time.deltaTime; }
            }

            // Left & Right
            if (Input.GetKey(KeyCode.A)) { newPos -= subject.transform.right * cameraMoveSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.D)) { newPos += subject.transform.right * cameraMoveSpeed * Time.deltaTime; }

            return newPos;
        }

        private Vector3 GetInputRotation(Transform subject)
        {
            Vector3 newRot = subject.transform.eulerAngles;

            if (Input.GetKey(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.Q)) { newRot -= subject.transform.forward * cameraRotateSpeed * Time.deltaTime; }
                if (Input.GetKey(KeyCode.E)) { newRot += subject.transform.forward * cameraRotateSpeed * Time.deltaTime; }
            }
            else if (Input.GetKey(KeyCode.CapsLock))
            {
                if (Input.GetKey(KeyCode.Q)) { newRot -= subject.transform.right * cameraRotateSpeed * Time.deltaTime; }
                if (Input.GetKey(KeyCode.E)) { newRot += subject.transform.right * cameraRotateSpeed * Time.deltaTime; }
            }
            else
            {
                if (Input.GetKey(KeyCode.Q)) { newRot -= subject.transform.up * cameraRotateSpeed * Time.deltaTime; }
                if (Input.GetKey(KeyCode.E)) { newRot += subject.transform.up * cameraRotateSpeed * Time.deltaTime; }
            }

            return newRot;
        }

        public Vector3 GetInputScale(Transform subject)
        {
            Vector3 newScale = subject.transform.localScale;

            if (Input.GetKey(KeyCode.X))
            {
                if (Input.GetKey(KeyCode.UpArrow)) { newScale.x += Time.deltaTime; }
                if (Input.GetKey(KeyCode.DownArrow)) { newScale.x -= Time.deltaTime; }
            }

            if (Input.GetKey(KeyCode.Y))
            {
                if (Input.GetKey(KeyCode.UpArrow)) { newScale.y += Time.deltaTime; }
                if (Input.GetKey(KeyCode.DownArrow)) { newScale.y -= Time.deltaTime; }
            }

            if (Input.GetKey(KeyCode.Z))
            {
                if (Input.GetKey(KeyCode.UpArrow)) { newScale.z += Time.deltaTime; }
                if (Input.GetKey(KeyCode.DownArrow)) { newScale.z -= Time.deltaTime; }
            }

            return newScale;
        }

        public void SetSelectionToUI()
        {
            editPanel.SetSelectedValues();
        }

        public void NameSelected()
        {
            if (!string.IsNullOrEmpty(editPanel.nameInput.text) && !string.IsNullOrWhiteSpace(editPanel.nameInput.text))
            {
                UIController.curSelection.SetLabelText(editPanel.nameInput.text);
            }
            SetSelectionToUI();
        }

        public void TransformSelection()
        {
            UIController.curSelection.shape.transform.position = editPanel.vector3UI[0].GetVec3(); ;
            SetSelectionToUI();
        }

        public void TransformSelection(Vector3 newPos)
        {
            UIController.curSelection.shape.transform.position = newPos;
            SetSelectionToUI();
        }

        public void TransformSelection(GameObject otherSubject, Vector3 newPos)
        {
            otherSubject.transform.position = newPos;
        }

        public void RotateSelection()
        {
            UIController.curSelection.shape.transform.eulerAngles = editPanel.vector3UI[1].GetVec3();
            SetSelectionToUI();
        }

        public void RotateSelection(Vector3 newRot)
        {
            UIController.curSelection.shape.transform.eulerAngles = newRot;
            SetSelectionToUI();
        }

        public void RotateSelection(GameObject otherSubject, Vector3 newRot)
        {
            otherSubject.transform.eulerAngles = newRot;
        }

        public void ScaleSelection()
        {
            UIController.curSelection.shape.transform.localScale = editPanel.vector3UI[2].GetVec3();
            SetSelectionToUI();
        }

        public void ScaleSelection(Vector3 newScale)
        {
            UIController.curSelection.shape.transform.localScale = newScale;
            SetSelectionToUI();
        }

        public void DeselectSelected()
        {
            UIController.DeselectShape();
            OpenCreatorPanel();
        }

        public void DeleteSelected()
        {
            UIController.DeleteShape();
            OpenCreatorPanel();
        }

        private void OpenCreatorPanel() => SwapPanels(editPanel.gameObject, createPanel.gameObject);
        private void CloseCreatorPanel() => SwapPanels(createPanel.gameObject, editPanel.gameObject);

        private void SwapPanels(GameObject panelA, GameObject panelB)
        {
            panelB.SetActive(true);
            panelA.SetActive(false);
        }

        public void ResetCamera()
        {
            Camera.main.transform.position = initialCamPos;
            Camera.main.transform.eulerAngles = Vector3.zero;
        }
    }
}