using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTile : MonoBehaviour {
    [SerializeField] private UpgradeSO backend;

    public UpgradeSO Backend => backend;

    private Image _image;
    public bool IsDone { get; private set; }

    private void Awake() {
        _image = GetComponent<Image>();
        _image.sprite = backend.unDone;

        IsDone = false;
    }

    public void Build() {
        _image.sprite = backend.done;
        IsDone = true;
    }

    public void Reset() {
        _image.sprite = backend.unDone;
        IsDone = false;
    }
}