using libABCD;
using libDataAccess;
using libDataAccess.Utils;
using LINQToTreeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static libDataAccess.PlotSpecifications;
using EventType = System.Tuple<libDataAccess.JetInfoExtra, libDataAccess.JetInfoExtra>;
using static ABCDExplorer.EventTypePlotters;
using LINQToTreeHelpers.FutureUtils;

namespace ABCDExplorer
{
    class Program
    {
        /// <summary>
        /// Specific options for ABCDExplorer.
        /// </summary>
        class Options : CommandLineUtils.CommonOptions
        {

        }

        /// <summary>
        /// Loop through a set of ABCD methods and try to dump
        /// some generic and dense info on what it is like.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Do something like the sum of logR and the NTrack variable Something dumb...
            var explorer = new ABCDCutExplorer<EventType>(JetEventTypeSumCalRPlot, JetEventTypeSum2JTrackPt);

            // Get the background along with the variables we are going to look at for each event.
            var backgrounds = CommandLineUtils.GetRequestedBackground()
                .AsEventStream();

            // Do a background
            using (var output = new FutureTFile("ABCDExplorer-SumCalR-Sum2JTrackPt.root"))
            {
                explorer.ProcessBackground(output.mkdir("JnZ"), backgrounds);

                // And a few signals
                var signalList = CommandLineUtils.GetRequestedSignalSourceList();
                foreach (var source in signalList)
                {
                    var asEvents = source.Item2
                        .AsEventStream();

                    explorer.ProcessSignal(output.mkdir(source.Item1), asEvents);
                }
            }
        }
    }
}
