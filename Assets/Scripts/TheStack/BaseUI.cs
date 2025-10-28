using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    protected TheStackUIManager uiManager;

    public virtual void Init(TheStackUIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    protected abstract UIState GetUIState();
    public void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }
}
