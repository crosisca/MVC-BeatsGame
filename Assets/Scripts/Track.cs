using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Beats
{
    //Model
    [CreateAssetMenu(menuName = "Beats/New Track", fileName = "New Beats Track.asset")]
    public class Track : ScriptableObject
    {
        [Header("Playback Settings")]

        [Tooltip("# of beats per minute")]
        [Range(30, 360)]
        public int bpm = 120;

        [HideInInspector]
        public List<int> beats;

        //Shouldn't have data generation method inside the model itself
        public static int inputs = 4;

        [Header("Random Settings")]

        [Tooltip("# of preroll (empty) beats")]
        [Range(0, 10)]
        [SerializeField]
        int _preroll = 10;

        [Tooltip("Minimum # of beats per block")]
        [Range(1, 20)]
        [SerializeField]
        int _minBlock = 2;

        [Tooltip ("Maximim # of beats per block")]
        [Range(1, 20)]
        [SerializeField]
        int _maxBlock = 5;

        [Tooltip("Minimum # of empty beats between blocks")]
        [Range(1, 20)]
        [SerializeField]
        int _minInterval = 1;

        [Tooltip("Maximum # of empty beats between blocks")]
        [Range(1, 20)]
        [SerializeField]
        int _maxInterval = 2;

        [Tooltip("# of beats blocks")]
        [Range(1, 20)]
        [SerializeField]
        int _blocks = 10;

        public void Randomize()
        {
            beats = new List<int>();

            //Preroll
            for (int i = 0; i < _preroll; i++)
                beats.Add((int)Beat.EMPTY);

            //Blocks
            for (int block = 0; block < _blocks; block++)
            {
                //Beats
                int blockLength = Random.Range(_minBlock, _maxBlock + 1);
                for (int j = 0; j < blockLength; j++)
                {
                    int beat = Random.Range(0, inputs);
                    beats.Add(beat);
                }

                //Dont add interval on  last block
                if (block == _blocks - 1)
                    break;

                //Interval
                int intervalLength = Random.Range(_minInterval, _maxInterval + 1);
                for (int j = 0; j < intervalLength; j++)
                    beats.Add((int)Beat.EMPTY);
            }


        }
    }
}