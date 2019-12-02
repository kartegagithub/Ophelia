﻿using Ophelia.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Mobile.Notification.iOS
{
    public class Notifier : Mobile.Notification.Notifier
    {
        public Notifier()
        {

        }
        public ServiceObjectResult<bool> Send(string certificatePath, string certificatePassword, long UserID, string deviceToken, string Message, string Title, string Reference = "", string Action = "", bool SandboxMode = false)
        {
            var notification = new PushNotification();
            var list = new Dictionary<long, string>() ;
            list.Add(UserID, deviceToken);
            return notification.SendAPSNNotification(SandboxMode, certificatePath, certificatePassword, list, Title, Message, Reference, Action);
        }
        public ServiceObjectResult<bool> Send(string certificatePath, string certificatePassword, Dictionary<long, string> Users, string Message, string Title, string Reference = "", string Action = "", bool SandboxMode = false)
        {
            var notification = new PushNotification();
            return notification.SendAPSNNotification(SandboxMode, certificatePath, certificatePassword, Users, Title, Message, Reference, Action);
        }
    }
}
