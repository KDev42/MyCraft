using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] InputController inputController;

    public void PauseGame()
    {
        inputController.LockInput();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        inputController.UnlockInput();
    }
}
