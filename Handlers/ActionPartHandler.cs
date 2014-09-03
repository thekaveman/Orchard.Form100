using CSM.Form100.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CSM.Form100.Handlers
{
    /// <summary>
    /// ContentHandlers can hook into events happening on ContentParts
    /// </summary>
    public class ActionPartHandler : ContentHandler
    {
        public ActionPartHandler(IRepository<ActionPartRecord> repository)
        {
            //adds a storage mechanism for ActionPartRecords into the content pipeline
            Filters.Add(StorageFilter.For(repository));
        }
    }
}