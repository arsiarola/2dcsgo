using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordableState
{
    interface ISettable
    {
        void SetToObject(GameObject obj);
    }

    interface IInitializable
    {
        void InitToObject(GameObject obj);
    }

    public abstract class RecordableProperty
    {
        public abstract void GetVariablesFrom(GameObject recordable);
    }

    public class Transform : RecordableProperty, ISettable
    {
        public Vector3 Position { get; private set; } = new Vector3(0, 0, 0);
        public Vector3 Rotation { get; private set; } = new Vector3(0, 0, 0);

        public override void GetVariablesFrom(GameObject recordable)
        {
            Position = recordable.transform.position;
            Rotation = recordable.transform.rotation.eulerAngles;
        }

        public void SetToObject(GameObject obj)
        {
            obj.transform.position = Position;
            obj.transform.eulerAngles = Rotation;
        }
    }

    public class Dynamics : RecordableProperty, IInitializable
    {
        public Vector3 Velocity { get; private set; } = new Vector3(0, 0, 0);

        public override void GetVariablesFrom(GameObject recordable)
        {
            Velocity = recordable.GetComponent<Rigidbody2D>().velocity;
        }

        public void InitToObject(GameObject obj)
        {
            obj.GetComponent<Rigidbody2D>().velocity = Velocity;
        }
    }

    public class Animations : RecordableProperty, ISettable
    {
        public List<AnimationState> AnimationStates { get; private set; } = new List<AnimationState>();

        public override void GetVariablesFrom(GameObject recordable)
        {
            Animator animator = recordable.GetComponent<Animator>();
            for (int i = 0; i < animator.layerCount; i++) {
                AnimationStates.Add(new AnimationState(animator, i));
            }
        }

        public void SetToObject(GameObject obj)
        {
            foreach (AnimationState anim in AnimationStates) {
                Animator animator = obj.GetComponent<Animator>();
                obj.GetComponent<Animator>().Play(anim.StateNameHash, anim.Layer, anim.Stage);
            }
        }
    }

    public class BaseAI : RecordableProperty
    {
        public Core.Side Side { get; private set; } = Core.Side.Neutral;
        public List<GameObject> VisibleEnemies { get; private set; } = new List<GameObject>();

        public override void GetVariablesFrom(GameObject recordable)
        {
            AI.AI ai = recordable.GetComponent<AI.AI>();
            VisibleEnemies = ai.VisibleEnemies;
            Side = ai.Side;
        }
    }

    public class OperatorState : RecordableProperty
    {
        public float Hp { get; private set; } = 0;
        public bool IsAlive { get; private set; } = false;

        public override void GetVariablesFrom(GameObject recordable)
        {
            Operator.OperatorState state = recordable.GetComponent<Operator.OperatorState>();
            Hp = state.Hp;
            IsAlive = state.IsAlive();
        }
    }

    public class Audio : RecordableProperty, ISettable
    {
        public bool IsPlaying { get; private set; }
        public int TimeSamples { get; private set; }

        public override void GetVariablesFrom(GameObject recordable)
        {
            AudioSource source = recordable.GetComponent<AudioSource>();
            IsPlaying = source.isPlaying;
            TimeSamples = source.timeSamples;
        }

        public void SetToObject(GameObject obj)
        {
            AudioSource source = obj.GetComponent<AudioSource>();
            if (!source.isPlaying && IsPlaying && Time.timeScale != 0) {
                source.Play();
            }

            source.pitch = Time.timeScale;
        }
    }
}
