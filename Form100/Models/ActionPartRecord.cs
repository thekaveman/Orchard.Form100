using System;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CSM.Form100.Models
{
    public class ActionPartRecord : ContentPartRecord
    {
        public virtual DateTime? EffectiveDate { get; set; }

        public virtual ActionType Type { get; set; }

        [StringLengthMax]
        public virtual string Label { get; set; }

        [StringLengthMax]
        public virtual string Detail { get; set; }

        public ActionPartRecord()
        {
            Type = ActionType.Undefined;
        }
    }
}