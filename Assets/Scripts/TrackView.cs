using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Beats
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    [RequireComponent(typeof(ContentSizeFitter))]
    [RequireComponent(typeof(RectTransform))]
    public class TrackView : MonoBehaviour
    {
        public enum Trigger
        {
            Missed,
            Right,
            Wrong
        }


        [SerializeField]
        RectTransform _left;

        [SerializeField]
        RectTransform _right;

        [SerializeField]
        RectTransform _up;

        [SerializeField]
        RectTransform _down;

        [SerializeField]
        RectTransform _empty;

        RectTransform _rectTransform;
        List<Image> _beatViews;

        Vector2 _position;
        public float position
        {
            get { return _position.y; }
            set
            {
                if (value != _position.y)
                {
                    _position.y = value;
                    _rectTransform.anchoredPosition = _position;
                }
            }
        }
        
        float _beatViewSize;
        float _spacing;

        public void Init(Track track)
        {
            _rectTransform = (RectTransform) transform;
            _position = _rectTransform.anchoredPosition;

            _beatViewSize = _empty.rect.height;
            _spacing = GetComponent<VerticalLayoutGroup>().spacing;

            _beatViews = new List<Image>();

            foreach (int beat in track.beats)
            {
                GameObject go;
                switch (beat)
                {
                    case 0:
                        go = _left.gameObject;
                        break;
                    case 1:
                        go = _right.gameObject;
                        break;
                    case 2:
                        go = _up.gameObject;
                        break;
                    case 3:
                        go = _down.gameObject;
                        break;
                    default:
                        go = _empty.gameObject;
                            break;
                }

                Image view = GameObject.Instantiate(go, transform).GetComponent<Image>();
                view.transform.SetAsFirstSibling();

                _beatViews.Add(view);
            }
        }

        void Start()
        {
            Init(GameplayController.Instance.track);
        }

        void Update()
        {
            position -= (_beatViewSize + _spacing) * Time.deltaTime * GameplayController.Instance.beatsPerSecond;
        }

        public void TriggerBeatView(int beatIndex, Trigger trigger)
        {
            switch (trigger)
            {
                case Trigger.Missed:
                    _beatViews[beatIndex].color = Color.gray;
                    break;
                case Trigger.Right:
                    _beatViews[beatIndex].color = Color.yellow;
                    break;
                case Trigger.Wrong:
                    _beatViews[beatIndex].color = Color.cyan;
                    break;
            }
        }
    }
}