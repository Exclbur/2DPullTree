using UnityEngine;

public class BallBounce : MonoBehaviour
{
    #region С�򷴵����Բ���
    //�����������
    private Rigidbody2D rb;

    //����ٶ�
    public float minSpeed;

    //�ٶ�����
    public float maxSpeed;

    //�������ȵ���ϵ��
    public float bounceMultiplier;
    #endregion
   

    #region Unity�Դ�����
    private void Awake()
    {
        //���������ȡ
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDir(Vector2 _fireSpeed , float _gravity)
    {
        rb.velocity = _fireSpeed;
        rb.gravityScale = _gravity;
    }

    private void FixedUpdate()
    {
        //���Ƶ����ٶ��ں���Χ��
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

    //��ײ����
    void OnCollisionEnter2D(Collision2D collision)
    {
        //ֻ�������ϰ���ʱ���Ż���ǿ����
        if (collision.transform.tag == "Obstacle")
        {
            //��ǿ����Ч��
            Vector2 normal = collision.contacts[0].normal;
            Vector2 newVelocity = Vector2.Reflect(rb.velocity, normal) * bounceMultiplier;
            Vector2 adjustedVelocity = Quaternion.Euler(0, 0, Random.Range(-10f, 10f)) * newVelocity;
            rb.velocity = adjustedVelocity;
        }
    }
}