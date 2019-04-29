using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordableState
{
    public class RecordableState
    {
        List<RecordableProperty> components = new List<RecordableProperty>();

        public RecordableState()
        {
            components.Add(new Position(new Vector3(0, 0, 0)));
        }

        public T GetProperty<T>()
        {
            foreach (RecordableProperty component in components) {
                if (component.GetType() == typeof(T)) {
                    Debug.Log(component.GetType());
                    return (T)System.Convert.ChangeType(component, typeof(T));
                }
            }
            return (T)System.Convert.ChangeType(null, typeof(T)); ;
        }

        public void AddComponent(RecordableProperty component)
        {
            components.Add(component);
        }
    }
}