using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent _agent;
    public GameObject _player;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Transform _transform;
    /// <summary>�����̒l��������Δ��������͈͂��ς��/// </summary>
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
    }//�G��Player �����m
    void Attack()
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(0, 0, 5); // X��������\���x�N�g��
        Ray ray = new Ray(transform.position, direction); // Ray�𐶐�;

        if (Physics.Raycast(ray, dis, _layerMask))
        {
            Debug.Log("������");
        }//�����ɍU���̏�����A�j���[�V����������
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.5f);
        Gizmos.DrawSphere (transform.position, _discoverySphere);
    }
}
