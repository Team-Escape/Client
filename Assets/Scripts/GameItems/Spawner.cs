using System.Linq;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PlayerSpace.Gameplayer
{
    public class Spawner : MonoBehaviour
    {
        public float duration = 10f;
        public int currentItemID = -1;
        public List<ItemContainer> containers = new List<ItemContainer>();
        [HideInInspector]
        public bool useHealthPotion = true;
        [HideInInspector]
        public bool useShrinkingPotion = true;

        #region Unity Native APIs
        private void OnEnable()
        {
            if (containers == null)
                RegisterItems();

            RandomItem();
        }
        #endregion

        public void ResetItem()
        {
            if (currentItemID == -1) return;
            currentItemID = -1;
            this.AbleToDo(duration, () => RandomItem());
        }

        public void RandomItem()
        {
            List<int> box = new List<int>();
            foreach (var item in containers)
            {
                int index = item.id;
                for (int i = 0; i < item.probability; i++)
                {
                    box.Add(index);
                }
            }
            currentItemID = box[Random.Range(0, box.Count)];
        }

        public void RegisterItems()
        {
            containers = new List<ItemContainer>();
            if (useHealthPotion)
                Register("HealthPotion", useHealthPotion);
            if (useShrinkingPotion)
                Register("ShrinkingPotion", useShrinkingPotion);
        }

        private void Register(string name, bool isActive)
        {
            int id = containers.Count;
            ItemContainer newItem = new ItemContainer();
            newItem.id = id;
            newItem.name = name;
            newItem.isActive = isActive;
            containers.Add(newItem);
        }
    }
    [System.Serializable]
    public class ItemContainer
    {
        // public GameItemControl self = null;
        public int id = 0;
        public bool isActive = true;
        public string name = "";
        public int probability = 1;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Spawner)), CanEditMultipleObjects]
    public class SpawnEditor : Editor
    {
        Spawner self;
        public SerializedProperty containersProp;
        public SerializedProperty useHealthPotionProp;
        public SerializedProperty useShrinkingPotionProp;

        private void OnEnable()
        {
            self = (Spawner)target;

            containersProp = serializedObject.FindProperty("containers");
            useHealthPotionProp = serializedObject.FindProperty("useHealthPotion");
            useShrinkingPotionProp = serializedObject.FindProperty("useShrinkingPotion");
        }
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Bools", GUILayout.Width(75));
            EditorGUILayout.PropertyField(useHealthPotionProp);
            EditorGUILayout.PropertyField(useShrinkingPotionProp);
            EditorGUILayout.Space();
            GUILayout.Label("Containers", GUILayout.Width(75));
            if (GUILayout.Button("Register items"))
            {
                self.RegisterItems();
            }
            EditorGUILayout.PropertyField(containersProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
