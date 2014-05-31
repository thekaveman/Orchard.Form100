using System;
using System.ComponentModel;

namespace CSM.Form100
{
    /// <summary>
    /// A marker for the type of Employee Action
    /// </summary>
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