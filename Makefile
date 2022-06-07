all: test 


test: build
	dotnet test ./Test.AMT.Extensions.Logging
	dotnet test ./Test.AMT.Extensions.System


cover: build
	dotnet test /p:CollectCoverage=true ./Test.AMT.Extensions.Logging
	dotnet test /p:CollectCoverage=true ./Test.AMT.Extensions.System


build: 
	dotnet build


clean:
	dotnet clean


clean-all: clean
	@echo "### Remove all NuPkg files"
	find . -iname "*.nupkg" -exec rm {} \;