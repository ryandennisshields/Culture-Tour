using UnityEngine;

namespace GCU.CultureTour
{
    public static class ColorHelpers
    {
        public static Color LerpColor(Color a, Color b, float t)
        {
            Color c;

            c.r = Mathf.Lerp(a.r, b.r, t);
            c.g = Mathf.Lerp(a.g, b.g, t);
            c.b = Mathf.Lerp(a.b, b.b, t);
            c.a = Mathf.Lerp(a.a, b.a, t);

            return c;
        }
    }
}