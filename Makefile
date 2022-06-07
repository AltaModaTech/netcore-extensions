all: test 


test: build
	dotnet test ./Test.AMT.Extensions.Logging
	dotnet test ./Test.AMT.Extensions.System


# cover: build
# 	dotnet test /p:CollectCoverage=true /p:Include="[J3DI*]*" /p:Exclude="[Test.J3DI*]*"  ./Test.J3DI.Domain
# 	dotnet test /p:CollectCoverage=true /p:Include="[J3DI*]*" /p:Exclude="[Test.J3DI*]*"  ./Test.J3DI.Infrastructure.EntityfactoryFx
# 	dotnet test /p:CollectCoverage=true /p:Include="[J3DI*]*" /p:Exclude="[Test.J3DI*]*"  ./Test.J3DI.Infrastructure.RepositoryFx


build: 
	dotnet build


clean:
	dotnet clean


clean-all: clean
	@echo "### Remove all NuPkg files"
	find . -iname "*.nupkg" -exec rm {} \;