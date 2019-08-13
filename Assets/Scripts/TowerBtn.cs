using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField] private Tower towerObject;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private int towerPrice;

    public Tower TowerObject { get => towerObject; }
    public Sprite DragSprite { get => dragSprite;  }
    public int TowerPrice { get => towerPrice;  }


   
}
