# Form100

An Orchard (1.8.1) module to model the City of Santa Monica's Form 100 workflow.

## Initial Development Setup

There are a few steps required to setup an environment for development/testing of this module.

  1. Obtain a copy of the [Orchard 1.8.1 source](https://orchard.codeplex.com/releases), 
and extract it into a local working directory.
  
  2. From that local working directory, open a command prompt and change into the `Modules` directory

        cd Orchard.Source.1.8.1\src\Orchard.Web\Modules
  
  3. Inside `Modules`, clone the git repo down

        git clone git@github.com:thekaveman/Form100.git CSM.Form100  

  Note the name of the local repo is **CSM.Form100**. This is important, it *must* match the 
	name of the module given in the [Module manifest](Module.txt)

  4. Open the `Orchard.Source.1.8.1\src\Orchard.sln` file in VS2013, and 
"Add Existing Project..." `CSM.Form100.csproj` to the solution. (Build the solution to ensure this worked).

  5. Optional: Copy the custom setup recipe to the Orchard.Setup module to configure your Orchard instance
with the required and otherwise useful modules enabled.

  *assuming we're still at the root of the CSM.Form100 module*

        copy Recipes\DevSetup.recipe.xml ..\Orchard.Setup\Recipes\DevSetup.recipe.xml


After completing the above steps, you will have a local development environment for this module. 
Run the Orchard site and enable the `CSM.Form100` module as you would any other.
