using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libDataAccess;
using LINQToTreeHelpers;
using static LINQToTreeHelpers.PlottingUtils;
using LINQToTreeHelpers.FutureUtils;

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
        /// Create an ABCD explorer along these two axes
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public ABCDCutExplorer(IPlotSpec<T> v1, IPlotSpec<T> v2)
        {
            this._v1 = v1;
            this._v2 = v2;
        }

        /// <summary>
        /// Generate plots for a background.
        /// </summary>
        /// <param name="backgrounds"></param>
        public void ProcessBackground(FutureTDirectory output, IQueryable<T> backgrounds)
        {
            GenericPlots(output, backgrounds);
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
                .Plot(_v1, "")
                .Save(output);
            source
                .Plot(_v2, "")
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
