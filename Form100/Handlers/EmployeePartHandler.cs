using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CSM.Form100.Handlers
{
    using Models;

    public class EmployeePartHandler : ContentHandler
    {
        public EmployeePartHandler(IRepository<EmployeePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}