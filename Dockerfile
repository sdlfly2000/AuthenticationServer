# sudo docker image build -t authservice:last ../

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY [".","."]
ENTRYPOINT ["dotnet","AuthService.dll","--urls","http://*:4202"]


