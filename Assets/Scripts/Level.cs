using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Note {
    public float time;
    public float radius;
}
    
[Serializable]
public class RegularNote: Note {
        
}
    
[Serializable]
public class SlideNote: Note {
        
}
    
[Serializable]
public class CenterNote: Note {
        
}

[Serializable]
public class Path {
    public string name;
    public float delay;
    public float duration;
    public iTween.EaseType easeType;
    public iTween.LoopType loopType;
}
    
[CreateAssetMenu(fileName = "Level", menuName = "Alio/Level", order = 0)]
public class Level : ScriptableObject {
    [SerializeReference]
    public List<Note> notes = new();
    
    public List<Path> paths = new();
}