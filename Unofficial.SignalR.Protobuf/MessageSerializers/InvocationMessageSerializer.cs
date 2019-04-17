﻿using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Microsoft.AspNetCore.SignalR.Protocol;
using Unofficial.SignalR.Protobuf.MessageSerializers.Base;
using Unofficial.SignalR.Protobuf.Util;

namespace Unofficial.SignalR.Protobuf.MessageSerializers
{
    internal class InvocationMessageSerializer : BaseMessageSerializer
    {
        public override ProtobufMessageType EnumType => ProtobufMessageType.Invocation;
        public override Type MessageType => typeof(InvocationMessage);

        protected override IEnumerable<object> CreateItems(HubMessage message)
        {
            var invocationMessage = (InvocationMessage) message;

            yield return new InvocationMessageProtobuf
            {
                InvocationId = invocationMessage.InvocationId,
                Target = invocationMessage.Target,
                Headers = { invocationMessage.Headers.Flatten() }
            };

            foreach (var argument in invocationMessage.Arguments)
            {
                yield return argument;
            }
        }

        protected override HubMessage CreateHubMessage(IReadOnlyList<object> items)
        {
            var protobuf = (InvocationMessageProtobuf) items.First();
            var argumentProtobufs = items.Skip(1).ToArray();

            return new InvocationMessage(
                protobuf.InvocationId, 
                protobuf.Target, 
                argumentProtobufs
            )
            {
                Headers = protobuf.Headers.Unflatten()
            };
        }
    }
}
