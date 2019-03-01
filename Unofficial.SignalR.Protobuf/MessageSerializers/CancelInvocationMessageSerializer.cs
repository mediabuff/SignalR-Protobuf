﻿using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Microsoft.AspNetCore.SignalR.Protocol;
using Unofficial.SignalR.Protobuf.MessageSerializers.Base;

namespace Unofficial.SignalR.Protobuf.MessageSerializers
{
    internal class CancelInvocationMessageSerializer : BaseMessageSerializer
    {
        public override ProtobufMessageType EnumType => ProtobufMessageType.CancelInvocation;
        public override Type MessageType => typeof(CancelInvocationMessage);

        protected override IEnumerable<IMessage> CreateProtobufModels(HubMessage message)
        {
            var cancelInvocationMessage = (CancelInvocationMessage) message;

            yield return new CancelInvocationMessageProtobuf
            {
                InvocationId = cancelInvocationMessage.InvocationId,
                Headers = { cancelInvocationMessage.Headers.Flatten() }
            };
        }

        protected override HubMessage CreateHubMessage(IReadOnlyList<IMessage> protobufModels)
        {
            var protobuf = (CancelInvocationMessageProtobuf) protobufModels.Single();
            
            return new CancelInvocationMessage(protobuf.InvocationId)
            {
                Headers = protobuf.Headers.Unflatten()
            };
        }
    }
}