using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RevitSamples
{
    [ServiceContract(Namespace = "http://RevitSamples", ConfigurationName = nameof(IRevitConnectionService))]
    interface IRevitConnectionService
    {
        [OperationContract(Action = "http://RevitSamples/IRevitConnectionService/CreateMessageObject"
            , ReplyAction = "http://RevitSamples/IRevitConnectionService/CreateMessageObjectResponse")]
        void CreateMessageObject(MessageResponse response);
    }
}
