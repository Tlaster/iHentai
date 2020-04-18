using System;
using System.Collections.Generic;
using System.Linq;

namespace iHentai.Controls.Paging
{
    internal class ActivityStackManager
    {
        private readonly List<ActivityModel> _activities = new List<ActivityModel>();

        public int CurrentIndex { get; private set; } = -1;

        public ActivityModel CurrentActivity => _activities.ElementAtOrDefault(CurrentIndex);

        public bool CanGoBack => CurrentIndex > 0;

        public IReadOnlyList<ActivityModel> Activities => _activities;

        public int BackStackDepth => CurrentIndex + 1;

        public event Action BackStackChanged;

        public ActivityModel GetActivityAt(int index)
        {
            return _activities[index];
        }

        public bool RemoveActivity(Activity activity)
        {
            if (activity == CurrentActivity.Activity)
            {
                throw new ArgumentException("The current activity cannot be removed from the stack. ");
            }

            var index = _activities.FindIndex(it => it.Activity == activity);
            if (index == -1)
            {
                throw new ArgumentException("Can not find activity from the back stack");
            }

            return RemoveActivityAt(index);
        }

        public bool RemoveActivityAt(int index)
        {
            if (index == CurrentIndex)
                throw new ArgumentException("The current activity cannot be removed from the stack. ");

            _activities.RemoveAt(index);
            if (index < CurrentIndex) CurrentIndex--;
            BackStackChanged?.Invoke();
            return true;
        }

        public void ClearBackStack()
        {
            for (var i = CurrentIndex - 1; i >= 0; i--)
                RemoveActivityAt(i);
        }

        public void ClearForwardStack()
        {
            for (var i = _activities.Count - 1; i > CurrentIndex; i--)
                RemoveActivityAt(i);
        }

        public void ChangeCurrentActivity(ActivityModel newActivity, int nextIndex)
        {
            if (_activities.Count <= nextIndex) _activities.Add(newActivity);
            CurrentIndex = nextIndex;
            BackStackChanged?.Invoke();
        }

        public bool CanGoBackTo(int newIndex)
        {
            if (newIndex == CurrentIndex) return false;

            return newIndex >= 0 && newIndex <= CurrentIndex;
        }
    }
}