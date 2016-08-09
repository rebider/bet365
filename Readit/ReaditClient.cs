using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.Readit
{
    public class ReaditClient : FNet.Events.EventDispatcher
    {
      //  private ReaditConnectionStatus _reportingStatus = 0;
        private int _reportingStatus = 0;
        private ReaditConnectionDetails currentConnectionDetails;
        private int connectionCycleAttempts;
        private ReaditConnectionStatus _connectionStatus;
        public int connectionListCycles;
        private ReaditConnectionDetails connectionListEnd;
        private ReaditConnectionDetails connectionListStart;
        private UInt32 instanceid;
        private IReaditTransportMethod currentTransportMethod;
        //private EventHandler<EventArgs> currentTransportMethod;
        private static uint count = 0;


        public ReaditClient()
        {
            instanceid = count + 1;
            //super(this);

            
        }

        public int ReportingStatus { get { return _reportingStatus; } }

        public void send(string param1, string param2)
        {
            if (_connectionStatus == ReaditConnectionStatus.CONNECTED)
            {
                currentTransportMethod.Send(param1, param2);
            }
        }

        public bool Connected { get { return _connectionStatus == ReaditConnectionStatus.CONNECTED; } }

        public void subscribe(string param1)
        {
            if (_connectionStatus == ReaditConnectionStatus.CONNECTED)
            {
                currentTransportMethod.Subscribe(param1);
            }
        }


        public void unsubscribe(string param1)
        {
            if (_connectionStatus == ReaditConnectionStatus.CONNECTED)
            {
                currentTransportMethod.UnSubscribe(param1);
            }
        }

        public void connectToNext()
        {
            var _loc_1 = currentConnectionDetails.next;
            currentConnectionDetails = currentConnectionDetails.next;

            if (null == _loc_1)
            {
                if (connectionListCycles > 0 && connectionCycleAttempts++ == connectionListCycles)
                {
                    _reportingStatus = 4;
                    //TODO:      dispatchEvent(new ReaditConnectionEvent(ReaditConnectionEvent.CONNECTION_FAILED));
                    //      dispatchEvent(new ReaditConnectionEvent(ReaditConnectionEvent.CONNECTION_FAILED));
                    //     return;
                }
                currentConnectionDetails = connectionListStart;
            }
            else
            {
                _reportingStatus = 3;
            }
            disposeCurrentTransportMethod();
            //currentTransportMethod =new  currentConnectionDetails.transportMethod;
            currentTransportMethod.removeAllEventListener();
            currentTransportMethod.eventHandler += new EventHandler<EventArgs>(currentTransportMethod_eventHandler);



        }

        void currentTransportMethod_eventHandler(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (e is Event.TransportConnectionEvent)
            {
                Event.TransportConnectionEvent tce = (Event.TransportConnectionEvent)e;

                if (Event.TransportConnectionEvent.CONNECTED == tce.EventMsg)
                {
                }
                else if (Event.TransportConnectionEvent.CONNECTION_FAILED == tce.EventMsg)
                {
                }
                else if (Event.TransportConnectionEvent.DISCONNECTED == tce.EventMsg)
                {
                    transportDisconnectedHandler();
                }

            }


        }

        private void transportConnectedHandler()
        {
            var _loc_2 = ReaditConnectionStatus.CONNECTED;
            _connectionStatus = ReaditConnectionStatus.CONNECTED;
            _reportingStatus =(int) _loc_2;



        }

        private void transportConnectionFailedHandler()
        {
            connectToNext();
            return;
        }


        //private void transportDisconnectedHandler(object othermgr)
        private void transportDisconnectedHandler()
        {
            close();
            var _loc_2 = ReaditConnectionStatus.DISCONNECTED;
            _connectionStatus = ReaditConnectionStatus.DISCONNECTED;
            _reportingStatus = (int) _loc_2;

            //TODO: dispatchEvent(new ReaditConnectionEvent(ReaditConnectionEvent.DISCONNECTED));
            //dispatchEvent(new ReaditConnectionEvent(ReaditConnectionEvent.DISCONNECTED));

        }

        public ReaditConnectionStatus ConnectionStatus { get { return _connectionStatus; } }



        public void close()
        {
            if (null != currentTransportMethod)
            {
                disposeCurrentTransportMethod();
                currentTransportMethod.Close();
            }


        }

        public string ConnectionID { get { return _connectionStatus == ReaditConnectionStatus.CONNECTED ? (currentTransportMethod.ConnectionID) : (null); } }


        private void disposeCurrentTransportMethod()
        {
            if (null != currentTransportMethod)
            {
                currentTransportMethod.Dispose();
                //TODO: 这里的事件委托还没有实现
                /*
                currentTransportMethod.removeEventListener(TransportConnectionEvent.CONNECTED, transportConnectedHandler);
                currentTransportMethod.removeEventListener(TransportConnectionEvent.CONNECTION_FAILED, transportConnectionFailedHandler);
                curentTransportMethod.removeEventListener(TransportConnectionEvent.DISCONNECTED, transportDisconnectedHandler);
                 */ 
            }

        }


    }
}
