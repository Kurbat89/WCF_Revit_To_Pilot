﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RevitSamples
{
    [DataContract(Name = nameof(MessageResponse), Namespace = "http://schemas.datacontract.org/2004/07/RevitSamples")]
    [Serializable]
    public class MessageResponse : IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}
