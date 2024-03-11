#!/bin/bash
sleep 20
/opt/mssql-tools/bin/sqlcmd -U sa -P somepw -Q "CREATE DATABASE EntityAnywhere;"
/opt/mssql-tools/bin/sqlcmd -U sa -P somepw -d EntityAnywhere -i /docker/eaf.sql;
/opt/mssql-tools/bin/sqlcmd -U sa -P somepw -Q "CREATE DATABASE inData;"
/opt/mssql-tools/bin/sqlcmd -U sa -P somepw -d inData -i /docker/inData.sql;