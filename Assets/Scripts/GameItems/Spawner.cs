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
        public List<ItemContainer> containers = new List<ItemContainer>();
        public ItemContainer currentItem = null;
        [HideInInspector]
        public bool useHealthPotion = true;
        [HideInInspector]
        public bool useShrinkingPotion = true;

        #region Unity Native APIs
        private void OnEnable()
        {
            RegisterItems();
        }
        #endregion

        public void RandomItem()
        {

        }

        public void RegisterItems()
        {
            containers = new List<ItemContainer>();
            Register(new HealthPotion(), "HealthPostion", useHealthPotion);
            Register(new ShrinkingPotion(), "ShrinkingPostion", useShrinkingPotion);
        }

        private void Register(GameItemControl item, string name, bool isActive)
        {
            int id = containers.Count;
            ItemContainer newItem = new ItemContainer();
            newItem.id = id;
            newItem.self = item;
            newItem.itemName = name;
            newItem.isActive = isActive;
            containers.Add(newItem);
        }
    }
    [System.Serializable]
    public class ItemContainer
    {
        public int id = 0;
        public GameItemControl self = null;
        public bool isActive = true;
        public string itemName = "";
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
