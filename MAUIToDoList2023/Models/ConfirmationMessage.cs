using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIToDoList2023.Models
{
    public class ConfirmationMessage : ValueChangedMessage<string>
    {
        public ConfirmationMessage(string value) : base(value) { }
    }
}
