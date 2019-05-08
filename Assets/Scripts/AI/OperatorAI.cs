using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI
{
    public abstract class OperatorAI : AI
    {
        public List<Vector3> Path { get; protected set; } = new List<Vector3>();
        public int NextPointInPath { get; private set; } = 1;
        private float slerp = 0;
        private float rotationSpeed = 0.025f;
        public Quaternion Rotation { get; protected set; } = new Quaternion();
        private GameObject Target { get; set; } = null;
        private Quaternion lastQuaternion = new Quaternion();
        private GameObject lastTarget = null;

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

        public void SetPath(List<Vector3> list)
        {
            Path = list;
            NextPointInPath = 1;
        }

        public void SetRotation(Quaternion q)
        {
            Rotation = q;
            slerp = 0;
        }

        public void FollowPath()
        {
            if (NextPointInPath < Path.Count - 1) {
                float speed = 3.6125f * Time.fixedDeltaTime;
                float distance = Vector3.Distance(transform.position, Path[NextPointInPath]);
                while (distance < speed && NextPointInPath + 1 < Path.Count) {
                    NextPointInPath++;
                    distance = Vector3.Distance(transform.position, Path[NextPointInPath]);
                }
                float t = speed / distance;
                Vector3 pathDirection = Path[NextPointInPath] - transform.position;
                float angle = Vector3.SignedAngle(pathDirection, transform.up, transform.forward);
                transform.position = Vector3.Lerp(transform.position, Path[NextPointInPath], t);

                GetComponent<Animator>().SetBool("Moving", true);
                if (-135 < angle && angle < -45) {
                    GetComponent<Animator>().SetTrigger("StrafeLeft");
                }
                else if (45 < angle && angle < 135) {
                    GetComponent<Animator>().SetTrigger("StrafeRight");
                }
                else {
                    GetComponent<Animator>().SetTrigger("Forward");
                }
            } else {
                GetComponent<Animator>().SetTrigger("Idle");
                GetComponent<Animator>().SetBool("Moving", false);
            }
            
        }

        public void FindTarget()
        {
            if (VisibleEnemies.Contains(Target)) {
                // do nothing
            }
            else if (VisibleEnemies.Count > 0) {
                Vector3 thisPos = gameObject.transform.position;
                Target = VisibleEnemies[0];
                float shortestDistance = Vector3.Distance(thisPos, VisibleEnemies[0].transform.position);
                for (int i = 1; i < VisibleEnemies.Count; i++) {
                    Vector3 enemyPos = VisibleEnemies[i].transform.position;
                    if (Vector3.Distance(thisPos, enemyPos) < shortestDistance) {
                        Target = VisibleEnemies[i];
                    }
                }
            } else {
                Target = null;
            }
        }

        private void RotateTowards(Quaternion q)
        {
            if ((q != lastQuaternion && Target == null) || (Target != lastTarget)) {
                slerp = 0;
            }

            if (Target != null) {
                lastTarget = Target;
            } else {
                lastTarget = null;
            }
            lastQuaternion = q;

            if (slerp < 0.75f) {
                slerp += 0.25f * rotationSpeed;
            }
            else if (slerp < 1f) {
                slerp += 0.25f / 4 * rotationSpeed;
            }
            else {
                slerp = 0;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, q, slerp);
        }

        public void Rotate()
        {
            if (Target != null) {
                Vector3 relativePos = Target.transform.position - transform.position;
                float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg - 90;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                RotateTowards(rotation);
            }
            else {
                RotateTowards(Rotation);
            }
        }

        public void Shoot()
        {
            GameObject weapon = GetComponent<Operator.OperatorState>().Weapon;
            Weapon.Weapon weaponScript = weapon.GetComponent<Weapon.Weapon>();
            if (Target != null) {
                Vector3 vectorToTarget = Target.transform.position - transform.position;
                Vector3 rotation = transform.up;
                float angle = Vector3.Angle(rotation, vectorToTarget);
                // CALCULATE THE NEEDED ANGLE GIVEN THE DISTANCE
                if (angle < 1f) {
                    GetComponent<Animator>().SetBool("Firing", true);
                    weaponScript.FireAt(Target);
                } else {
                    if (weaponScript.Firing) weaponScript.StopFiring();
                    GetComponent<Animator>().SetBool("Firing", false);
                }
            } else {
                if (weaponScript.Firing) weaponScript.StopFiring();
                GetComponent<Animator>().SetBool("Firing", false);
            }
        } 
    }
}