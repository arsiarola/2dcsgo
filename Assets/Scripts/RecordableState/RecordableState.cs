using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordableState
{
    public class RecordableState
    {
        public List<RecordableProperty> Properties { get; private set; } = new List<RecordableProperty>();
        private GameObject Owner { get; set; }

        public RecordableState(GameObject recordable)
        {
            Owner = recordable;
        }

        public T GetProperty<T>() where T : RecordableProperty
        {
            foreach (RecordableProperty property in Properties) {
                if (property.GetType() == typeof(T)) {
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
            T property = new T();
            property.GetVariablesFrom(Owner);
            Properties.Add(property);
        }

        /*public void AddProperty<T>(T property) where T : RecordableProperty
        {
            if (GetProperty<T>() != null) {
                Debug.Log("Cannot add an already existing property");
                return;
            }
            properties.Add(property);
        }*/

        public void SetToObject(GameObject obj)
        {
            foreach (RecordableProperty property in Properties) {
                if (property is ISettable) {
                    ISettable settable = property as ISettable;
                    settable.SetToObject(obj);
                }
            }
        }

        public void InitOwner()
        {
            foreach (RecordableProperty property in Properties) {
                if (property is IInitializable) {
                    IInitializable initializable = property as IInitializable;
                    initializable.InitToObject(Owner);
                }
            }
        }
    }
}