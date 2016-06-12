using LINQToTreeHelpers;
using LINQToTreeHelpers.FutureUtils;
using LINQToTTreeLib;
using System.Linq;
using static LINQToTreeHelpers.PlottingUtils;

namespace libABCD
{
    /// <summary>
    /// Explore a set of ABCD cuts along a plane
    /// </summary>
    public class ABCDCutExplorer<T>
    {
        /// <summary>
        /// Variable 1 we are plotting against
        /// </summary>
        private IPlotSpec<T> _v1;

        /// <summary>
        /// Variable 2 we are plotting against
        /// </summary>
        private IPlotSpec<T> _v2;

        /// <summary>
        /// The 2D Plot.
        /// </summary>
        private IPlotSpec<T> _2DPlot;

        /// <summary>
        /// Create an ABCD explorer along these two axes
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public ABCDCutExplorer(IPlotSpec<T> v1, IPlotSpec<T> v2)
        {
            _v1 = v1;
            _v2 = v2;

            _2DPlot = _v1.CombinePlotAxes(_v2);
        }

        /// <summary>
        /// Generate plots for a background.
        /// </summary>
        /// <param name="backgrounds"></param>
        public void ProcessBackground(FutureTDirectory output, IQueryable<T> backgrounds)
        {
            GenericPlots(output, backgrounds);

            // We have to calculate the correlation for this.
            var count = backgrounds.FutureCount();
            var s1 = backgrounds.Select(v => _v1.ValueExpressionX.Invoke(v)).FutureAggregate(0.0, (ac, v) => ac + v);
            var s2 = backgrounds.Select(v => _v2.ValueExpressionX.Invoke(v)).FutureAggregate(0.0, (ac, v) => ac + v);

            var av1 = s1.Value / count.Value;
            var av2 = s2.Value / count.Value;

            var sdS1 = backgrounds.Select(v => _v1.ValueExpressionX.Invoke(v) - av1).Select(v => v * v).FutureAggregate(0.0, (ac, v) => ac + v);
            var sdS2 = backgrounds.Select(v => _v2.ValueExpressionX.Invoke(v) - av2).Select(v => v * v).FutureAggregate(0.0, (ac, v) => ac + v);
            var sdS12 = backgrounds.Select(v => (_v1.ValueExpressionX.Invoke(v) - av1)*(_v2.ValueExpressionX.Invoke(v) - av2)).FutureAggregate(0.0, (ac, v) => ac + v);
        }

        /// <summary>
        /// Make the generic plots of everything
        /// </summary>
        /// <param name="output"></param>
        /// <param name="backgrounds"></param>
        private void GenericPlots(FutureTDirectory output, IQueryable<T> source)
        {
            // Do the 1D plots of everything
            source
                .FuturePlot(_v1, "")
                .Save(output);
            source
                .FuturePlot(_v2, "")
                .Save(output);
            source
                .FuturePlot(_2DPlot, "")
                .Save(output);
        }

        /// <summary>
        /// Make some signal like plots
        /// </summary>
        /// <param name="output"></param>
        /// <param name="signal"></param>
        public void ProcessSignal(FutureTDirectory output, IQueryable<T> signal)
        {
            GenericPlots(output, signal);
        }
    }
}
