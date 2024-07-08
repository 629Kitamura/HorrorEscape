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
            nextGoal();//�w��ʒu����ɖ߂�
            _animator.SetTrigger("walk02");
        }
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

        if (Physics.Raycast(ray, _dis, _layerMask))
        {
            Debug.Log("������");
        }//�����ɍU���̏�����A�j���[�V����������
    }
    protected void nextGoal()
    {
        _destNum += 1;
        if (_destNum == 3)
        {
            _destNum = 0;
            //_destNum = Random.Range(0, 3); //�����_���̏ꍇ
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
