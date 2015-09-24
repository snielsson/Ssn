SET "packagename=Ssn.Utils"
nuget pack Ssn.Utils.csproj -IncludeReferencedProjects -Prop Configuration=Release -sym
nuget push Ssn.Utils
pause