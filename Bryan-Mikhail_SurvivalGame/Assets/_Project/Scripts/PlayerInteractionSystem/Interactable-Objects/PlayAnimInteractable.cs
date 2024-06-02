using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class PlayAnimInteractable : InteractableBase
    {
        private Animator _anim;
        private bool _isPlaying;

        public string _animPlaying = "Animation Playing";
        public string _startAnimation = "Play Animation";


        private void Start()
        {
            _anim = GetComponent<Animator>();
            toolTipMessage = _startAnimation;
        }

        public override void OnInteract()
        {
            base.OnInteract();
            _isPlaying = !_isPlaying;
            _anim.SetBool("Play", _isPlaying);

            if(_isPlaying )
            {
                toolTipMessage = _animPlaying;
            }
            else
            {
                toolTipMessage = _startAnimation;
            }


        }
    }
}
