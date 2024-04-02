using System;
using UnityEngine;

namespace Tridi
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private GameObject hud;
        [SerializeField] private GameObject pauseMenu;

        private void OnEnable()
        {
            PauseManager.onPaused += Paused;
            PauseManager.onUnPaused += UnPaused;
        }

        private void OnDisable()
        {
            PauseManager.onPaused -= Paused;
            PauseManager.onUnPaused -= UnPaused;
        }

        private void Paused()
        {
            hud.SetActive(false);
            pauseMenu.SetActive(true);
        }
        
        private void UnPaused()
        {
            hud.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }
}