using System.Collections.Generic;
using Windows.UI.Xaml;

namespace iHentai.Paging
{
    public static class HentaiSuspensionManager
    {
        private static readonly DependencyProperty FrameSessionStateKeyProperty =
            DependencyProperty.RegisterAttached("_FrameSessionStateKey", typeof(string),
                typeof(HentaiSuspensionManager),
                null);

        private static readonly DependencyProperty FrameSessionStateProperty =
            DependencyProperty.RegisterAttached("_FrameSessionState", typeof(Dictionary<string, object>),
                typeof(HentaiSuspensionManager), null);

        public static Dictionary<string, object> SessionState { get; } = new Dictionary<string, object>();

        public static Dictionary<string, object> SessionStateForFrame(HentaiFrame frame)
        {
            var frameState = (Dictionary<string, object>) frame.GetValue(FrameSessionStateProperty);
            if (frameState == null)
            {
                var frameSessionKey = (string) frame.GetValue(FrameSessionStateKeyProperty);
                if (frameSessionKey != null)
                {
                    if (!SessionState.ContainsKey(frameSessionKey))
                        SessionState[frameSessionKey] = new Dictionary<string, object>();
                    frameState = (Dictionary<string, object>) SessionState[frameSessionKey];
                }
                else
                {
                    frameState = new Dictionary<string, object>();
                }
                frame.SetValue(FrameSessionStateProperty, frameState);
            }
            return frameState;
        }
    }
}