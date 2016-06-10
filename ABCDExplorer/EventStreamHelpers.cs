using libDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQToTreeHelpers;
using LINQToTTreeLib;
using System.Threading.Tasks;
using EventType = System.Tuple<libDataAccess.JetInfoExtra, libDataAccess.JetInfoExtra>;

namespace ABCDExplorer
{
    /// <summary>
    /// Helper methods for dealing with events of various sorts
    /// </summary>
    static class EventStreamHelpers
    {
        /// <summary>
        /// Given a stream of events, take the two highest pT jets and return the jet info for them.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<EventType> AsEventStream(this IQueryable<Files.MetaData> source)
        {
            return from ev in source
                   let jets = ev.Data.Jets.AsQueryable()
                        .Where(j => JetInfoExtraHelpers.IsGoodJet.Invoke(j))
                        .Select(j => JetInfoExtraHelpers.CreateJetInfoExtra.Invoke(ev.Data, j))
                   where jets.Count() > 2
                   select new EventType (jets.First(), jets.Skip(1).First());
        }
    }
}
