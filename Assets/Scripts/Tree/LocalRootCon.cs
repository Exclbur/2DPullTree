using System.Collections;
using UnityEngine;

public class LocalRootCon : MonoBehaviour
{
    
        private LoaclPlayer player;

        public float shakeAmount = 0.1f; // ��������
        public float shakeDuration = 0.5f; // ��������ʱ��

        private Vector3 originalPosition; // ����ĳ�ʼλ��

        private Animator anim;
        private void Start()
        {
            anim = GetComponent<Animator>();
            player = GetComponentInParent<LoaclPlayer>();

        }

   

    private void Update()
    {
        if (player.soilAmount < 110)
        {
            anim.SetBool("middle", true);
            
        }
        if (player.soilAmount < 80)
        {
            anim.SetBool("end", true);

        }
    }

        public IEnumerator Shake()
        {

            originalPosition = transform.position;

            float elapsedTime = 0f;

            // �ڶ�������ʱ���ڽ��ж���
            while (elapsedTime < shakeDuration)
            {
                // ÿ�ζ���ʱ����������λ��Ϊԭʼλ�� + һ�������ƫ����
                float xOffset = Random.Range(-shakeAmount, shakeAmount);
                float yOffset = Random.Range(-shakeAmount, shakeAmount);
                transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

                // ��������ʱ��
                elapsedTime += Time.deltaTime;

                // �ȴ���һ֡
                yield return null;

            }

            // ����������ȷ������ص���ʼλ��
            transform.position = originalPosition;

        }

    
}
