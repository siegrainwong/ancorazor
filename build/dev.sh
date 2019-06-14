docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d

docker run -it \
	-v ~/siegrain.blog:/app/ \
	-v ~/siegrain.blog/Blog.API/appsettings.Development.docker.json:/app/Blog.API/appsettings.Development.json \
	-p 8088:8088 \
	--link skywalking-oap \
	--link mssql \
	--name ancorazor_dev \
	--network build_default \
 	-w /app/Blog.API siegrainwong/aspnetcore-build:2.2-bionic dotnet watch run