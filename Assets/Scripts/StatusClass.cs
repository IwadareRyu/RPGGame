using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatusClass : MonoBehaviour
{
    [SerializeField] int _defaultHP;
    public int _hp = 100;
    public int HP => _hp;

    [SerializeField] int _defaultAttack;
    int _attack = 10;
    float _attackBuffTime = 0;
    bool _buffAttack;

    public int Attack => _attack;

    [SerializeField] int _defaultDiffence;
    int _diffence = 5;
    float _diffenceBuffTime = 0;
    bool _buffDiffence;

    public int Diffence => _diffence;
    [HideInInspector]
    public EnemyController _enemy;
    [SerializeField]
    Slider _hpSlider;

    DataBase _dataBase;
    public DataBase DataBase => _dataBase;

    public Survive _survive = Survive.Survive;

    public bool _death;

    public void Awake()
    {
        _dataBase = DataBase.Instance;
    }

    /// <summary>�o�t�̌��ʎ��Ԃ̏���</summary>
    public void TimeMethod()
    {
        //time�̕ϐ���0��荂�����A�b�����͂���B0�ɂȂ�����bool��true�ɂ��ăR���[�`���̑ҋ@�����𔲂���B
        if(_attackBuffTime > 0) { _attackBuffTime = DeltaTime(_attackBuffTime); }
        else if (!_buffAttack) { _buffAttack = true; }
        if(_diffenceBuffTime > 0) { _diffenceBuffTime = DeltaTime(_diffenceBuffTime); }
        else if (!_buffDiffence) { _buffDiffence = true; }
    }

    /// <summary>���Ԃ����炵�Ă�������</summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public float DeltaTime(float time)
    {
        return time - Time.deltaTime;
    }

    /// <summary>�L�����̐퓬�X�e�[�^�X��ݒ肷�鏈��</summary>
    public void SetStatus()
    {
        _hp = _defaultHP;
        _attack = _defaultAttack;
        _diffence = _defaultDiffence;
    }

    /// <summary>���ʂɃ_���[�W���󂯂����̏���</summary>
    /// <param name="damage"></param>
    /// <param name="skillParsent"></param>
    public void AddDamage(float damage,float skillParsent = 1)
    {
        _hp = _hp - (int)(damage * skillParsent - _diffence);
        ShowSlider();
    }

    /// <summary>����ȃ_���[�W���󂯂����̏���(�h��ђ�)</summary>
    /// <param name="damage"></param>
    /// <param name="skillParsent"></param>
    public void AddMagicDamage(float damage, float skillParsent = 1)
    {
        _hp = _hp - (int)(damage * skillParsent);
        ShowSlider();
    }

    /// <summary>�f�o�t�ƃ_���[�W���󂯂����̏���</summary>
    /// <param name="damage"></param>
    /// <param name="skillParsent"></param>
    /// <param name="attackDebuff"></param>
    /// <param name="diffenceDebuff"></param>
    public void AddDebuffDamage(float damage, float skillParsent = 1,int attackDebuff = 0,int diffenceDebuff = 0)
    {
        _hp = _hp - (int)(damage * skillParsent);
        ShowSlider();
        if (_attack >= _defaultAttack && attackDebuff != 0)
        {
            StartCoroutine(AttackBuffTIme(attackDebuff));
        }
        if(_diffence >= _defaultDiffence && diffenceDebuff != 0)
        {
            StartCoroutine(DiffenceBuffTIme(diffenceDebuff));
        }
    }

    /// <summary>�o�t���󂯂����̏���</summary>
    /// <param name="attackBuff"></param>
    /// <param name="diffenceBuff"></param>
    /// <param name="heal"></param>
    public void AddBuff(int attackBuff, int diffenceBuff,int heal)
    {
        if (_attack <= _defaultAttack && attackBuff != 0)
        {
            StartCoroutine(AttackBuffTIme(attackBuff));
        }
        if (_diffence <= _defaultDiffence && diffenceBuff != 0)
        {
            StartCoroutine(DiffenceBuffTIme(diffenceBuff));
        }
        if (heal != 0 && _hp > 0)
        {
            _hp = Mathf.Min(_defaultHP, _hp + heal);
            ShowSlider();
        }
    }

    /// <summary>HP�̃Q�[�W���X�V���鏈��</summary>
    public void ShowSlider()
    {
        _hpSlider.value = (float)_hp / (float)_defaultHP;
    }

    /// <summary>�U���̃o�t�A�f�o�t���󂯂����̃R���[�`��</summary>
    /// <param name="attackBuff"></param>
    /// <returns></returns>
    IEnumerator AttackBuffTIme(int attackBuff)
    {
        _attackBuffTime = 0;
        yield return null;
        _attack = Mathf.Max(1, _defaultAttack + attackBuff);
        Debug.Log(_attack);
        _attackBuffTime = 30f;
        _buffAttack = false;
        //������30�b�o���A�X�e�[�^�X�X�V(�f�o�t�Ȃ�o�t�A�o�t�Ȃ�f�o�t���󂯂���)�܂őҋ@����
        yield return new WaitUntil(() => _buffAttack == true);
        _attack = _defaultAttack;
        Debug.Log("�U����ԕω�����");
    }

    /// <summary>�h��̃o�t�A�f�o�t���󂯂����̃R���[�`��</summary>
    /// <param name="diffenceDebuff"></param>
    /// <returns></returns>
    IEnumerator DiffenceBuffTIme(int diffenceDebuff)
    {
        _diffenceBuffTime = 0;
        yield return null;
        _diffence = Mathf.Max(1, _defaultDiffence + diffenceDebuff);
        Debug.Log(_diffence);
        _diffenceBuffTime = 30f;
        _buffDiffence = false;
        //������30�b�o���A�X�e�[�^�X�X�V(�f�o�t�Ȃ�o�t�A�o�t�Ȃ�f�o�t���󂯂���)�܂őҋ@����
        yield return new WaitUntil(() => _buffDiffence == true);
        _diffence = _defaultDiffence;
        Debug.Log("�h���ԕω�����");
    }

    /// <summary>�����Ă��邩����ł��邩��state</summary>
    public enum Survive
    {
        Survive,
        Death,
    }
}
