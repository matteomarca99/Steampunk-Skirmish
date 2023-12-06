namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("Layout/Extensions/Radial Layout")]
    public class RadialLayout : LayoutGroup
    {
        public float fDistance;
        [Range(0f, 360f)]
        public float MinAngle, MaxAngle, StartAngle;
        public bool OnlyLayoutVisible = false;

        protected override void OnEnable() { base.OnEnable(); CalculateRadial(); }

        public override void SetLayoutHorizontal() { }
        public override void SetLayoutVertical() { }
        public override void CalculateLayoutInputVertical() { CalculateRadial(); }
        public override void CalculateLayoutInputHorizontal() { CalculateRadial(); }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            CalculateRadial();
        }
#endif

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        void CalculateRadial()
        {
            m_Tracker.Clear();
            if (transform.childCount == 0)
                return;

            int ChildrenToFormat = OnlyLayoutVisible ? CountVisibleChildren() : transform.childCount;

            float fOffsetAngle = (MaxAngle - MinAngle) / ChildrenToFormat;
            float fAngle = StartAngle;

            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if ((child != null) && (!OnlyLayoutVisible || child.gameObject.activeSelf))
                {
                    m_Tracker.Add(this, child,
                        DrivenTransformProperties.Anchors |
                        DrivenTransformProperties.AnchoredPosition |
                        DrivenTransformProperties.Pivot);

                    float rotation = -fAngle - 90f; // Calcola l'angolo di rotazione per la carta

                    // Imposta la posizione radiale per la carta senza considerare la distanza
                    Vector3 pos = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0);
                    child.localPosition = pos;

                    // Applica la rotazione
                    child.localRotation = Quaternion.Euler(0f, 0f, rotation);

                    fAngle += fOffsetAngle;
                }
            }

            // Utilizza fDistance come spaziatura tra le carte nel layout orizzontale
            float totalSpacing = fDistance * (transform.childCount - 1);
            float layoutWidth = rectTransform.rect.width - totalSpacing;
            float spacing = layoutWidth / (transform.childCount - 1);
            ((HorizontalOrVerticalLayoutGroup)GetComponent<HorizontalLayoutGroup>()).spacing = spacing;
        }



        int CountVisibleChildren()
        {
            int count = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if ((child != null) && child.gameObject.activeSelf)
                    ++count;
            }
            return count;
        }
    }
}
