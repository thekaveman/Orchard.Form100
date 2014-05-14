﻿using Orchard.ContentManagement;

namespace CSM.Form100.Models
{
    public class EmployeePart : ContentPart<EmployeePartRecord>
    {
        public int? EmployeeId
        {
            get { return Retrieve(e => e.EmployeeId); }
            set { Store(e => e.EmployeeId, value); }
        }

        public string FirstName
        {
            get { return Retrieve(e => e.FirstName); }
            set { Store(e => e.FirstName, value); }
        }

        public string LastName
        {
            get { return Retrieve(e => e.LastName); }
            set { Store(e => e.LastName, value); }
        }

        public JobClassRecord PriorJobClass
        {
            get { return Retrieve(e => e.PriorJobClass); }
            set { Store(e => e.PriorJobClass, value); }
        }

        public JobClassRecord CurrentJobClass
        {
            get { return Retrieve(e => e.CurrentJobClass); }
            set { Store(e => e.CurrentJobClass, value); }
        }

        public string DepartmentName
        {
            get { return Retrieve(e => e.DepartmentName); }
            set { Store(e => e.DepartmentName, value); }
        }

        public string DivisionName
        {
            get { return Retrieve(e => e.DivisionName); }
            set { Store(e => e.DivisionName, value); }
        }

        public string DivisionNumber
        {
            get { return Retrieve(e => e.DivisionNumber); }
            set { Store(e => e.DivisionNumber, value); }
        }
    }
}