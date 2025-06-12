using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Text label;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform parentAfterDrag;
    private GameObject placeholder;

    private bool isHorizontal = true;

    private SpellListUI listUI;
    private int index;

    public void Setup(SpellListUI list, SpellBase spell, int index)
    {
        listUI = list;
        this.index = index;
        if (label != null)
            label.text = spell.name;
    }

    public void UpdateIndex(int newIndex)
    {
        index = newIndex;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;

        parentAfterDrag = transform.parent;
        placeholder = new GameObject("placeholder");
        var layout = placeholder.AddComponent<LayoutElement>();
        layout.preferredWidth = rectTransform.rect.width;
        layout.preferredHeight = rectTransform.rect.height;
        placeholder.transform.SetParent(parentAfterDrag);
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(parentAfterDrag.parent);


        // detect layout orientation (horizontal vs vertical)
        if (parentAfterDrag.childCount >= 2)
        {
            Vector3 diff = parentAfterDrag.GetChild(1).position - parentAfterDrag.GetChild(0).position;
            isHorizontal = Mathf.Abs(diff.x) > Mathf.Abs(diff.y);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform != null)
            rectTransform.position = eventData.position;


        int targetIndex = parentAfterDrag.childCount;
        for (int i = 0; i < parentAfterDrag.childCount; i++)
        {
            var child = parentAfterDrag.GetChild(i);
            if (isHorizontal)
            {
                if (rectTransform.position.x < child.position.x)
                {
                    targetIndex = i;
                    break;
                }
            }
            else
            {
                if (rectTransform.position.y > child.position.y)
                {
                    targetIndex = i;
                    break;
                }
            }
        }

        placeholder.transform.SetSiblingIndex(targetIndex);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;

        transform.SetParent(parentAfterDrag);
        int newIndex = placeholder.transform.GetSiblingIndex();
        transform.SetSiblingIndex(newIndex);

        listUI.OnItemMoved(index, newIndex);

        Destroy(placeholder);
    }
}
