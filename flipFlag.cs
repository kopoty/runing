using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipFlag : MonoBehaviour
{
    public GameObject _flagOnPlayer;

    private SpriteRenderer playerFlip;
    private SpriteRenderer flagFlip;
     
    // private BoxCollider2D flagCollider;
    private BoxCollider2D flagCollider;
    
    public BoxCollider2D baseCol;
    public GameObject flagOnBase;
    public GameObject vfxComplete;
    public GameObject player; 

    public LayerMask baseLayer;

    int axis = 1;
    // Start is called before the first frame update
    void Start()
    {
        playerFlip = player.GetComponent<SpriteRenderer>();
        flagFlip = GetComponent<SpriteRenderer>();
        flagCollider = GetComponent<BoxCollider2D>();
        baseCol = flagOnBase.GetComponent<BoxCollider2D>();
        // flagCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        flip();
        onBase();
    }

    private void flip()
    {
        if(playerFlip.flipX) 
        {
            flagFlip.flipX = false;
            axis = -1;
        }
        else
        {
            flagFlip.flipX = true;
            axis = 1;
        }

        transform.position = player.transform.position + new Vector3(-axis * 0.4f, 0 ,0);
        transform.rotation = Quaternion.Euler (0,0, axis * 45);
    }

    private void onBase()  // ставим флаг на базу, если персонаж принёс флаг
    {
        RaycastHit2D hit = Physics2D.BoxCast(flagCollider.bounds.center, flagCollider.bounds.size, 360, Vector2.one, 0.3f, baseLayer);
        
        if (hit.collider != null) 
        {   

            _flagOnPlayer.SetActive(false);

           
            

            GameObject fl = Instantiate(flagOnBase) as GameObject;
            fl.transform.position = new Vector2(hit.transform.position.x + baseCol.bounds.extents.x, hit.transform.position.y + baseCol.bounds.extents.y );
            
            GameObject fxComp = Instantiate(vfxComplete) as GameObject;
            fxComp.transform.position = new Vector2(fl.transform.position.x - baseCol.bounds.extents.x, fl.transform.position.y ) ;
        }
    }
}
