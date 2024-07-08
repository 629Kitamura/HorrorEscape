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
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.destination = _goalpos[_destNum].position;
        defaultMoveSpeed = _agent.speed;
        MoveSpeed = defaultMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Discavery();
          _dis = Vector3.Distance(transform.position, _player.transform.position);
        if (_discoverySphere > _dis)
        {
            _agent.destination = _player.transform.position;
            _disState = true;
            _animator.SetTrigger("Run02");
        }
        else
        {
            nextGoal();//指定位置巡回に戻る
            _animator.SetTrigger("walk02");
        }
        Attack();
    }
    void Discavery()
    {
        RaycastHit[] discoverySphere = Physics.SphereCastAll(transform.position, 3.0f, Vector3.forward);
    }//敵がPlayer を検知
    void Attack()
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(0, 0, 5); // X軸方向を表すベクトル
        Ray ray = new Ray(transform.position, direction); // Rayを生成;

        if (Physics.Raycast(ray, _dis, _layerMask))
        {
            Debug.Log("当たる");
        }//ここに攻撃の処理やアニメーションをかく
    }
    protected void nextGoal()
    {
        _destNum += 1;
        if (_destNum == 3)
        {
            _destNum = 0;
            //_destNum = Random.Range(0, 3); //ランダムの場合
        }
        _agent.destination = _goalpos[_destNum].position;
        Debug.Log("Ss00");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.5f);
        Gizmos.DrawSphere (transform.position, _discoverySphere);
    }
}
