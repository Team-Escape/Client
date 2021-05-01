using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSpace.Hall
{
    public class View : MonoBehaviour
    {
        public int GetRolesLength { get { return roleContainers.Count; } }
        public string GetRoleName(int id) { return roleContainers[id].name; }
        [SerializeField] Transform roleUI = null;
        List<Transform> roleContainers = new List<Transform>();
        [SerializeField] Transform mapUI = null;
        List<Transform> mapContainers = new List<Transform>();

        public void UpdateRoleContainer(int id, int index, bool isActive = true)
        {
            roleContainers[index].GetChild(id + 1).gameObject.SetActive(isActive);
        }
        public void UpdateMapContainer(int id, int index, bool isActive = true)
        {
            mapContainers[index].GetChild(id + 1).gameObject.SetActive(isActive);
        }

        private void Awake()
        {
            foreach (Transform go in roleUI)
            {
                roleContainers.Add(go);
            }
            foreach (Transform go in mapUI)
            {
                mapContainers.Add(go);
            }
        }
    }
}