using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Sentence[] sentences;

    [Serializable]
    public struct Sentence
    {
        [TextArea(1, 3)]
        public string words;
        public int characterIndex;
    }
}