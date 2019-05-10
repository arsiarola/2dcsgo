using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract float FireRate { get; protected set; }   // rpm
        protected float Stage { get; set; } = 0;
        public abstract float Damage { get; protected set; }
        public abstract float HitDifficulty { get; protected set; }

        public float LastShot = 0;

        // sound variables
        protected AudioSource source;

        public void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayShootSound()
        {
            //var sound = GameObject.Find("ShootSound").GetComponent<SoundAssets>().sounds["shootSound"];
            //var sound = GameObject.Find("ShootSound").GetComponent<AudioClip>();
            //source = GameObject.Find("ShootSound").GetComponent<AudioSource>();
            //float vol = Random.Range(volLow, volHigh); // vary the volume to increase immersion
            /*if (!source.isPlaying) {
                source.Play(); // play AudioClip of AudioSource
            }*/
            source.Play();
            //source.loop = true;
            //source.volume = vol;
        }

        public void FireAt(GameObject target)
        {
            GetComponentInParent<Animator>().SetBool("Firing", true);

            if (Core.Vars.SimulationTime > FireRate + LastShot) {
                if (Random.Range(0.0f, 1.0f) < HitDifficulty) target.GetComponent<Operator.OperatorState>().Damage(Damage);
                GetComponent<Recordable.AudioRecordable>().StartPlayingAudio = true;
                GetComponentInParent<Animator>().ResetTrigger("Fire");
                GetComponentInParent<Animator>().SetTrigger("Fire");
                LastShot = Core.Vars.SimulationTime;
            }
        }
    }
}
