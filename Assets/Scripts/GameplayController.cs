using UnityEngine;
using System;

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

        public float secondsPerBeat { get; private set; }
        public float beatsPerSecond { get; private set; }

        bool _played;
        bool _completed;

        #region Monobehaviour Methods

        void Awake()
        {
            secondsPerBeat = track.bpm / 60f;
            beatsPerSecond = 60f / track.bpm;
        }

        void Start ()
        {
            InvokeRepeating("NextBeat", 0f, beatsPerSecond);
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
                }
            }
        }

        void PlayBeat(Beat input)
        {
            PlayBeat((int)input);
        }

        void PlayBeat(int input)
        {
            Debug.Log((Beat)input);

            if (_track.beats[current] == -1)
            {
                //Played when not suposed to
                Debug.Log($"{input} played untimely");
            }
            else if (_track.beats[current] == input)
            {
                //Correct beat played
                Debug.Log($"{_track.beats[current]} played right");
            }
            else
            {
                //Wrong beat played
                Debug.Log($"{input} played, {_track.beats[current]} expected");
            }
            _played = true;
        }

        void NextBeat()
        {
            //Debug.Log("Tick");

            if (!_played && _track.beats[current] != -1)
                Debug.Log($"{_track.beats[current]} missed");

            current++;

            _played = false;
        }
        

        #endregion
    }
}