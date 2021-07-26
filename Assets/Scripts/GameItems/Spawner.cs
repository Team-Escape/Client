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
        public GameItemControl item;
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

        public void GetItem()
        {
            if (item == null) return;
            item = null;
            this.AbleToDo(duration, () => RandomItem());
        }

        public void RandomItem()
        {
            List<int> box = new List<int>();
            foreach (var item in containers)
            {
                int index = 0;
                for (int i = 0; i < item.probability; i++)
                {
                    box.Add(index);
                }
                index++;
            }
            int rnd = Random.Range(0, box.Count);
            ItemContainer container = containers[rnd];

            GameObject go = Resources.Load("Game/Items/" + container.name) as GameObject;
            if (go == null)
                Debug.Log("Prefab not found or registered yet, make sure the name is correct and item prefab exists");
            else
                item = go.GetComponent<GameItemControl>();
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
