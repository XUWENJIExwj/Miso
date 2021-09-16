using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MapZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler, IDragHandler
{
	[Header("�g��k���̑��x")]
	public float zoomSpeed = .5f;

	private bool _zoomEnable;
	private bool _dragEnable;

	// ������Ԃ̍��W�ƃX�P�[��
	private Vector2 initPos;
	private Vector2 initScale;

	void Start()
	{
		// ������Ԃ̍��W�ƃX�P�[����ێ�
		initPos = transform.localPosition;
		initScale = transform.localScale;
	}

	void Update()
	{
		// ���Z�b�g
		if (Input.GetKeyDown("r"))
		{
			transform.localPosition = initPos;
			transform.localScale = Vector2.one;
		}
	}

	/// <summary>
	/// MouesOn Handler
	/// /// </summary>
	/// <param name="eventData"></param>
	public void OnPointerEnter(PointerEventData e)
	{
		_zoomEnable = true;
	}

	/// <summary>
	/// MouseOut Handler
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerExit(PointerEventData e)
	{
		_zoomEnable = false;
	}

	/// <summary>
	/// WheelScroll Handler
	/// </summary>
	/// <param name="eventData"></param>
	public void OnScroll(PointerEventData e)
	{
		if (_zoomEnable)
			zoom();
	}

	/// <summary>
	/// PointerDrag Handler
	/// </summary>
	/// <param name="e"></param>
	public void OnDrag(PointerEventData e)
	{
		Vector3 p = gameObject.GetComponent<RectTransform>().position + (Vector3)e.delta;
		gameObject.GetComponent<RectTransform>().position = p;
	}


	// ���̃R���|�[�l���g���A�^�b�`���ꂽ UI Image ���g��k��
	private void zoom()
	{
		float val = Input.GetAxis("Mouse ScrollWheel");

		if (val == 0)
		{
			return;
		}

		// �Y�[���O�̍��W�ƃX�P�[��
		Vector2 pastPos = transform.localPosition;
		Vector2 pastScale = transform.localScale;
		
		// �Y�[������X�P�[�����v�Z
		Vector2 scale = transform.localScale * (1 + val * zoomSpeed);

		// �{���P�ȉ��ɂ͂��Ȃ�
		if (scale.x < 1)
		{
			return;
		}

		// �Y�[���v�Z��̃X�P�[����K�p
		transform.localScale = scale;

		// �J�[�\���𒆐S�ɃY�[������悤�ɃI�t�Z�b�g���W���v�Z
		Vector2 ofsetPos = new Vector2(cursorPotision().x * (scale.x - pastScale.x), cursorPotision().y * (scale.y - pastScale.y));

		// �I�t�Z�b�g���W��K�p
		transform.localPosition = -ofsetPos + pastPos;
	}

	// ���̃R���|�[�l���g���A�^�b�`���ꂽ UI Image ��̃}�E�X�J�[�\���̍��W���擾
	private Vector2 cursorPotision()
	{
		var canvas = gameObject.GetComponentInParent<Canvas>();
		var imRect = gameObject.GetComponent<RectTransform>();

		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(imRect, Input.mousePosition, canvas.worldCamera, out pos);

		return pos;
	}
}