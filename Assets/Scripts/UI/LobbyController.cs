using System;
using UnityEngine;

namespace UI
{
    public class LobbyController : MonoBehaviour
    {
        public event Action OnPlayPressed;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Play()
        {
            OnPlayPressed?.Invoke();
        }
    }
}