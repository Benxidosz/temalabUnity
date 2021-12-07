using ScriptableObjects.CardObjects;
using UnityEngine;
using UnityEngine.UI;

namespace ActionCards
{
    public class CardScript : MonoBehaviour
    {
        [SerializeField] private CardObject backend;

        public CardObject Backend
        {
            get => backend;
            set
            {
                backend = value;
                RefreshUI();
            }
        }

        private Text[] _texts;
        private Image _img;

        private void Awake()
        {
            _texts = GetComponentsInChildren<Text>();
            _img = GetComponent<Image>();
        }

        private void RefreshUI()
        {
            _texts[0].text = backend.name;
            _texts[0].fontSize = backend.titleSize;
            _texts[1].text = backend.description;
            _img.sprite = backend.background;
        }

        public void OnClick()
        {
            Backend.action?.Invoke("Hello");
        }
    }
}