
The RoutingEngine Soln is a C# project built using VS2015.

Contains the following projects:

1. RoutingApp(Console App). This project will generate the EXE for running the application.
   
2. RoutingLib(Library project). This project is used by the RoutingApp project

3. RoutingApp.Tests - a test project for the RoutingApp using MSTest Framework

4. RoutingLib.Tests - a test project for the RoutingLib using MSTest Framework


Instructions in building and running the app:

1. Open RoutingEngine.sln in VS2015
2. Build/Compile the solution in Release configuration.
3. Find the RoutingApp.EXE file generated from step 2 (~\RoutingEngine\RoutingApp\bin\Release\RoutingApp.exe)
4. Double-click on the EXE file and a console window should display the following:

***************************************************************
Enter 0 to reload graph data from file
Enter 1 to display current nodes
Enter 2 to calculate distance of nodes
Enter 3 to get # of trips specify # stop between two nodes
Enter 4 to get length of shortest route between two nodes
Enter 5 to get routes with specified distance
Enter 9 to clear screen
***************************************************************


5. By default, the application is pre-loaded with graph data from RoutingData.txt file 
   located in the application folder (~\RoutingEngine\RoutingApp\bin\Release\RoutingData.txt).

6. To reload graph data from another file, enter '0' when prompted with the screen
   in step 4. It will ask for path of file to be loaded. Please use follow format as the
   sample data file (RoutingData.txt).

7. Enter input data from console when prompted.