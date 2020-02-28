using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.ServiceModel;
using Ascon.Pilot.SDK;
using System.ComponentModel.Composition;
using System.ServiceModel.Description;

namespace PilotSamples
{
    [Export(typeof(IDataPlugin))]
    public class Main : IDataPlugin
    {
        private ServiceHost ServiceHost = null;
        private readonly IPilotDialogService _dialogService;

        [ImportingConstructor]
        public Main(IPilotDialogService dialogService)
        {
            _dialogService = dialogService;
            CreateHost();
        }

        private void CreateHost()
        {
            try
            {
                var newTcpBinding = NewBinding();

                var uri = new Uri("net.tcp://localhost:8002/rvtconnect/");
                ServiceHost = new ServiceHost(typeof(RevitConnectionService), uri);
                var addres = new EndpointAddress(uri);
                var serviceDebugBehavior = ServiceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>() ?? new ServiceBehaviorAttribute();
                serviceDebugBehavior.MaxItemsInObjectGraph = 2147483647;
                var serviceThrottlongBehavior = new ServiceThrottlingBehavior();
                serviceThrottlongBehavior.MaxConcurrentCalls = 1000;
                serviceThrottlongBehavior.MaxConcurrentInstances = 2147483647;
                serviceThrottlongBehavior.MaxConcurrentSessions = 1000;
                ServiceHost.Description.Behaviors.Add(serviceThrottlongBehavior);
                var serviceEndpoint = ServiceHost.AddServiceEndpoint("PilotSamples.IRevitConnectionService", newTcpBinding, "");
                serviceEndpoint.Address = addres;
                serviceEndpoint.Binding = newTcpBinding;

                ServiceHost.OpenTimeout = TimeSpan.FromMinutes(1.0);
                ServiceHost.CloseTimeout = TimeSpan.FromMinutes(1.0);
                ServiceHost.Open();
                _dialogService.ShowBalloon("Успешно", "Сервер готов", PilotBalloonIcon.Info);

            }
            catch (Exception ex)
            {
                _dialogService.ShowBalloon("Exception", "Exception", PilotBalloonIcon.Error);
            }
        }

        private static NetTcpBinding NewBinding()
        {
            return new NetTcpBinding(SecurityMode.None)
            {
                ReliableSession = { InactivityTimeout = TimeSpan.FromDays(1.0), Enabled = false },
                OpenTimeout = TimeSpan.FromMinutes(1.0),
                ReceiveTimeout = TimeSpan.FromDays(1.0),
                SendTimeout = TimeSpan.FromDays(1.0),
                MaxConnections = 1000,
                MaxReceivedMessageSize = (int)Math.Pow(2.0, 30.0),
                ReaderQuotas =
                    {
                        MaxArrayLength = (int) Math.Pow(2.0, 30.0),
                        MaxBytesPerRead = (int) Math.Pow(2.0, 30.0),
                        MaxDepth = 32,
                        MaxNameTableCharCount = 16384,
                        MaxStringContentLength = (int) Math.Pow(2.0, 30.0)
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
    }
}
