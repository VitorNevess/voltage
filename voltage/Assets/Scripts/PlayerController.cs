using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    public float speed;
    public float jumpForce;
    //esta olhando para esquerda
    public bool isLookLeft;

    public Transform groundCheck;
    private bool isGrounded;//é chao
    private bool isAtack;

    public Transform mao;
    public GameObject hitBoxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //captura os componetes do objeto da cena
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetAxisRaw = Não tem sensibilidade no analogico, se o jogador pressionar mais forte ou mais fraco mais andar a mesma qtd
        float h = Input.GetAxisRaw("Horizontal");

        //se ele atacar no chao ele para na hora, no ar nao
        if(isAtack && isGrounded)
        {
            h = 0;
        }

        if(h > 0 && isLookLeft)
        {
            Flip();
        }else if (h < 0 && !isLookLeft)
        {
            Flip();
        }

        float speedY = playerRb.velocity.y;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButtonDown("Fire1") && !isAtack)
        {
            isAtack = true;
            playerAnimator.SetTrigger("atack");
        }

        //nao sei o que faz
        playerRb.velocity = new Vector2(h * speed, speedY);

        //Seta os valores que estao no animator do personagem
        playerAnimator.SetInteger("horizontal", (int)h);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAtack", isAtack);
    }

    //Atualizacao fixa em 0.02 segundos
    private void FixedUpdate()
    {
        //cria um colisor de cicurlo, em baixo do colisor do protagonista, quando ele estiver no cao
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    //Vira o personagem para esquerda ou direita
    void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;//inverte o sinal do X do transforme
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.y);
    }

    void OnEndAtack()
    {
        isAtack = false;
    }

    void hitBoxAtack()
    {
        //faz a instancia do objeto prefab
        GameObject hitBoxTemp = Instantiate(hitBoxPrefab, mao.position, transform.localRotation);
        //tempo de vida da caixa de colisao
        Destroy(hitBoxTemp, 0.2f);
    }
}
