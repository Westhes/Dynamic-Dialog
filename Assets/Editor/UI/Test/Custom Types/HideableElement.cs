using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

public class HideableElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<HideableElement, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        //private UxmlBoolAttributeDescription usePercentages = new() { name = "Use Percentages", defaultValue = true };
        private UxmlIntAttributeDescription minimumWidth = new() { name = "MinimumWidth", defaultValue = 50 };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as HideableElement;

            //ate.UsePercentage = usePercentages.GetValueFromBag(bag, cc);
            ate.MinimumWidth = minimumWidth.GetValueFromBag(bag, cc);
            ate.UsePercentage = false;
        }
    }

    private bool usePercentage;

    public bool UsePercentage
    {
        get => usePercentage;
        set
        {
            usePercentage = value;
            //this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedPercentage);
            this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedPixel);

            //if (value)
            //    this.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedPercentage);
            //else
                this.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedPixel);
        }
    }

    public int MinimumWidth { get; set; }

    private float MinimumParent { get; set; } = float.MinValue;

    //private void OnGeometryChangedPercentage(GeometryChangedEvent gCE)
    //{
    //    if (this.hierarchy.childCount > 0)
    //    {
    //        var child = this.hierarchy.Children().FirstOrDefault();
    //        //child.measure
    //        var childText = child as Label;
    //        Debug.Log(child);
    //    }

    //    if (this.resolvedStyle.width < MinimumWidth)
    //        this.AddToClassList("hide");
    //    else
    //        this.RemoveFromClassList("hide");
    //}

    private void OnGeometryChangedPixel(GeometryChangedEvent gCE)
    {
        //if (this.resolvedStyle.width < MinimumWidth)
        //{
        //    this.AddToClassList("hide");
        //    MinimumParent = parent.resolvedStyle.width > MinimumParent ? parent.resolvedStyle.width : MinimumParent;
        //    Debug.Log(MinimumParent);
        //}

        //if (parent.resolvedStyle.width > MinimumParent)
        //    this.RemoveFromClassList("hide");

        if (this.resolvedStyle.width < MinimumWidth)
            this.AddToClassList("hide");
        else
            this.RemoveFromClassList("hide");
    }
}
