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
using LINQToTTreeLib;
using MathNet.Numerics.LinearAlgebra;
using static LINQToTreeHelpers.PlottingUtils;
using System.Linq.Expressions;

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
            // Get the background along with the variables we are going to look at for each event.
            var backgrounds = CommandLineUtils.GetRequestedBackground()
                .AsEventStream();

            // Do a background
            using (var output = new FutureTFile("ABCDExplorer-SumCalR-Sum2JTrackPt.root"))
            {
                // Do something like the sum of logR and the NTrack variable Something dumb...
                var explorer = new ABCDCutExplorer<EventType>(JetEventTypeSumCalRPlot, JetEventTypeSum2JTrackPt);

                // Do the uncorrelated version of the exploration.
                var info = DoABCDExploration(explorer, backgrounds, output.mkdir("correlated"));

                // Next, get the uncorrelated version
                var unCorVars = UncorrelateVariables(info.CoVar, JetEventTypeSumCalRPlot, JetEventTypeSum2JTrackPt);
                var exploreUnCor = new ABCDCutExplorer<EventType>(unCorVars.Item1, unCorVars.Item2);
                DoABCDExploration(exploreUnCor, backgrounds, output.mkdir("uncorrelated"));
            }
        }

        /// <summary>
        /// Try to uncorrelate the variables
        /// </summary>
        /// <param name="coVar"></param>
        /// <param name="jetEventTypeSumCalRPlot"></param>
        /// <param name="jetEventTypeSum2JTrackPt"></param>
        /// <returns></returns>
        private static Tuple<IPlotSpec<EventType>, IPlotSpec<EventType>> UncorrelateVariables(Matrix<double> coVar, IPlotSpec<EventType> v1, IPlotSpec<EventType> v2)
        {
            // Calculate the coefficients, and then the expressions
            var c = coVar.Cholesky();
            var coeff = c.Factor.Inverse();

            Expression<Func<EventType, double>> v1UCcalc = et => coeff[0, 0] * v1.ValueExpressionX.Invoke(et) + coeff[0,1] * v2.ValueExpressionX.Invoke(et);
            Expression<Func<EventType, double>> v2UCcalc = et => coeff[1, 0] * v1.ValueExpressionX.Invoke(et) + coeff[1, 1] * v2.ValueExpressionX.Invoke(et);

            // Next, we have to build up new plotter guys!

            var plotter1 = MakePlotterSpec<EventType>(100, -10.0, 10.0, v1UCcalc, "UnCor1{0}", "Uncorrelated Variable 1");
            var plotter2 = MakePlotterSpec<EventType>(100, -10.0, 10.0, v2UCcalc, "UnCor2{0}", "Uncorrelated Variable 2");

            return Tuple.Create(plotter1, plotter2);
        }

        /// <summary>
        /// Look at a single set of stuff for the ABCD method
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="backgrounds"></param>
        /// <param name="output"></param>
        private static ABCDInfo DoABCDExploration(ABCDCutExplorer<EventType> explorer, IQueryable<EventType> backgrounds, FutureTDirectory output)
        {
            var info = explorer.ProcessBackground(output.mkdir("JnZ"), backgrounds);

            // And a few signals
            var signalList = CommandLineUtils.GetRequestedSignalSourceList();
            foreach (var source in signalList)
            {
                // Do everything
                var asEvents = source.Item2
                    .AsEventStream();
                explorer.ProcessSignal(output.mkdir(source.Item1), asEvents);

                // Now, look carefully at only "signal" jets
                var asCalSignalEvents = asEvents
                    .Where(t => SampleUtils.IsGoodSignalJet.Invoke(t.Item1.Jet) && SampleUtils.IsGoodSignalJet.Invoke(t.Item2.Jet));
                explorer.ProcessSignal(output.mkdir($"{source.Item1}-CalOnly"), asCalSignalEvents);
            }

            return info;
        }
    }
}
