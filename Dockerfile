# sudo docker image build -t authservice:last ../

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY [".","."]
ENTRYPOINT ["dotnet","AuthService.dll","--urls","http://*:4200"]


