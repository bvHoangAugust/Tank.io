using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerTankManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject LocalPlayerInstance;
    public TankModel tank;
    public ShootInfo currentShootInfo;

    private FixedJoystick joystick;
    private FixedJoystick joystickBarel;

    public float sizeOfTank;

    [SerializeField]
    private GameObject playerUiPrefab;

    [Header("Shoot")]
    public Transform shootTransform;
    public GameObject bulletPrefab;

    [Header("Part Of Tank")]
    public GameObject tankTurret;

    void Awake()
    {
        if (photonView.IsMine)
        {
            photonView.ObservedComponents.Add(this);
            LocalPlayerInstance = gameObject;

            gameObject.tag = "MainPlayer";
            tank.health = tank.baseHP;
            sizeOfTank = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y;

            joystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
            joystickBarel = GameObject.FindWithTag("JoystickBarel").GetComponent<FixedJoystick>();
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController != null)
        {
            if (photonView.IsMine)
            {
                cameraController.SetUpTransformFollow(transform);
            }
        }

        if (this.playerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(this.playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        //SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if(this.tank.timeShoot < this.tank.maxTimeDelayShoot)
            {
                this.tank.timeShoot += Time.deltaTime;
            }
            MoveWithJoystick();
            RotateTurretWithJoystick();
        }
    }

    [PunRPC]
    public void Hit(int dame)
    {
        this.tank.health -= dame;
        if (this.tank.health <= 0)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
                MainGameManager.instance.LeaveRoom();
            }
            else
            {
                MainGameManager.instance.SetScore(10);
            }
        }
    }
    void Shoot()
    {
        if(this.tank.timeShoot >= this.tank.maxTimeDelayShoot)
        {
            GameObject bullet = PhotonNetwork.Instantiate(this.bulletPrefab.name, shootTransform.position, tankTurret.transform.rotation);
            BulletController bulletControl = bullet.GetComponent<BulletController>();
            bulletControl.SetDame(tank.dame);
            bulletControl.MoveBullet(tank.speedBullet);
            this.tank.timeShoot = 0;
        }
    }


    void CalledOnLevelWasLoaded(int level)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }

        GameObject _uiGo = Instantiate(this.playerUiPrefab);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }

    #region JoystickMove
    void MoveWithJoystick()
    {
        transform.position += new Vector3(joystick.Horizontal, joystick.Vertical) * tank.moveSpeed * Time.deltaTime;

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            //transform.rotation = Quaternion.(new Vector3(myBody.velocity.x, myBody.velocity.y));

            Quaternion eulerRot = Quaternion.Euler(0, 0, -Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * 180 / Mathf.PI);
            transform.rotation = Quaternion.Slerp(transform.rotation, eulerRot, Time.deltaTime * tank.barrelRotateSpeed);
        }
    }

    void RotateTurretWithJoystick()
    {
        if (joystickBarel.Horizontal != 0 || joystickBarel.Vertical != 0)
        {
            Quaternion eulerRot = Quaternion.Euler(0, 0, -Mathf.Atan2(joystickBarel.Horizontal, joystickBarel.Vertical) * 180 / Mathf.PI);
            tankTurret.transform.rotation = Quaternion.Slerp(tankTurret.transform.rotation, eulerRot, Time.deltaTime * tank.barrelRotateSpeed);
            Shoot();
        }
    }
    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            //stream.SendNext(this.IsFiring);
            stream.SendNext(this.tank.health);
        }
        else
        {
            // Network player, receive data
            //this.IsFiring = (bool)stream.ReceiveNext();
            this.tank.health = (int)stream.ReceiveNext();
        }
        
    }

    #endregion
}
