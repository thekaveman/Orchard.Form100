using System;
using Orchard.Data.Migration;
using CSM.Form100.Models;

namespace CSM.Form100
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
			// Creating table JobClassRecord
			SchemaBuilder.CreateTable(
                typeof(JobClassRecord).Name,
                table => table
				    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<string>("Title", col => col.WithLength(1024))
				    .Column<int>("Step", col => col.NotNull())
				    .Column<decimal>("PayRate", col => col.NotNull())
				    .Column<int>("HoursPerWeek", col => col.NotNull())
			);

			// Creating table ApprovalRecord
			SchemaBuilder.CreateTable(
                typeof(ApprovalRecord).Name,
                table => table
                    .Column<int>("Id", col => col.PrimaryKey().Identity())
				    .Column<bool>("IsApproved", col => col.NotNull())
				    .Column<DateTime>("DateOfApproval", col => col.Nullable())
				    .Column<string>("Approver", col => col.Nullable())
			);

            return 1;
        }
    }
}