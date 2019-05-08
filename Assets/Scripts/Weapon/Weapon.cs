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
        public bool Firing { get; private set; } = false;
        public bool hasShotOnce = false;

        // sound variables
        protected AudioSource source;
        private float volLow = .5f;
        private float volHigh = 1.0f;

        public void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayShootSound()
        {
            //var sound = GameObject.Find("ShootSound").GetComponent<SoundAssets>().sounds["shootSound"];
            //var sound = GameObject.Find("ShootSound").GetComponent<AudioClip>();
            //source = GameObject.Find("ShootSound").GetComponent<AudioSource>();
            float vol = Random.Range(volLow, volHigh); // vary the volume to increase immersion
            if (!source.isPlaying) {
                source.Play(); // play AudioClip of AudioSource
            }
            
            //source.volume = vol;
        }

        public void FireAt(GameObject target)
        {
            Firing = true;
            PlayShootSound();
            
            if (Stage == 0) {
                if (Random.Range(0.0f, 1.0f) < HitDifficulty) target.GetComponent<Operator.OperatorState>().Damage(Damage);
            }
            Stage += FireRate / 60 * Time.fixedDeltaTime;
            while (1 < Stage) {
                if (Random.Range(0.0f, 1.0f) < HitDifficulty) target.GetComponent<Operator.OperatorState>().Damage(Damage);
                Stage -= 1;
            }
        }

        public void StopFiring()
        {
            Firing = false;
        }
    }
}
