using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    PlayerTankManager target;
    Transform targetTransform;
    Vector3 targetPosition;

    public TextMeshProUGUI playerNameText;
    public Image healthFillSlider;
    void Awake()
    {
        this.gameObject.transform.SetParent(GameObject.Find("MainGameCanvas").transform);
        this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
        if(healthFillSlider!= null)
        {
            float percent = (float)target.tank.health / target.tank.baseHP;
            if (percent >= 0.8f)
            {
                healthFillSlider.color = new Color32(0, 255, 97, 255);
            }
            else if(percent < 0.8f && percent >= 0.3f)
            {
                healthFillSlider.color = new Color32(255, 194, 10, 255);
            }
            else if(percent < 0.3f)
            {
                healthFillSlider.color = new Color32(254, 72, 23, 255);
            }
            healthFillSlider.fillAmount = percent;
        }
    }
    private void LateUpdate()
    {
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            Vector3 pos = Camera.main.WorldToScreenPoint(targetPosition);
            pos.y += 50f;
            this.transform.position = pos;
        }
    }

    public void SetTarget(PlayerTankManager _target)
    {
        if (_target == null)
        {
            return;
        }
        this.target = _target;
        targetTransform = this.target.transform;
        if (playerNameText != null)
        {
            playerNameText.text = this.target.photonView.Owner.NickName;
            playerNameText.color = new Color32(255, 140, 0, 255);
        }
    }
}
