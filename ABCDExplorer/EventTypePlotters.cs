using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LINQToTreeHelpers.PlottingUtils;
using EventType = System.Tuple<libDataAccess.JetInfoExtra, libDataAccess.JetInfoExtra>;
using static libDataAccess.PlotSpecifications;
using LINQToTreeHelpers;
using LINQToTTreeLib;

namespace ABCDExplorer
{
    static class EventTypePlotters
    {
        /// <summary>
        /// Plot the sum of the CalR of two jets.
        /// </summary>
        public static IPlotSpec<double> JetRawSumCalRPlot =
            MakePlotterSpec<double>(50, -6.0, 8.0, j => j);
        public static IPlotSpec<EventType> JetEventTypeSumCalRPlot;

        public static IPlotSpec<double> JetRawSum2JTrackPt =
            MakePlotterSpec<double>(50, 0.0, 10.0, j => j);
        public static IPlotSpec<EventType> JetEventTypeSum2JTrackPt;

        /// <summary>
        /// Initialize everything we can do before.
        /// </summary>
        static EventTypePlotters()
        {
            // Sum logR
            JetEventTypeSumCalRPlot = JetRawSumCalRPlot
                .FromType<double, EventType>(info => NormalizeCalRatio.Invoke(info.Item1.Jet.logRatio) + NormalizeCalRatio.Invoke(info.Item2.Jet.logRatio));

            // Sum Track pT of the two
            JetEventTypeSum2JTrackPt = JetRawSum2JTrackPt
                .FromType<double, EventType>(info => info.Item1.AllTracks.Select(t => t.pT).Sum() + info.Item2.AllTracks.Select(t => t.pT).Sum());
        }
    }
}
