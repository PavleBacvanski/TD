using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioClip arrow;
    [SerializeField] private AudioClip fireball;
    [SerializeField] private AudioClip rock;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip level;
    [SerializeField] private AudioClip newGame;
    [SerializeField] private AudioClip towerPlace;

    public AudioClip Arrow { get => arrow; }
    public AudioClip Fireball { get => fireball; }
    public AudioClip Rock { get => rock;  }
    public AudioClip Death { get => death; }
    public AudioClip GameOver { get => gameOver; }
    public AudioClip Hit { get => hit; }
    public AudioClip Level { get => level; }
    public AudioClip NewGame { get => newGame; }
    public AudioClip TowerPlace { get => towerPlace; }

    
}


