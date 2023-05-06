using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Chicken : MonoBehaviour
{

    [SerializeField, Range(0, 1)] float moveDuration = 0.1f;
    [SerializeField, Range(0, 1)] float jumpHeight = 0.5f;

    [SerializeField] int leftMoveLimit;
    [SerializeField] int rightMoveLimit;
    [SerializeField] int backMoveLimit;

    public UnityEvent<Vector3> OnJumpEnd;

    public UnityEvent<int> OnGetCoin;
    
    public UnityEvent OnDie;

    public UnityEvent OnCarCollison;

    private bool isMoveable = false;

    // Update is called once per frame
    void Update()
    {

        if (isMoveable == false)
        {
            return;
        }

        //  Mengecek apakah suatu objek sedang dimanipulasi
        //  Untuk kasus ini transform dari GameObject ayam yang sedang di tweening
        //  Jika masih di tweening, maka player tidak dapat memasukkan input keyboard
        if (DOTween.IsTweening(transform))
        {
            return;
        }

        //  Posisi awal GameObject
        Vector3 direction = Vector3.zero;

        //  Jika button keyboard "W" ditekan, maka jalankan:
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //  GameObject menghadap kedepan
            direction += Vector3.forward;
        }

        //  Jika button keyboard "S" ditekan, maka jalankan:
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            //  GameObject menghadap kebelakang
            direction += Vector3.back;
        }

        //  Jika button keyboard "A" ditekan, maka jalankan:
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //  GameObject menghadap kekiri
            direction += Vector3.left;
        }

        //  Jika button keyboard "D" ditekan, maka jalankan:
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            //  GameObject menghadap kekanan
            direction += Vector3.right;
        }

        //  Jika player tidak menekan input apapun melalui keyboard
        //  Maka lanjut ke frame update berikutnya
        if (direction == Vector3.zero)
        {
            return;
        }

        //  Objek akan bergerak sesuai arah yang telah ditentunkan
        Move(direction);

    }

    public void Move(Vector3 direction)
    {

        var targetPosition = transform.position + direction;

        //  check apakah target posisi valid
        //  jika target posisi sudah terdapat pohon, maka tidak dapat tembus
        if (targetPosition.x < leftMoveLimit || targetPosition.x > rightMoveLimit || targetPosition.z < backMoveLimit || Tree.AllPositions.Contains(targetPosition))
        {
            targetPosition = transform.position;
        }

        //  ".onComplete = BroadCastPositionOnJumpEnd" merupakan fungsi callback
        //  dimana ketika karakter sudah selesai melompat akan memanggil fungsi
        //  yang bernama "BroadCastPositionOnJumpEnd"
        //  untuk menghitung travel distance dari karakter.
        transform.DOJump(targetPosition, jumpHeight, 1, moveDuration).onComplete = BroadCastPositionOnJumpEnd;

        //  GameObject akan menghadap sesuai arah dari GameObjectnya
        transform.forward = direction;
    }


    public void UpdateMoveLimit(int horizontalSize, int backLimit)
    {
        leftMoveLimit = -horizontalSize / 2;
        rightMoveLimit = horizontalSize / 2;
        backMoveLimit = backLimit;
    }


    private void BroadCastPositionOnJumpEnd()
    {
        OnJumpEnd.Invoke(transform.position);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Car"))
        {
            if (transform.localScale.y == 0.1f)
            {
                return;
            }

            transform.DOScale(new Vector3(1, 0.1f, 1), 0.2f);

            isMoveable = false;

            OnCarCollison.Invoke();

            Invoke("Die", 1);
        }

        else if (other.CompareTag("Coin"))
        {
            var coin = other.GetComponent<Coin>();
            OnGetCoin.Invoke(coin.Value);

            coin.Collected();
        }

        else if (other.CompareTag("Cursed Eagle"))
        {
            if (this.transform != other.transform)
            {
                this.transform.SetParent(other.transform);

                Invoke("Die", 1);
            }
        }
    }


    public void setMoveable(bool value)
    {
        isMoveable = value;
    }


    private void Die()
    {
        OnDie.Invoke();
    }
}
