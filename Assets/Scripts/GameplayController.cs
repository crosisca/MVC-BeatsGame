using UnityEngine;
using System;
using System.Collections;

namespace Beats
{
    public enum Beat
    {
        EMPTY = -1,
        LEFT = 0,
        RIGHT = 1,
        UP = 2,
        DOWN = 3
    }

    public class GameplayController : MonoBehaviour
    {
        //Separate input handling from the gameplay controller

        [Header("Input")]
        [SerializeField]
        KeyCode _left = KeyCode.LeftArrow;

        [SerializeField]
        KeyCode _right = KeyCode.RightArrow;

        [SerializeField]
        KeyCode _up = KeyCode.UpArrow;

        [SerializeField]
        KeyCode _down = KeyCode.DownArrow;


        [Header("Track")]
        [Tooltip("Beats Track to play")]
        [SerializeField]
        Track _track;
        /// <summary>
        /// The current track
        /// </summary>
        public Track track => _track;

        public float beatsPerSecond { get; private set; }
        public float secondsPerBeat { get; private set; }

        bool _played;
        bool _completed;

        TrackView _trackView;

        WaitForSeconds waitAndStop;

        static GameplayController _instance;

        public static GameplayController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameplayController>();

                return _instance;
            }
            set { _instance = value; }
        }

        #region Monobehaviour Methods
        void Awake()
        {
            _instance = this;

            beatsPerSecond = track.bpm / 60f;
            secondsPerBeat = 60f / track.bpm;

            waitAndStop = new WaitForSeconds(secondsPerBeat * 2);//wait for 2 beats

            _trackView = FindObjectOfType<TrackView>();
            if(!_trackView)
                Debug.LogWarning("No TrackView found in current scene");
        }

        void Start ()
        {
            InvokeRepeating("NextBeat", 0f, secondsPerBeat);
        }

        void Update ()
        {
            if (_played || _completed)
                return;

            if (Input.GetKeyDown(_left))
                PlayBeat(Beat.LEFT);
            if (Input.GetKeyDown(_right))
                PlayBeat(Beat.RIGHT);
            if (Input.GetKeyDown(_up))
                PlayBeat(Beat.UP);
            if (Input.GetKeyDown(_down))
                PlayBeat(Beat.DOWN);
        }

        void OnDestroy()
        {
            _instance = null;
        }

        #endregion


        #region Gameplay

        int _current;
        public int current
        {
            get { return _current; }
            set
            {
                _current = value;

                if (_current == track.beats.Count)
                {
                    CancelInvoke("NextBeat");

                    _completed = true;

                    StartCoroutine(WaitAndStop());
                }
            }
        }

        void PlayBeat(Beat input)
        {
            PlayBeat((int)input);
        }

        void PlayBeat(int input)
        {
            _played = true;

            //Played when not suposed to
            if (_track.beats[current] == -1)
            {
                //Debug.Log($"{input} played untimely");
                //TODO Create some feedback for untimely
            }
            //Correct beat played
            else if (_track.beats[current] == input)
            {
                //Debug.Log($"{_track.beats[current]} played right");
                _trackView.TriggerBeatView(current, TrackView.Trigger.Right);
            }
            //Wrong beat played
            else
            {
                //Debug.Log($"{input} played, {_track.beats[current]} expected");
                _trackView.TriggerBeatView(current, TrackView.Trigger.Wrong);
            }
        }

        void NextBeat()
        {
            //Debug.Log("Tick");

            if (!_played && _track.beats[current] != -1)
            {
                //Debug.Log($"{_track.beats[current]} missed");
                _trackView.TriggerBeatView(current, TrackView.Trigger.Missed);
            }

            current++;

            _played = false;
        }

        IEnumerator WaitAndStop()
        {
            yield return waitAndStop;

            enabled = false;
        }
        

        #endregion
    }
}