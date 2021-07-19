using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaExample.Gateways.Messages
{
    public class NotificationMessage : MediatR.INotification
    {
        public string NotifyText { get; set; }
    }
}
