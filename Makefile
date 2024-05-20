all: test 


test: build
	dotnet test ./Test.AMT.Extensions.Linq
	dotnet test ./Test.AMT.Extensions.Logging
	dotnet test ./Test.AMT.Extensions.System


cover: build
	dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover\" ./Test.AMT.Extensions.Linq
	dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover\" ./Test.AMT.Extensions.Logging
	dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover\" ./Test.AMT.Extensions.System


build: 
	dotnet build

report:
	dotnet reportgenerator  "-reports:**/coverage.net6.0.opencover.xml" "-targetdir:Coverage"


clean:
	dotnet clean


clean-all: clean
	@echo "### Remove all NuPkg files"
	find . -iname "*.nupkg" -exec rm {} \;
	@echo "### Remove code coverage report"
	rm -fr ./Coverage
	@echo "### Remove bin and obj dirs"
	find . -type d -name "bin" -exec rm -r {} \;
	find . -type d -name "obj" -exec rm -r {} \;
	