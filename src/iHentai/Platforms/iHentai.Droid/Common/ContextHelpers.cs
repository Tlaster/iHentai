using Android.Content;
using Android.Util;

namespace iHentai.Droid.Common
{
    internal class ContextHelpers
    {
        public static int Dp2Px(int dp, Context context)
        {
            return (int) TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }
}