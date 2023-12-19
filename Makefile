all: test 


test: build
	dotnet test ./Test.AMT.Extensions.Logging
	dotnet test ./Test.AMT.Extensions.System


cover: build
	dotnet test /p:CollectCoverage=true ./Test.AMT.Extensions.Logging
	dotnet test /p:CollectCoverage=true ./Test.AMT.Extensions.System


build: 
	dotnet build

report:
	# @echo "Profile is $(HOME)."
	# dotnet reportgenerator  "-reports:**/coverage.net6.0.info" "-targetdir:Coverage"
	reportgenerator "-reports:**/coverage.net6.0.info" "-targetdir:Coverage"
#	dotnet $(HOME)/.nuget/packages/reportgenerator/5.1.9/tools/net6.0/ReportGenerator.dll  "-reports:**/coverage.net6.0.info" "-targetdir:Coverage"


clean:
	dotnet clean


clean-all: clean
	@echo "### Remove all NuPkg files"
	find . -iname "*.nupkg" -exec rm {} \;