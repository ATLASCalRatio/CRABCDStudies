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
            MakePlotterSpec<double>(50, -6.0, 8.0, j => j, "SumCalR{0}", "Sum of the Log Ratios for {0} Events; SumLogR");
        public static IPlotSpec<EventType> JetEventTypeSumCalRPlot;

        public static IPlotSpec<double> JetRawSum2JTrackPt =
            MakePlotterSpec<double>(50, 0.0, 10.0, j => j, "Sum2JTrackPt{0}", "Sum of track pT close to jet axis for both {0} jets; Sum track pT [GeV]");
        public static IPlotSpec<EventType> JetEventTypeSum2JTrackPt;

        public static IPlotSpec<double> JetRawDRToTrackSum =
            MakePlotterSpec<double>(50, 0.0, 1.0, j => j, "DRToTrackSum{0}", "Sum of DR between jet axis and first 2 GeV jet in to leading {0} jets.; Sum DR");
        public static IPlotSpec<EventType> JetEventDRToTrackSum;

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

            // Sum of the DR's for both trakcs.
            JetEventDRToTrackSum = JetRawDRToTrackSum
                .FromType<double, EventType>(info => CalcDR2GeVTrack.Invoke(info.Item1.AllTracks, info.Item1.Jet) + CalcDR2GeVTrack.Invoke(info.Item2.AllTracks, info.Item2.Jet));
        }
    }
}
