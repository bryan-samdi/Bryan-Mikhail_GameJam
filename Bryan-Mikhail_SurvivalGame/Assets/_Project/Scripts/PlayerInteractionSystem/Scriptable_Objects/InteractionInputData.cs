
using UnityEngine;
using NaughtyAttributes;
namespace VHS
{

    [CreateAssetMenu(fileName = "InteractionInputData", menuName = "InteractionSystem/InputData")]
    public class InteractionInputData : ScriptableObject
    {
        private  bool m_interatedClicked;
        private bool m_interactedRelease;

        public bool InteractedClicked
        {
            get => m_interatedClicked;
            set => m_interatedClicked = value;
        }

        public bool InteractedReleased
        {
            get => m_interactedRelease;
            set => m_interactedRelease = value;
        }

        public void ResetInput()
        {
            m_interatedClicked = false;
            m_interactedRelease = false;
        }
    }
}
