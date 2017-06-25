e2e
===
An example end-to-end simple data entry system.

### Building
You'll need Visual Studio 2017 installed. It should build within the IDE out of the box.


### Running
This requires the following URL namespace reservation to have been made if not running with elevated access rights:
```netsh http add urlacl url=http://+:80/e2e/cars/ user=everyone```.
