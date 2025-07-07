using UnityEngine;

public class BallBounce : MonoBehaviour
{
    #region 小球反弹属性参数
    //刚体组件反馈
    private Rigidbody2D rb;

    //最低速度
    public float minSpeed;

    //速度上限
    public float maxSpeed;

    //反弹力度调整系数
    public float bounceMultiplier;
    #endregion
   

    #region Unity自带函数
    private void Awake()
    {
        //基本组件获取
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDir(Vector2 _fireSpeed , float _gravity)
    {
        rb.velocity = _fireSpeed;
        rb.gravityScale = _gravity;
    }

    private void FixedUpdate()
    {
        //限制弹球速度在合理范围内
        if (rb.velocity.magnitude < minSpeed)
        {
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
        else if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    #endregion

    //碰撞反馈
    void OnCollisionEnter2D(Collision2D collision)
    {
        //只有碰到障碍物时，才会增强反弹
        if (collision.transform.tag == "Obstacle")
        {
            //增强反弹效果
            Vector2 normal = collision.contacts[0].normal;
            Vector2 newVelocity = Vector2.Reflect(rb.velocity, normal) * bounceMultiplier;
            Vector2 adjustedVelocity = Quaternion.Euler(0, 0, Random.Range(-10f, 10f)) * newVelocity;
            rb.velocity = adjustedVelocity;
        }
    }
}