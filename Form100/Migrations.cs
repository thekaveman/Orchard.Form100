using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace CSM.Form100
{
    using Models;

    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
			// creating tables for non-part record classes
            // (part record data is stored with the overall content record e.g. infoset/document storage)

			SchemaBuilder.CreateTable(
                typeof(JobStepRecord).Name,
                table => table
				    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<string>("Title")
                    .Column<string>("DepartmentName")
                    .Column<string>("DivisionName")
                    .Column<int>("DivisionNumber", col => col.NotNull())
                    .Column<int>("StepNumber", col => col.NotNull())
                    .Column<int>("HoursPerWeek", col => col.NotNull())
                    .Column<decimal>("HourlyPay", col => col.NotNull())                    
			);

			SchemaBuilder.CreateTable(
                typeof(ReviewDecisionRecord).Name,
                table => table
                    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<bool>("IsApproved", col => col.NotNull())
                    .Column<DateTime>("ReviewDate", col => col.Nullable())
                    .Column<string>("ReviewerName")
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