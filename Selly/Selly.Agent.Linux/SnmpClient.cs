// Based loosely on: https://github.com/lextm/sharpsnmplib/blob/master/Samples/CSharpCore/snmpd/Program.cs

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Selly.Agent.Linux
{
    public class SnmpClient
    {
        private SnmpEngine _engine;

        public SnmpClient()
        {
            var store = new ObjectStore();
            store.Add(new SysDescr());
            store.Add(new SysObjectId());
            store.Add(new SysUpTime());
            store.Add(new SysContact());
            store.Add(new SysName());
            store.Add(new SysLocation());
            store.Add(new SysServices());
            store.Add(new SysORLastChange());
            store.Add(new SysORTable());
            store.Add(new IfNumber());
            store.Add(new IfTable());

            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));

            var getv1 = new GetV1MessageHandler();
            var getv1Mapping = new HandlerMapping("v1", "GET", getv1);

            var getv23 = new GetMessageHandler();
            var getv23Mapping = new HandlerMapping("v2,v3", "GET", getv23);

            var setv1 = new SetV1MessageHandler();
            var setv1Mapping = new HandlerMapping("v1", "SET", setv1);

            var setv23 = new SetMessageHandler();
            var setv23Mapping = new HandlerMapping("v2,v3", "SET", setv23);

            var getnextv1 = new GetNextV1MessageHandler();
            var getnextv1Mapping = new HandlerMapping("v1", "GETNEXT", getnextv1);

            var getnextv23 = new GetNextMessageHandler();
            var getnextv23Mapping = new HandlerMapping("v2,v3", "GETNEXT", getnextv23);

            var getbulk = new GetBulkMessageHandler();
            var getbulkMapping = new HandlerMapping("v2,v3", "GETBULK", getbulk);

            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v2 = new Version2MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v3 = new Version3MembershipProvider();
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] { v1, v2, v3 });
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                getv1Mapping,
                getv23Mapping,
                setv1Mapping,
                setv23Mapping,
                getnextv1Mapping,
                getnextv23Mapping,
                getbulkMapping
            });

            var pipelineFactory = new SnmpApplicationFactory(store, membership, handlerFactory);
            _engine = new SnmpEngine(pipelineFactory, new Listener { Users = users }, new EngineGroup());
            _engine.Listener.MessageReceived += Listener_MessageReceived;
            
            _engine.ExceptionRaised += (sender, e) =>
            {
                Debugger.Break();
            };
        }

        private void Listener_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            
            Debugger.Break();
        }

        public void Start()
        {
            try
            {
                _engine.Listener.ClearBindings();
                if (Socket.OSSupportsIPv4)
                {
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, 161));
                }

                if (Socket.OSSupportsIPv6)
                {
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.IPv6Any, 161));
                }

                _engine.Start();

            }
            catch(Exception e)
            {
                ExceptionHelper.WriteFile(e.Message, "snmpstart");
            }
        }

        public void Close()
        {
            _engine.Stop();
            _engine.Dispose();
        }
    }
}
