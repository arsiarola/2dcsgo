using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI
{
    public abstract class OperatorAI : AI
    {
        public override List<GameObject> VisibleEnemies { get; protected set; } = new List<GameObject>();

        public void CheckVisibility(List<GameObject> checkAgainst)
        {
            List<GameObject> updatedVisibleEnemies = new List<GameObject>();
            Vector3 thisPos = gameObject.transform.position;
            //Debug.Log(thisPos + ", " + Side);
            foreach (GameObject check in checkAgainst) {
                Vector3 checkPos = check.transform.position;
                //Debug.Log(checkPos + ", " + Side);
                Vector3 direction = checkPos - thisPos;
                float distance = direction.magnitude;
                RaycastHit2D cast = Physics2D.Raycast(thisPos, direction, distance, 1 << 8);
                Debug.DrawLine(thisPos, checkPos, Color.red, 2);
                if (cast.collider == null) {
                    updatedVisibleEnemies.Add(check);
                }
            }
            VisibleEnemies = updatedVisibleEnemies;
        }
    }
}