set -ex
cd #{ScriptDirectory}#

docker-compose up -d

# reset sa password using a query
docker exec -it mssql /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U SA -P '#{MSSQL_SA_PASSWORD}#' \
   -Q 'ALTER LOGIN SA WITH PASSWORD="#{MSSQL_SA_PASSWORD}#"'

# test mssql connection
docker exec -it mssql "bash"
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P '#{MSSQL_SA_PASSWORD}#'
SELECT Name from sys.Databases
go
quit