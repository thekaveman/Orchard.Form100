using System;
using CSM.Form100.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CSM.Form100
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            // creating tables for part record classes

            SchemaBuilder.CreateTable(
                typeof(ActionPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<DateTime>("EffectiveDate", col => col.NotNull())
                    .Column<ActionCategory>("Category", col => col.NotNull())
                    .Column<string>("Type", col => col.NotNull())
                    .Column<string>("Detail")
            );

            SchemaBuilder.CreateTable(
                typeof(EmployeePartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<int>("EmployeeId", col => col.NotNull())
                    .Column<string>("FirstName", col => col.NotNull())
                    .Column<string>("LastName", col => col.NotNull())
                    .Column<int>("CurrentJobStep_Id", col => col.Nullable())
                    .Column<int>("PriorJobStep_Id", col => col.Nullable())                    
            );

            SchemaBuilder.CreateTable(
                typeof(ReviewPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("ApprovalChainIds", col => col.Unlimited())
                    .Column<WorkflowStatus>("Status", col => col.NotNull())
            );

			// creating tables for non-part record classes
            
			SchemaBuilder.CreateTable(
                typeof(JobStepRecord).Name,
                table => table
				    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<string>("Title", col => col.NotNull())
                    .Column<string>("DepartmentName", col => col.NotNull())
                    .Column<string>("DivisionName", col => col.NotNull())
                    .Column<int>("DivisionNumber", col => col.NotNull())
                    .Column<int>("StepNumber", col => col.NotNull())
                    .Column<int>("HoursPerWeek", col => col.NotNull())
                    .Column<decimal>("HourlyRate", col => col.NotNull().WithPrecision((byte)10).WithScale((byte)2))
			);

			SchemaBuilder.CreateTable(
                typeof(ReviewDecisionRecord).Name,
                table => table
                    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<bool>("IsApproved", col => col.Nullable())
                    .Column<DateTime>("ReviewDate", col => col.Nullable())
                    .Column<string>("ReviewerName", col => col.NotNull())
			);

            // defining the content parts that make up a Form 100

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ActionPart).Name,
                part => part.WithDescription("Encapsulates data about a Form 100 action (e.g. Appointment, Personnel Change, Separation/End Appointment).")
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(EmployeePart).Name,
                part => part.WithDescription("Encapsulates employee information for a Form 100.")
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ReviewPart).Name,
                part => part.WithDescription("Encapsulates information about the review process of a Form 100.")
            );

            // defining the Form 100 content type

            ContentDefinitionManager.AlterTypeDefinition(
                "Form100",
                type => type
                    .DisplayedAs("Form 100")
                    .Draftable(true)
                    .Creatable(true)
                    .WithPart("CommonPart", p => p
                        .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                        .WithSetting("DateEditorSettings.ShowDateEditor", "false"))
                    .WithPart("IdentityPart")
                    .WithPart("TitlePart")
                    .WithPart(typeof(ActionPart).Name)
                    .WithPart(typeof(EmployeePart).Name)
                    .WithPart(typeof(ReviewPart).Name)
            );

            return 1;
        }
    }
}