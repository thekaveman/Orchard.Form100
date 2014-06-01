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

            //SchemaBuilder.CreateTable(
            //    typeof(ActionPartRecord).Name,
            //    table => table
            //        .ContentPartRecord()
            //        .Column<DateTime>("EffectiveDate", col => col.Nullable())
            //        .Column<string>("Category")
            //        .Column<string>("Type")
            //        .Column<string>("Detail")
            //);

            //SchemaBuilder.CreateTable(
            //    typeof(EmployeePartRecord).Name,
            //    table => table
            //        .ContentPartRecord()
            //        .Column<int>("EmployeeId")
            //        .Column<string>("FirstName")
            //        .Column<string>("LastName")
            //        .Column<int>("CurrentJobStep_Id", col => col.Nullable())
            //        .Column<int>("PriorJobStep_Id", col => col.Nullable())
            //);

            SchemaBuilder.CreateTable(
                typeof(ReviewPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("PendingReviewsIds")
                    .Column<string>("ReviewHistoryIds")
                    //.Column<string>("Status")
            );

			// creating tables for non-part record classes
            
			SchemaBuilder.CreateTable(
                typeof(JobStepRecord).Name,
                table => table
				    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<string>("EmployeePartIdentifier")
                    .Column<string>("Title")
                    .Column<string>("DepartmentName")
                    .Column<string>("DivisionName")
                    .Column<int>("DivisionNumber")
                    .Column<int>("StepNumber")
                    .Column<int>("HoursPerWeek")
                    .Column<decimal>("HourlyRate", col => col.WithPrecision((byte)10).WithScale((byte)2))
			);

			SchemaBuilder.CreateTable(
                typeof(ReviewStepRecord).Name,
                table => table
                    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<string>("ReviewPartIdentifier", col => col.Nullable())
                    .Column<string>("ApprovingStatus")
                    .Column<string>("RejectingStatus")
                    .Column<string>("ReviewDecision")
                    .Column<DateTime>("ReviewDate", col => col.Nullable())
                    .Column<string>("ReviewerName")
                    .Column<string>("ReviewerEmail")
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