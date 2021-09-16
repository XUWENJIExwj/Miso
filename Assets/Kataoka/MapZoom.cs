using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MapZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler, IDragHandler
{
	[Header("拡大縮小の速度")]
	public float zoomSpeed = .5f;

	private bool _zoomEnable;
	private bool _dragEnable;

	// 初期状態の座標とスケール
	private Vector2 initPos;
	private Vector2 initScale;

	void Start()
	{
		// 初期状態の座標とスケールを保持
		initPos = transform.localPosition;
		initScale = transform.localScale;
	}

	void Update()
	{
		// リセット
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


	// このコンポーネントがアタッチされた UI Image を拡大縮小
	private void zoom()
	{
		float val = Input.GetAxis("Mouse ScrollWheel");

		if (val == 0)
		{
			return;
		}

		// ズーム前の座標とスケール
		Vector2 pastPos = transform.localPosition;
		Vector2 pastScale = transform.localScale;
		
		// ズームするスケールを計算
		Vector2 scale = transform.localScale * (1 + val * zoomSpeed);

		// 倍率１以下にはしない
		if (scale.x < 1)
		{
			return;
		}

		// ズーム計算後のスケールを適用
		transform.localScale = scale;

		// カーソルを中心にズームするようにオフセット座標を計算
		Vector2 ofsetPos = new Vector2(cursorPotision().x * (scale.x - pastScale.x), cursorPotision().y * (scale.y - pastScale.y));

		// オフセット座標を適用
		transform.localPosition = -ofsetPos + pastPos;
	}

	// このコンポーネントがアタッチされた UI Image 上のマウスカーソルの座標を取得
	private Vector2 cursorPotision()
	{
		var canvas = gameObject.GetComponentInParent<Canvas>();
		var imRect = gameObject.GetComponent<RectTransform>();

		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(imRect, Input.mousePosition, canvas.worldCamera, out pos);

		return pos;
	}
}