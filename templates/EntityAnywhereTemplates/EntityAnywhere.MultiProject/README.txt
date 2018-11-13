This isn't working correctly.

The multiproject adds the projects with an extra directory

The problem:
	mysolution/mysolution.sln
	mysolution/mysolution
	mysolution/mysolution/Entities/Entities.Entity1/
	mysolution/mysolution/Entities/Entities.Entity1/Entities.Entity1.csproj
	...

The expected correct:
	mysolution/mysolution.sln
	mysolution/Entities/Entities.Entity1/
	mysolution/Entities/Entities.Entity1/Entities.Entity1.csproj
	...

We need to figure out who to make it not create an extra directory.