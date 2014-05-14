using System.ComponentModel;

namespace CSM.Form100.Models
{
    public enum ActionCategory
    {
        [Description("Undefined")]
        Undefined,

        [Description("Appointment")]
        Appointment,

        [Description("Status Change")]
        Change,

        [Description("Separation/End Appointment")]
        Separation
    }
}