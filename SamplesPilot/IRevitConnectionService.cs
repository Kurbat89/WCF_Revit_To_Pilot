using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace PilotSamples
{
    [ServiceContract(Namespace = "http://RevitSamples", SessionMode = SessionMode.Allowed)]
    interface IRevitConnectionService
    {
        [OperationContract]
        void CreateMessageObject(MessageResponse response);
    }
}
