using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BuildingSystem
{
    public static class UIController
    {
        private const int listCapDefault = 50;

        public static ShapeInstance shapeInstPrefab;
        public static Material shapeDefaultMat;

        public static RectTransform hierPanel; // Where the hierarchy UI field's selector labels/buttons are stored.
        private static float selectBttnHeight; // Keeps a record of the hierarchy UI element's content RectTransform's height on Awake as a basis of hierarchy element spacing.

        private static Dictionary<PrimitiveType, List<ShapeInstance>> shapeInstances;
        private static List<ShapeInstance> allCreated = new List<ShapeInstance>(listCapDefault);

        public static ShapeInstance curSelection { get; private set; }


        // Just for debugging purposes to keep things neat. Didn't utilize it as much as expected.
        public static string[] errorMsgs =
        {
       /* 0 */ "No supported shapes icon spritesheet found in Resources folder specified path: {0}",
       /* 1 */ "The same number of functions have not been set to the Vector3 editor UI elements.",
        };

        public static void Initialize(PrimitiveType[] supportedShapes, RectTransform hierarchyContent, ShapeInstance shapePrefab, Material defaultMat)
        {
            shapeInstances = new Dictionary<PrimitiveType, List<ShapeInstance>>(supportedShapes.Length);

            foreach (PrimitiveType shapeType in supportedShapes)
            {
                shapeInstances.Add(shapeType, new List<ShapeInstance>(listCapDefault));
            }

            hierPanel = hierarchyContent;
            selectBttnHeight = hierPanel.rect.height;
            shapeInstPrefab = shapePrefab;
            shapeDefaultMat = defaultMat;
        }

        // Gets the next avilable label number (e.g. "Cube[2]"). Keeps the labels in order after a deletion.
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
            hierPanel.sizeDelta += new Vector2(0, selectBttnHeight); // The size of the Hierarchy Content's RectTransform needs to be scaled with the list items for the scroll wheel.
        }

        public static void SelectShape(ShapeInstance instance) => curSelection = instance;
        public static void DeselectShape() => curSelection = null;

        public static void DeleteShape()
        {
            shapeInstances[curSelection.type].Remove(curSelection);
            allCreated.Remove(curSelection);

            for (int i = curSelection.totalID; i < allCreated.Count; i++)
            {
                allCreated[i].totalID--;
                allCreated[i].MoveHierarchyPosition(selectBttnHeight); // Moves the selection label/button in the Hierarchy UI upwards.
            }

            hierPanel.sizeDelta -= new Vector2(0, selectBttnHeight); 
           
            curSelection.Destroy();
            DeselectShape();
        }
    }
}