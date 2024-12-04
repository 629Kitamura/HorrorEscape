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
    private int _destNum = 0; //�ړI�n�w�肷�邽�߂ɍ�邽�߂̊g���ϐ�
    /// <summary>�����̒l��������Δ��������͈͂��ς��/// </summary>
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

        nextGoal();//�w��ʒu����ɖ߂�
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
    }//�G��Player �����m
    void Attack()
    {
        RaycastHit hit;
        Vector3 direction = new Vector3(0, 0, 5); // X��������\���x�N�g��
        Ray ray = new Ray(transform.position, direction); // Ray�𐶐�;

        if (Physics.Raycast(ray, out RaycastHit data, _dis, _layerMask))
        {
            if (data.collider.gameObject.CompareTag("player")) 
            {
                Debug.Log("������");
            }
        }//�����ɍU���̏�����A�j���[�V����������
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
