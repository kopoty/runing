
public class Flag : MonoBehaviour
{
    public GameObject flagOnPlayer;
    public GameObject flag;

    public GameObject vfx;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D other) // инициализируем игрока, который коснулся флага
    {

        if(other.CompareTag("Player") && flagOnPlayer.activeSelf == false) // если к флагу коснулся объект с тегом игрок и объект флаг на игроке не активен
        {   
            GameObject fxComp = Instantiate(vfx) as GameObject;
            fxComp.transform.position = new Vector2(other.transform.position.x, other.transform.position.y);

            flagOnPlayer.SetActive(true);

            Destroy(fxComp, 1.02f);
            Destroy(flag);
        }        
    }

  
}
