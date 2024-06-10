using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent _agent;
    public GameObject _player;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Transform _transform;
    /// <summary>ここの値をいじれば発見される範囲が変わる/// </summary>
    public float _discoverySphere;
    public bool _disState = false;
    public float dis;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Discavery();
          dis = Vector3.Distance(transform.position, _player.transform.position);
        if (_discoverySphere > dis)
        {
            _agent.destination = _player.transform.position;
            _disState = true;
        }//
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

        if (Physics.Raycast(ray, dis, _layerMask))
        {
            Debug.Log("当たる");
        }//ここに攻撃の処理やアニメーションをかく
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.5f);
        Gizmos.DrawSphere (transform.position, _discoverySphere);
    }
}
