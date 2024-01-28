using System.Collections.Generic;
using UnityEngine;

public class BulletPoolActive : MonoBehaviour
{
    [Tooltip("�v�[������I�u�W�F�N�g�B")]
    [SerializeField] GameObject _poolObj;
    [Tooltip("���������������郊�X�g")]
    List<GameObject> _poolObjects = new List<GameObject>();
    [Tooltip("�ŏ�(Awake��)�ɐ������鋅�̐�")]
    [SerializeField] int _maxCount = 20;
    [Tooltip("�������������܂Ƃ߂�ꏊ")]
    [SerializeField] GameObject Parent;

    private void Awake()
    {
        //Awake�Ńv�[�������B
        Pool();
    }

    /// <summary>�ŏ��ɋ��𕡐��������āA�v�[�����Ă������\�b�h</summary>
    private void Pool()
    {
        //_maxCount��Afor�����񂷁B
        for (int i = 0; i < _maxCount; i++)
        {
            var newObj = CreateNewBullet(); //�V�����������B
            newObj.SetActive(false);
            if (Parent) newObj.transform.parent = Parent.transform; //Hielarcey���Y��ɂ������̂ň�x������̐e�I�u�W�F�N�g�̎q�ɂ����B
            _poolObjects.Add(newObj); //�������X�g�ɒǉ��B
        }
    }

    /// <summary>�V�������𐶐����郁�\�b�h</summary>
    /// <returns>������������Ԃ��B</returns>
    private GameObject CreateNewBullet()
    {
        var posistion = new Vector2(100, -100);
        var newObj = Instantiate(_poolObj, posistion, Quaternion.identity); //�w��̃|�W�V�����ɃI�u�W�F�N�g�𐶐��B
        newObj.name = _poolObj.name + (_poolObjects.Count + 1); // ���O�����Ȃ��悤�ɖ����̐�����ς���B

        return newObj;
    }

    /// <summary>���g�p�̋��̕������Z��true�ɂ��ĕԂ����\�b�h����S�Ďg���Ă�����V��������ĕԂ�</summary>
    /// <returns>���g�p�̋�or�V�����������</returns>
    public GameObject GetBullet()
    {
        //�g�p���łȂ����̂�T���ĕԂ��B
        foreach (var go in _poolObjects)
        {
            if (go.activeSelf == false)
            {
                go.SetActive(true);
                return go;
            }
        }
        //�S�Ďg�p����������V�������A���X�g�ɒǉ����Ă���Ԃ��B
        var newObj = CreateNewBullet();
        Debug.Log("�������܂����B");
        _poolObjects.Add(newObj);
        if (Parent) newObj.transform.parent = Parent.transform; //Hielarcey���Y��ɂ������̂ň�x������̐e�I�u�W�F�N�g�̎q�ɂ����B
        newObj.SetActive(true);
        return newObj;
    }
}
