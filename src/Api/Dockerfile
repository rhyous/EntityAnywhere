FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
# COPY --from=build-env /App/out .
# COPY --from=build-env /App/Plugins ./Plugins
# COPY --from=build-env /App/Certificate ./Certificate
# COPY ./wait.sh ./wait.sh
COPY --from=build-env /App/out .
COPY --from=build-env /App/. .

ENV ASPNETCORE_ENVIRONMENT=Release
ENV EAF_TokenTimeToLive=86400
ENV EAF_AutomaticMigrationDataLossAllowed=true
ENV EAF_EntityAdminToken=ba258083519e2cb36d59b6105531cef2529a83e87826f3448e1f06e9b16ca400
ENV EAF_InternalOrganizationId=1
ENV EAF_SSO_TokenSize=100
ENV EAF_SSO_TokenTimeToLive=86400
ENV EAF_LogRequestMessage=true
ENV EAF_LogRequestUrls=OwnershipService/Create
ENV EAF_LogResponseMessage=true
ENV EAF_LogResponseUrls=OwnershipService/Create
ENV EAF_ForceExternalUsersToAuthenticateExternally=false
ENV EAF_MaxFailedAttempts=10
ENV EAF_UseEntityFrameworkDatabaseManagementForAgents=false
ENV EAF_UseEntityFrameworkDatabaseManagementForSkills=false
ENV EAF_PluginDirectory=/App/Plugins
ENV EAF_PluginSharedBinPaths=/App/Plugins/SharedBin
ENV EAF_JWTPublicKey=/App/Certificate/publicKey.pem
ENV EAF_JWTPrivateKey=/App/Certificate/privateKey.pem
ENV EAF_EntityWebHost=http://localhost:80
ENV EAF_EntitySubpath=Api

EXPOSE 5000:5000

ENTRYPOINT ["bash", "wait.sh"]