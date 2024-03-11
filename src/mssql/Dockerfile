FROM mcr.microsoft.com/mssql/server:2019-latest
WORKDIR /docker
COPY . .
ENV ACCEPT_EULA=Y   
ENV SA_PASSWORD=somepw
CMD /bin/bash /docker/Database.sh & /opt/mssql/bin/sqlservr