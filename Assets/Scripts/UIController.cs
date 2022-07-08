using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BuildingSystem
{
    public static class UIController
    {
        private const int listCapDefault = 50;

        public static RectTransform hierPanel;
       // public static EditorUI editPanel;
        //private static RectTransform createPanel;

        public static ShapeInstance shapeInstPrefab;
        private static float selectBttnHeight;

        private static Dictionary<PrimitiveType, List<ShapeInstance>> shapeInstances;
        private static List<ShapeInstance> allCreated = new List<ShapeInstance>(listCapDefault);
        public static ShapeInstance curSelection { get; private set; }
        public static bool IsSelection => curSelection != null;

        public static string[] errorMsgs =
        {
       /* 0 */ "No supported shapes icon spritesheet found in Resources folder specified path: {0}",
       /* 1 */ "The same number of functions have not been set to the Vector3 editor UI elements.",
        };

        public static void Initialize(PrimitiveType[] supportedShapes, RectTransform hierarchyContent, RectTransform createUI, EditorUI editUI, ShapeInstance shapePrefab)
        {
            shapeInstances = new Dictionary<PrimitiveType, List<ShapeInstance>>(supportedShapes.Length);

            foreach (PrimitiveType shapeType in supportedShapes)
            {
                shapeInstances.Add(shapeType, new List<ShapeInstance>(listCapDefault));
            }

            hierPanel = hierarchyContent;
            selectBttnHeight = hierPanel.rect.height;
            shapeInstPrefab = shapePrefab;

            //editPanel = editUI;
            //createPanel = createUI;
        }

        public static int GetAvailableTypeIDNum(PrimitiveType type)
        {
            IEnumerable<ShapeInstance> orderedInstances = shapeInstances[type].OrderBy(x => x.typeID);

            if (shapeInstances[type].Count == 0) { return 0; }

            for (int avaiableNum = 0; avaiableNum < orderedInstances.Count(); avaiableNum++)
            {
                if (orderedInstances.ElementAt(avaiableNum).typeID != avaiableNum)
                {
                    return avaiableNum;
                }
            }

            return shapeInstances[type].Count;
        }

        public static void AddToInstances(PrimitiveType type, ShapeInstance newInstance)
        {
            newInstance.totalID = allCreated.Count;
            newInstance.Initialize(type);

            shapeInstances[type].Add(newInstance);
            allCreated.Add(newInstance);

            newInstance.MoveHierarchyPosition(selectBttnHeight);
            hierPanel.sizeDelta += new Vector2(0, selectBttnHeight);
        }

        //public static void NameSelection()
        //{
        //    if (!string.IsNullOrEmpty(editPanel.nameInput.text) && !string.IsNullOrWhiteSpace(editPanel.nameInput.text))
        //    {
        //        curSelection.SetLabelText(editPanel.nameInput.text);
        //    }

        //    SelectShape(curSelection);
        //}

        //public static void SetSelectionToUI()
        //{
        //    editPanel.SetValues(curSelection.GetLabelText(), curSelection.shape.transform.position, curSelection.shape.transform.rotation.eulerAngles);
        //}

        public static void SelectShape(ShapeInstance instance)
        {
           // SwapPanels(createPanel.gameObject, editPanel.gameObject);
            curSelection = instance;
            //SetSelectionToUI();
        }

        public static void DeselectSelection()
        {
            curSelection = null;
            //SwapPanels(editPanel.gameObject, createPanel.gameObject);
        }

        public static void DeleteSelection()
        {
            shapeInstances[curSelection.type].Remove(curSelection);
            allCreated.Remove(curSelection);

            for (int i = curSelection.totalID; i < allCreated.Count; i++)
            {
                allCreated[i].totalID--;
                allCreated[i].MoveHierarchyPosition(selectBttnHeight);
            }

            hierPanel.sizeDelta -= new Vector2(0, selectBttnHeight);
           
            curSelection.Destroy();
            DeselectSelection();
        }
    }
}