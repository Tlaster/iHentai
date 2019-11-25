using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace iHentai.Common
{
    internal class BroadcastCenter
    {
        private readonly ConcurrentDictionary<Guid, (string message, Action<object, object> action)> _listeners =
            new ConcurrentDictionary<Guid, (string message, Action<object, object> action)>();

        private readonly List<(string message, object sender, object args)> _pendingMessages =
            new List<(string message, object sender, object args)>();

        public void Send(object sender, string message, object args = null)
        {
            foreach (var item in _listeners.Where(it => it.Value.message == message))
            {
                item.Value.action.Invoke(sender, args);
            }
        }

        public void SendWithPendingMessage(object sender, string message, object args = null)
        {
            var listeners = _listeners.Where(it => it.Value.message == message).ToList();
            if (listeners.Any())
            {
                foreach (var item in listeners)
                {
                    item.Value.action.Invoke(sender, args);
                }
            }
            else
            {
                _pendingMessages.Add((message, sender, args));
            }
        }

        public Guid SubscribeWithPendingMessage(string message, Action<object, object> action)
        {
            var pendingMessages = _pendingMessages.Where(it => it.message == message);
            foreach (var item in pendingMessages)
            {
                action.Invoke(item.sender, item.args);
            }

            var id = Guid.NewGuid();
            _listeners.TryAdd(id, (message, action));
            return id;
        }

        public Guid Subscribe(string message, Action<object, object> action)
        {
            var id = Guid.NewGuid();
            _listeners.TryAdd(id, (message, action));
            return id;
        }

        public void Unsubscribe(Guid id)
        {
            _listeners.TryRemove(id, out _);
        }
    }
}