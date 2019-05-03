using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class SideAI : MonoBehaviour
    {
        public abstract Core.Side Side { get; protected set; }
        public List<GameObject> Children { get; protected set; } = new List<GameObject>();
        public List<GameObject> VisibleEnemies { get; protected set; } = new List<GameObject>();

        public void UpdateChildrenList()
        {
            foreach (Transform childTransform in transform) {
                GameObject child = childTransform.gameObject;
                if (!Children.Contains(child)) {
                    Children.Add(child.gameObject);
                }
            }
            List<GameObject> childrenToBeLeft = new List<GameObject>();
            foreach (GameObject child in Children) {
                if (child != null) {
                    childrenToBeLeft.Add(child);
                }
            }
            Children = childrenToBeLeft;
        }

        public void CheckVisibility(List<GameObject> checkAgainst)
        {
            List<GameObject> updatedVisibleEnemies = new List<GameObject>();
            foreach (GameObject child in Children) {
                //Debug.Log(child.transform.position + ", " + Side);
                child.GetComponent<OperatorAI>().CheckVisibility(checkAgainst);
                List<GameObject> visibleToChild = child.GetComponent<OperatorAI>().VisibleEnemies;
                //Debug.Log(child.GetComponent<OperatorAI>().VisibleEnemies.Count + ", " + Side);
                foreach (GameObject enemy in visibleToChild) {
                    if (!updatedVisibleEnemies.Contains(enemy)) {
                        updatedVisibleEnemies.Add(enemy);
                    }
                }
            }
            VisibleEnemies = updatedVisibleEnemies;
        }
    }
}

