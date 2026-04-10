using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Sentence[] sentences;

    [Serializable]
    public struct Sentence
    {
        public string words;
        public int characterIndex;
    }
}
