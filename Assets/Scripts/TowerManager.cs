using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager>
{

    public TowerBtn towerBtnPress { get; set; }

    private SpriteRenderer spriteRenderer;
    private List<Tower> TowerList = new List<Tower>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private List<Projectile> ProjectileList = new List<Projectile>();
    private Collider2D buildTile;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mapPoint, Vector2.zero);
            if (hit.collider.tag == "BuildPlace")
            {
                buildTile = hit.collider;
                if (towerBtnPress)
                {
                    buildTile.tag = "PlaceFull";
                    RegistarBuildState(buildTile);
                    PlaceTower(hit);
                }
            }
            else
            {
                Debug.Log("Do nothing");
            }
            
        }
        if (spriteRenderer.enabled)
            FollowMouse();
    }

    public void RegistarBuildState(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RegistarTower(Tower tower)
    {
        TowerList.Add(tower);
    }

    public void RenameTagsBuildSite()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "BuildPlace";
        }
        BuildList.Clear();
    }

    public void DestroyAllTowers()
    {
        foreach (Tower tower in TowerList)
            Destroy(tower.gameObject);

        TowerList.Clear();
    }

    public void RegistarProjectile(Projectile projectile)
    {
        ProjectileList.Add(projectile);
    }

    public void DestroyAllProjectiles()
    {
        foreach (Projectile projectile in ProjectileList)
            Destroy(projectile.gameObject);

        ProjectileList.Clear();
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPress != null && GameManager.Instance.TotalMoney >= towerBtnPress.TowerPrice) //zbog UI
        {
            Tower newTower = Instantiate(towerBtnPress.TowerObject);
            newTower.transform.position = hit.transform.position;
            BuyTower(towerBtnPress.TowerPrice);
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerPlace);
            RegistarTower(newTower);
            DisableDragSprite();
            towerBtnPress = null;
        }
    }

    public void SelectedTower(TowerBtn towerSelected)
    {
        if (towerSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towerBtnPress = towerSelected;
            Debug.Log("Press" + towerBtnPress.gameObject);

            EnableDragSprite(towerBtnPress.DragSprite);
        }
    }

    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void EnableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void DisableDragSprite()
    {
        spriteRenderer.enabled = false;
    }

    public void BuyTower(int price)
    {
       
        GameManager.Instance.SubtracyMoney(price);
    }
}
