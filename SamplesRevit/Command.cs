using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net.Security;
using Autodesk.Revit.Attributes;

namespace RevitSamples
{
    [Transaction(TransactionMode.ReadOnly)]
    class Command : IExternalCommand 
    {

        //NetTcpBindind Безопасная и надежная привязка, которая подходит для обмена данными между компьютерами.
        private static NetTcpBinding NewBinding()
        {
            return new NetTcpBinding(SecurityMode.None)
            {
                ReliableSession = { InactivityTimeout = TimeSpan.MaxValue, Enabled = false },
                ReceiveTimeout = TimeSpan.MaxValue,
                MaxConnections = 1000,
                MaxReceivedMessageSize = (long)Math.Pow(2.0, 30.0),
                ReaderQuotas =
                {
                    MaxArrayLength = (int)Math.Pow(2.0, 30.0),
                    MaxBytesPerRead = (int)Math.Pow(2.0, 30.0),
                    MaxDepth = 32,
                    MaxNameTableCharCount = 16384,
                    MaxStringContentLength = (int)Math.Pow(2.0, 30.0)
                },
                Security =
                {
                    Mode = SecurityMode.None,
                    Transport =
                    {
                        ProtectionLevel = ProtectionLevel.None,
                        ClientCredentialType = TcpClientCredentialType.None
                    }
                }
            };
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var userMessage = GetMessage();
            SendToPilot(userMessage);
            return Result.Succeeded;
        }

        private void SendToPilot(string userMessage)
        {
            var binding = NewBinding();
            var epa = new EndpointAddress(new Uri("net.tcp://localhost:8002/rvtconnect/"));
            var message = new MessageResponse { Message = userMessage};
            try
            {
                ChannelFactory<IRevitConnectionService> clientFactory = new ChannelFactory<IRevitConnectionService>();
                clientFactory.Endpoint.Address = epa;
                clientFactory.Endpoint.Binding = binding;
                clientFactory.Open();
                var channel = clientFactory.CreateChannel();
                (channel as IContextChannel).OperationTimeout = new TimeSpan(0, 20, 0);
                channel.CreateMessageObject(message);
                clientFactory.Close();
            }
            catch (Exception)
            {
                TaskDialog.Show("ОШИБКА", "Ошибка!");
            }
        }

        private string GetMessage()
        {
            return Microsoft.VisualBasic.Interaction.InputBox("Пишите здесь!", DefaultResponse: "No response");
        }
    }
}
