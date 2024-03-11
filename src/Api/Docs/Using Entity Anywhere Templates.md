# Using Templates

In this file system of this solution (but not in the solution itself) there is a templates folder.

All the templates are in the templates folder


## Installing the templates
To install the templates, follow these steps
1. Open a powershell window
2. Navigate to the templates folder located in this solution
3. Run the Reinstall.ps1 script

## Templates

The following csproj templates exist
1. Entity Interface - shortcode: entityint
2. Entity - shortcode: entity
3. Entity Event - shortcode: entityev
4. Entity Service - shortcode: entityserv
5. Entity WebService - shortcode: entityweb
6. Custom Web Service - shortcode: customweb

The following item templates exist
1. Entity Interface - shortcode: entityii (i for item, i for interface)
2. Entity - shortcode: entityie (i for item, e for entity)

## Testing the templates

### powershell
To test the templates, follow these steps
1. Open a powershell window
2. Navigate to the templates folder located in this solution
3. Run the Reinstall.ps1 script

Or run the commands manually

### Visual Studio
To test a templates, follow these steps
1. Open your solution in Visual Studio
2. Make sure the templates are installed using the install steps above
3. Right-click on the approprate folder, such as Custom\Plugins\EntityInterfaces
4. Select Add | New Project
5. Find and highlight the Rhyous.EntityAnywhere.<template> type, such as Rhyous.EntityAnywhere.EntityInterface
6. Click Next
7. Enter the Entity name
8. Make sure to change the Location, for example, change the location to end with interfaces. Example: c:\dev\eaf\src\Interfaces
9. Answer the prompts on the next screen
10. Click Next to create the project