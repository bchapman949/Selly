// Almost all of this file comes from the TraceEvent samples
// Nuget Package https://www.nuget.org/packages/Microsoft.Diagnostics.Tracing.TraceEvent.Samples/
// It has been modified to use the WFP provider and dropped packet keyword, pop a toast when restarting
// a tracing session and to invoke a callback located in a separate file.

using Microsoft.Diagnostics.Tracing.Session;
using Selly.Agent.Windows.Helpers;
using System;

namespace Selly.Agent.Windows
{
    class EtwHelper
    {
        TraceEventSession session;

        /// <summary>
        /// This is a demo of using TraceEvent to activate a 'real time' provider that is listening to 
        /// the MyEventSource above.   Normally this event source would be in a different process,  but 
        /// it also works if this process generate the events and I do that here for simplicity.  
        /// </summary>
        public int Run()
        {
            // Today you have to be Admin to turn on ETW events (anyone can write ETW events).   
            if (!(TraceEventSession.IsElevated() ?? false))
            {
                ExceptionHelper.WriteFile("EtwErrors", "Not running as admin");
                return -1;
            }

            // To listen to ETW events you need a session, which allows you to control which events will be produced
            // Note that it is the session and not the source that buffers events, and by default sessions will buffer
            // 64MB of events before dropping events.  Thus even if you don't immediately connect up the source and
            // read the events you should not lose them. 
            //
            // As mentioned below, sessions can outlive the process that created them.  Thus you may need a way of 
            // naming the session so that you can 'reconnect' to it from another process.   This is what the name
            // is for.  It can be anything, but it should be descriptive and unique.   If you expect multiple versions
            // of your program to run simultaneously, you need to generate unique names (e.g. add a process ID suffix) 
            // however this is dangerous because you can leave data collection on if the program ends unexpectedly.  
            var sessionName = "SellyServiceEtwMonitor";
            using (session = new TraceEventSession(sessionName))
            {
                /* BY DEFAULT ETW SESSIONS SURVIVE THE DEATH OF THE PROESS THAT CREATES THEM! */
                // Unlike most other resources on the system, ETW session live beyond the lifetime of the 
                // process that created them.   This is very useful in some scenarios, but also creates the 
                // very real possibility of leaving 'orphan' sessions running.  
                //
                // To help avoid this by default TraceEventSession sets 'StopOnDispose' so that it will stop
                // the ETW session if the TraceEventSession dies.   Thus executions that 'clean up' the TraceEventSession
                // will clean up the ETW session.   This covers many cases (including throwing exceptions)
                //  
                // However if the process is killed manually (including control C) this cleanup will not happen.  
                // Thus best practices include
                //
                //     * Add a Control C handler that calls session.Dispose() so it gets cleaned up in this common case
                //     * use the same session name (say your program name) run-to-run so you don't create many orphans. 
                //
                // By default TraceEventSessions are in 'create' mode where it assumes you want to create a new session.
                // In this mode if a session already exists, it is stopped and the new one is created.   
                // 
                // Here we install the Control C handler.   It is OK if Dispose is called more than once.  
                Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) { session.Dispose(); };

                /*****************************************************************************************************/
                // Hook up events.   To so this first we need a 'Parser. which knows how to part the events of a particular Event Provider.
                // In this case we get a DynamicTraceEventSource, which knows how to parse any EventSource provider.    This parser
                // is so common, that TraceEventSource as a shortcut property called 'Dynamic' that fetches this parsers.  

                // For debugging, and demo purposes, hook up a callback for every event that 'Dynamic' knows about (this is not EVERY
                // event only those know about by RegisteredTraceEventParser).   However the 'UnhandledEvents' handler below will catch
                // the other ones.
                session.Source.Dynamic.All += EtwCallback.EventOccured;

                // At this point we have created a TraceEventSession, hooked it up to a TraceEventSource, and hooked the
                // TraceEventSource to a TraceEventParser (you can do several of these), and then hooked up callbacks
                // up to the TraceEventParser (again you can have several).  However we have NOT actually told any
                // provider (EventSources) to actually send any events to our TraceEventSession.  
                // We do that now.  

                // Enable my provider, you can call many of these on the same session to get events from other providers.  
                // Because this EventSource did not define any keywords, I can only turn on all events or none.  
                var restarted = session.EnableProvider("Microsoft-Windows-WFP", matchAnyKeywords: 0x0000010000000000);
                
                // Generally you don't bother with this warning, but for the demo we do. 
                if (restarted)
                {
                    ToastHelper.PopToast("Restarting ETW session");
                }

                // go into a loop processing events can calling the callbacks.  Because this is live data (not from a file)
                // processing never completes by itself, but only because someone called 'source.Dispose()'.  
                session.Source.Process();
            }
            return 0;
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
