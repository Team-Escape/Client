using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagerSpace.Hall
{
    public class View : MonoBehaviour
    {
        public int GetMapLength { get { return mapContainers.Count; } }
        public string GetMapName(int id) { return mapContainers[id].name; }
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

        public void RoleContainerEffect(int id)
        {

        }

        public void MapContainerEffect(int id, bool isAdditive)
        {
            Image image = mapContainers[id].GetChild(0).GetChild(0).GetComponent<Image>();
            string name = "_ChromAberrAmount";
            float amount = 0.2f * (isAdditive ? 1 : -1);
            float currentAmount = image.materialForRendering.GetFloat(name);
            image.materialForRendering.SetFloat(
                name,
                currentAmount + amount
            );
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