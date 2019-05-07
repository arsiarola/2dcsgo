using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operator
{
    public class Weapon
    {
        public float FireRate { get; private set; } = 600;    // rpm
        public float Stage { get; private set; } = 0;
        public float Damage { get; private set; } = 36;
        public float HitDifficulty { get; private set; } = 0.8f;

        // sound variables
        private AudioSource source; 
        private float volLow = .5f;
        private float volHigh = 1.0f;

        public Weapon()
        {
           
        }

        public void PlayShootSound()
        {
            //var sound = GameObject.Find("ShootSound").GetComponent<SoundAssets>().sounds["shootSound"];
            var sound = GameObject.Find("ShootSound").GetComponent<AudioClip>();
            source = GameObject.Find("ShootSound").GetComponent<AudioSource>();
            float vol = Random.Range(volLow, volHigh); // vary the volume to increase immersion
            source.Play(); // play AudioClip of AudioSource
        }

        public void FireAt(GameObject target)
        {
            PlayShootSound();
            if (Stage == 0) {
                if (Random.Range(0.0f, 1.0f) < HitDifficulty) target.GetComponent<OperatorState>().Damage(Damage);
            }
            Stage += FireRate / 60 * Time.fixedDeltaTime;
            while (1 < Stage) {
                if (Random.Range(0.0f, 1.0f) < HitDifficulty) target.GetComponent<OperatorState>().Damage(Damage);
                Stage -= 1;
            }
        }
    }
}
