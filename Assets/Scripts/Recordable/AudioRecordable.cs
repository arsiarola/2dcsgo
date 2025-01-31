﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Recordable
{
    public class AudioRecordable : TransformRecordable
    {
        public bool StartPlayingAudio { get; set; } = false;

        protected override void AddProperties(RecordableState.RecordableState state)
        {
            base.AddProperties(state);
            state.AddProperty<RecordableState.Audio>();
        }

        public void PlayAudio()
        {
            StartPlayingAudio = true;
        }
    }
}
