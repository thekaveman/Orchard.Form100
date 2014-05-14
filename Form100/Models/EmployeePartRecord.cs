﻿using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement.Records;

namespace CSM.Form100.Models
{
    public class EmployeePartRecord : ContentPartRecord
    {
        public virtual int? EmployeeId { get; set; }

        [StringLength(128)]
        public virtual string FirstName { get; set; }

        [StringLength(128)]
        public virtual string LastName { get; set; }

        public virtual JobClassRecord PriorJobClass { get; set; }

        public virtual JobClassRecord CurrentJobClass { get; set; }

        [StringLength(128)]
        public virtual string DepartmentName { get; set; }

        [StringLength(128)]
        public virtual string DivisionName { get; set; }

        public virtual string DivisionNumber { get; set; }
    }
}