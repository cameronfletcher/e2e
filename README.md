e2e
===
A trivial end-to-end web solution.

### Building
You'll probably need Visual Studio 2017 installed and .NET 4.7. It should build within the IDE out of the box.
This has been tested with SQL Server 2016 LocalDB which is also required to run some of the tests.

The .NET Framework 4.7 Developer pack can be found [here](https://www.microsoft.com/en-us/download/details.aspx?id=55168).

### Running
The following URL namespace reservation has to be made if not running with elevated access rights:
```
netsh http add urlacl url=http://+:80/e2e/ user=everyone
```
To configure logging please change the following line in ```log4net.config``` to point to a location suitable for you:
```xml
<file value="S:\logs\e2e\Cars.log" />
```
You will need to change the connection string value in the ```app.config``` file to point to a database that exists. The contents of the database should be automatically created upon the first repository call.

To run, execute ```Cars.exe``` then navigate to http://localhost/e2e