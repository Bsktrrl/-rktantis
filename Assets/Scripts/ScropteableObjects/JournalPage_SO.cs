using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JournalPage", menuName = "JournalPage", order = 1)]
public class JournalPage_SO : ScriptableObject
{
    public List<JournalPageInfo> journalPageList = new List<JournalPageInfo>();
}

[Serializable]
public class JournalPageInfo
{
    [HideInInspector] public JournalMenuState journalMenuState;

    public string title;
    public string autor;
    public string destination;
    public Vector3 date;
    public Vector3 time;

    [Space(10)]

    public int pageHeight = 530;
    [TextArea(3, 10)] public string message;

    [Space(10)]

    public AudioClip message_Clip;
}