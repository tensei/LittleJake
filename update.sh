git pull
sudo systemctl stop jakebot.service
dotnet build -c Release
sudo systemctl start jakebot.service