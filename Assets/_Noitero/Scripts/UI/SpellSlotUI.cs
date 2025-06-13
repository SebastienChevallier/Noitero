using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SpellSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image icon;
    [SerializeField] private Image bg;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform parentAfterDrag;
    private GameObject placeholder;

    private bool isHorizontal = true;

    private ISpellList listUI;
    private SpellBase spell;
    private int index;

    public void Setup(ISpellList list, SpellBase spell, int index)
    {
        listUI = list;
        this.index = index;
        this.spell = spell;
        if (label != null)
            label.text = spell.name;

        if(icon != null && spell.Icon != null)
        {
            icon.sprite = spell.Icon;
        }

        if(bg != null)
        {
            bg.color = spell.ColorType;
        }
    }

    public void UpdateIndex(int newIndex) => index = newIndex;

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

    private int GetTargetIndex(Transform parent, Vector3 position)
    {
        int targetIndex = parent.childCount;
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (isHorizontal)
            {
                if (position.x < child.position.x)
                {
                    targetIndex = i;
                    break;
                }
            }
            else
            {
                if (position.y > child.position.y)
                {
                    targetIndex = i;
                    break;
                }
            }
        }
        return targetIndex;
    }

    private ISpellList FindTargetList(PointerEventData eventData)
    {
        // Try raycast first
        if (eventData.pointerEnter != null)
        {
            var list = eventData.pointerEnter.GetComponentInParent<ISpellList>();
            if (list != null)
                return list;
        }

        // Fallback: check all lists by position
        foreach (var mb in Object.FindObjectsOfType<MonoBehaviour>())
        {
            if (mb is ISpellList list)
            {
                var rt = (mb.transform as RectTransform);
                if (rt != null && RectTransformUtility.RectangleContainsScreenPoint(rt, eventData.position, eventData.pressEventCamera))
                    return list;
            }
        }

        return null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;

        var targetList = FindTargetList(eventData);
        if (targetList == null)
            targetList = listUI;

        if (targetList == listUI)
        {
            transform.SetParent(parentAfterDrag);
            int newIndex = placeholder.transform.GetSiblingIndex();
            transform.SetSiblingIndex(newIndex);
            listUI.OnItemMoved(index, newIndex);
        }
        else
        {
            int newIndex = GetTargetIndex(targetList.Content, eventData.position);
            listUI.RemoveSpellAt(index);
            targetList.InsertSpellAt(spell, newIndex);
            Destroy(gameObject);
        }

        Destroy(placeholder);
    }
}
