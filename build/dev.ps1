docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d

# 如果这句命令挂了可能是因为mssql还没有跑起来
docker exec -i mssql /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Fuckme123!@#" -Q "ALTER LOGIN SA WITH PASSWORD='Fuckme123!@#'"