using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conet.Apis.Core
{
    public enum ActionTypes
    {
        Account,
        Detail,
        Follower,
        Following,
        Custom
    }

    public class ConetActionArgs
    {
        public const string ConetAction = nameof(ConetAction);

        public ConetActionArgs(ActionTypes action, object param)
        {
            Action = action;
            Param = param;
        }

        public ActionTypes Action { get; }

        public object Param { get; }
    }
}
