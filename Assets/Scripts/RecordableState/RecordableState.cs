using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordableState
{
    public class RecordableState
    {
        private List<RecordableProperty> properties = new List<RecordableProperty>();

        public RecordableState() {}

        public T GetProperty<T>() where T : RecordableProperty
        {
            foreach (RecordableProperty property in properties) {
                if (property.GetType() == typeof(T)) {
                    Debug.Log(property.GetType());
                    return (T)System.Convert.ChangeType(property, typeof(T));
                }
            }
            return (T)System.Convert.ChangeType(null, typeof(T)); ;
        }

        public void AddProperty<T>() where T : RecordableProperty, new()
        {
            if (GetProperty<T>() != null)
            {
                Debug.Log("Cannot add an already existing property");
                return;
            }
            properties.Add(new T());
        }

        public void AddProperty<T>(T property) where T : RecordableProperty
        {
            if (GetProperty<T>() != null) {
                Debug.Log("Cannot add an already existing property");
                return;
            }
            properties.Add(property);
        }
    }
}