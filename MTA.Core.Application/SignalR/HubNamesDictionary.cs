using System;
using System.Collections.Generic;

namespace MTA.Core.Application.SignalR
{
    public class HubNamesDictionary : Dictionary<Type, string>
    {
        private const string NotifierHub = "NOTIFIER";

        public static HubNamesDictionary Build() => new HubNamesDictionary
        {
            {typeof(NotifierHub), NotifierHub}
        };
    }
}