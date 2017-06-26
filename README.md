e2e
===
A trivial end-to-end web solution.

### Building
You'll probably need Visual Studio 2017 installed and .NET 4.7. It should build within the IDE out of the box.


### Running
This requires the following URL namespace reservation to have been made if not running with elevated access rights:
```
netsh http add urlacl url=http://+:80/e2e/ user=everyone
```
To run, simply execute ```Cars.exe```.