using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libDataAccess;
using LINQToTreeHelpers;
using static LINQToTreeHelpers.PlottingUtils;

namespace libABCD
{
    /// <summary>
    /// Explore a set of ABCD cuts along a plane
    /// </summary>
    public class ABCDCutExplorer<T>
    {
        private IPlotSpec<T> jetExtraCalRPlot;
        private IPlotSpec<T> jetExtraMaxPt;

        /// <summary>
        /// Create an ABCD explorer along these two axes
        /// </summary>
        /// <param name="jetExtraCalRPlot"></param>
        /// <param name="jetExtraMaxPt"></param>
        public ABCDCutExplorer(IPlotSpec<T> jetExtraCalRPlot, IPlotSpec<T> jetExtraMaxPt)
        {
            this.jetExtraCalRPlot = jetExtraCalRPlot;
            this.jetExtraMaxPt = jetExtraMaxPt;
        }

        /// <summary>
        /// Generate plots for a background.
        /// </summary>
        /// <param name="backgrounds"></param>
        public void ProcessBackground(string backgroundName, IQueryable<T> backgrounds)
        {
            throw new NotImplementedException();
        }
    }
}
