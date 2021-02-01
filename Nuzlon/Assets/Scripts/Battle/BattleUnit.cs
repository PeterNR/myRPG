using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField]
    private BaseCreature _base;
    [SerializeField]
    private int _level;
    [SerializeField]
    private bool _isPlayerUnit;

    public Creature BattleCreature { get; set; }

    private Image _image;
    private Vector3 _originalPos;
    private Color _originalColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _originalPos = _image.transform.localPosition;
        _originalColor = _image.color;
    }

    public void Setup()
    {
        BattleCreature = new Creature(_base, _level);
        if(_isPlayerUnit)
        {
            _image.sprite = BattleCreature.Base.BackSprite;
        }
        else
        {
            _image.sprite = BattleCreature.Base.FrontSprite;
        }

        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if(_isPlayerUnit)
        {
            _image.transform.localPosition = new Vector3 (-500, _originalPos.y);
        }
        else
        {
            _image.transform.localPosition = new Vector3(500, _originalPos.y);
        }

        _image.transform.DOLocalMoveX(_originalPos.x, 1f);
    }
    public void PlayAttackAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        if(_isPlayerUnit)
        {
            sequence.Append(_image.transform.DOLocalMoveX(_originalPos.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append( _image.transform.DOLocalMoveX(_originalPos.x - 50f, 0.25f));
        }
        sequence.Append(_image.transform.DOLocalMoveX(_originalPos.x, 0.25f));
        sequence.Play();
    }

    public void PlayHitAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_image.DOColor(Color.gray, 0.1f));
        sequence.Append(_image.DOColor(_originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_image.transform.DOLocalMoveY(_originalPos.y - 150f, 0.5F));
        sequence.Join(_image.DOFade(0f, 0.5f));
    }
}
