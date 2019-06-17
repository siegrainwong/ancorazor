docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d

docker run --rm -it `
	-v F:\Projects\ancorazor\:/app/ `
	-v F:\Projects\ancorazor\Ancorazor.API\appsettings.Development.docker.json:/app/Ancorazor.API/appsettings.Development.json `
	-p 8088:8088 `
	--link skywalking-oap `
	--link mssql `
	--name ancorazor_dev `
	--network build_default `
 	-w /app/Ancorazor.API siegrainwong/aspnetcore-build:2.2-bionic dotnet watch run