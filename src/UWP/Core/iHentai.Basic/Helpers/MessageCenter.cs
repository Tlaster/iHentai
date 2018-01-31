using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace iHentai.Basic.Helpers
{
    public class MessagingCenter
    {
        private readonly Dictionary<Sender, List<Subscription>> _subscriptions =
            new Dictionary<Sender, List<Subscription>>();
       

        public void Send<TArgs>(string message, TArgs args)
        {
            InnerSend(message, typeof(TArgs), args);
        }

        public void Send(string message)
        {
            InnerSend(message, null, null);
        }

        public void Subscribe<TSender, TArgs>(object subscriber, string message,
            Action<TSender, TArgs> callback)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var target = callback.Target;
            
            InnerSubscribe(subscriber, message, typeof(TArgs), target, callback.GetMethodInfo());
        }

        public void Subscribe(object subscriber, string message, Action callback)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var target = callback.Target;
            

            InnerSubscribe(subscriber, message, null, target, callback.GetMethodInfo());
        }

        public void Unsubscribe<TArgs>(object subscriber, string message)
        {
            InnerUnsubscribe(message, typeof(TArgs), subscriber);
        }

        public void Unsubscribe(object subscriber, string message)
        {
            InnerUnsubscribe(message, null, subscriber);
        }

        private void InnerSend(string message, Type argType, object args)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            var key = new Sender(message, argType);
            if (!_subscriptions.ContainsKey(key))
                return;
            var subcriptions = _subscriptions[key];
            if (subcriptions == null || !subcriptions.Any())
                return; // should not be reachable

            // ok so this code looks a bit funky but here is the gist of the problem. It is possible that in the course
            // of executing the callbacks for this message someone will subscribe/unsubscribe from the same message in
            // the callback. This would invalidate the enumerator. To work around this we make a copy. However if you unsubscribe 
            // from a message you can fairly reasonably expect that you will therefor not receive a call. To fix this we then
            // check that the item we are about to send the message to actually exists in the live list.
            var subscriptionsCopy = subcriptions.ToList();
            foreach (var subscription in subscriptionsCopy)
                if (subscription.Subscriber.Target != null && subcriptions.Contains(subscription))
                    subscription.InvokeCallback(args);
        }

        private void InnerSubscribe(object subscriber, string message, Type argType, object target,
            MethodInfo methodInfo)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            var key = new Sender(message, argType);
            var value = new Subscription(subscriber, target, methodInfo);
            if (_subscriptions.ContainsKey(key))
            {
                _subscriptions[key].Add(value);
            }
            else
            {
                var list = new List<Subscription> {value};
                _subscriptions[key] = list;
            }
        }

        private void InnerUnsubscribe(string message, Type argType, object subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var key = new Sender(message, argType);
            if (!_subscriptions.ContainsKey(key))
                return;
            _subscriptions[key].RemoveAll(sub => sub.CanBeRemoved() || sub.Subscriber.Target == subscriber);
            if (!_subscriptions[key].Any())
                _subscriptions.Remove(key);
        }

        // This is a bit gross; it only exists to support the unit tests in PageTests
        // because the implementations of ActionSheet, Alert, and IsBusy are all very
        // tightly coupled to the MessagingCenter singleton 
        internal void ClearSubscribers()
        {
            _subscriptions.Clear();
        }

        private class Sender : Tuple<string, Type, Window>
        {
            public Sender(string message, Type argType) : base(message, argType, Window.Current)
            {
            }
        }

        private class MaybeWeakReference
        {
            private readonly bool _isStrongReference;

            public MaybeWeakReference(object subscriber, object delegateSource)
            {
                if (subscriber.Equals(delegateSource))
                {
                    // The target is the subscriber; we can use a weakreference
                    DelegateWeakReference = new WeakReference(delegateSource);
                    _isStrongReference = false;
                }
                else
                {
                    DelegateStrongReference = delegateSource;
                    _isStrongReference = true;
                }
            }

            private WeakReference DelegateWeakReference { get; }
            private object DelegateStrongReference { get; }

            public object Target => _isStrongReference ? DelegateStrongReference : DelegateWeakReference.Target;
            public bool IsAlive => _isStrongReference || DelegateWeakReference.IsAlive;
        }

        private class Subscription : Tuple<WeakReference, MaybeWeakReference, MethodInfo>
        {
            public Subscription(object subscriber, object delegateSource, MethodInfo methodInfo)
                : base(new WeakReference(subscriber), new MaybeWeakReference(subscriber, delegateSource), methodInfo)
            {
            }

            public WeakReference Subscriber => Item1;
            private MaybeWeakReference DelegateSource => Item2;
            private MethodInfo MethodInfo => Item3;

            public void InvokeCallback(object args)
            {
                if (MethodInfo.IsStatic)
                {
                    MethodInfo.Invoke(null,
                        MethodInfo.GetParameters().Length == 0 ? null : new[] {args});
                    return;
                }

                var target = DelegateSource.Target;

                if (target == null) return; // Collected 
                var a = MethodInfo.GetParameters().Length;
                MethodInfo.Invoke(target,
                    MethodInfo.GetParameters().Length == 0 ? null : new[] {args});
            }

            public bool CanBeRemoved()
            {
                return !Subscriber.IsAlive || !DelegateSource.IsAlive;
            }
        }
    }
}