# Use the official ASP.NET Framework runtime image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

# Set the working directory
WORKDIR /inetpub/wwwroot

# Copy the published application files
COPY ./publish .

# Expose port 80
EXPOSE 80

# The base image already has IIS configured, so no additional setup needed
