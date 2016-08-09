using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Readit
{
    public interface IReaditTransportMethod
    {
       // public IReaditTransportMethod();

       event  EventHandler<EventArgs> eventHandler;

        string ConnectionID { get; set; }

        ReaditConnectionDetails ConnectionDetails { get; set; }

        bool Connected { get; }

        void Connect();

        void SetMessageDispatcher(object para1);

        void Send(string param1, string param2);

        void Close();

        void Subscribe(string param1);

        void UnSubscribe(string param1);

        void Dispose();

        void addEventListener();

        void dispatchEvent(EventArgs events);

        void removeAllEventListener();



    }
}
