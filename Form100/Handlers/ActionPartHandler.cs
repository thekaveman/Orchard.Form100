using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CSM.Form100.Handlers
{
    using Models;

    public class ActionPartHandler : ContentHandler
    {
        public ActionPartHandler(IRepository<ActionPartRecord> repository)
        {
            // *conventional* way to configure persistence for *PartRecord backing classes
            Filters.Add(StorageFilter.For(repository));
        }
    }
}