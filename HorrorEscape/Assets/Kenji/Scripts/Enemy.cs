using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected float defaultMoveSpeed;
    [SerializeField] GameObject _player;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Transform[] _goalpos;
    private int _destNum = 0; //目的地指定するために作るための拡張変数
    /// <summary>ここの値をいじれば発見される範囲が変わる/// </summary>
    protected float _discoverySphere;
    public bool _disState = false;
    public float _dis;
    protected Animator _animator;

    public float MoveSpeed { set; get; }

    // Start is called before the first frame update
    void Start()
    {
        _agent = this.gameObject.GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.SetDestination(_goalpos[_destNum].position);
        defaultMoveSpeed = _agent.speed;
        MoveSpeed = defaultMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _dis = Vector3.Distance(transform.position, _player.transform.position);

        nextGoal();//指定位置巡回に戻る
        Discavery();
        Attack();
    }
    void Discavery()
    {
        RaycastHit[] discoverySphere = Physics.SphereCastAll(transform.position, 3.0f, Vector3.forward);

        if (discoverySphere.Length > _dis) 
        {
            _agent.destination = _player.transform.position;
            _disState = true;
            _animator.SetFloat("Speed", 3.5f);
        }
        else
        {
            _animator.SetFloat("Speed", 1f);
        }
    }//敵がPlayer を検知
    void Attack()
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(0, 0, 5); // X軸方向を表すベクトル
        Ray ray = new Ray(transform.position, direction); // Rayを生成;

        if (Physics.Raycast(ray, out RaycastHit data, _dis, _layerMask))
        {
            if (data.collider.gameObject.CompareTag("player")) 
            {
                Debug.Log("当たる");
            }
        }//ここに攻撃の処理やアニメーションをかく
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 1f);
        Gizmos.DrawSphere(this.transform.position, _discoverySphere);
    }
    protected void nextGoal()
    {
        if(_agent.remainingDistance < _agent.stoppingDistance)
        {
            _destNum = (_destNum + 1 ) % _goalpos.Length;
            _agent.SetDestination(_goalpos[_destNum].position);
        }
        Debug.Log("Ss00");
    }
}
