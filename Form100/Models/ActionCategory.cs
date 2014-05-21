using System;
using System.ComponentModel;

namespace CSM.Form100.Models
{
    public enum ActionCategory
    {
        [Description("Appointment")]
        Appointment,

        [Description("Status Change")]
        Change,

        [Description("Separation/End Appointment")]
        Separation
    }
}