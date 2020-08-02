using UnityEngine;
using UnityEngine.UI;

namespace Beats
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    [RequireComponent(typeof(ContentSizeFitter))]
    [RequireComponent(typeof(RectTransform))]
    public class TrackView : MonoBehaviour
    {
        [SerializeField]
        Track _track;

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


        public void Init(Track track)
        {
            _rectTransform = (RectTransform) transform;
            _position = _rectTransform.anchoredPosition;

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

                Transform view = GameObject.Instantiate(go, transform).transform;
                view.SetAsFirstSibling();
            }

        }

        void Start()
        {
            Init(_track);
        }

        void Update()
        {
            position -= Time.deltaTime * 100;
        }
    }
}