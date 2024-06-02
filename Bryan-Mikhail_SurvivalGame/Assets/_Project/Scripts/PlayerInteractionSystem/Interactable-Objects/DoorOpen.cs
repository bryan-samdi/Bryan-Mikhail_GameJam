using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VHS
{
    public class DoorOpen : InteractableBase
    {
        public GameObject door;
        private bool doorOpen;

        public string doorTextOpen = "Open Door";
        public string doorTextClose = "Close Door";

        public override void OnInteract()
        {
            base.OnInteract();
            doorOpen = !doorOpen;
            door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);

            if (doorOpen)
            {
                toolTipMessage = doorTextClose;
            }
            else
            {
                toolTipMessage = doorTextOpen;
            }




        }

    }
}
