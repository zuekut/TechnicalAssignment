# Set-up guide
## 1. Run the .NET Core application via terminal
1. Go to src/DataSetEnricher/
2. Build the project, by running the following command:
   ```console
   dotnet build
    ```

3. Run the project, by running the following command:
   ```console
   dotnet run
    ```
4. If all succeeded well you should see the following
   ```console
   [12:02:49 DBG] Hosting starting
   [12:02:49 INF] Now listening on: https://localhost:7103
   [12:02:49 INF] Now listening on: http://localhost:5295
   [12:02:49 INF] Application started. Press Ctrl+C to shut down.
   [12:02:49 INF] Hosting environment: Development

   ```
5. Go to https://localhost:7103/swagger/index.html
6. Once there, click on Try it out and choose file and add the input_dataset.csv [Swagger screen](Images/DataSetEnricherInput.png)
7. Click on execute
8. Once done processing you would be able to download the enriched csv file, by clicking the Download file button [Download file](Images/DataSetEnricherDownload.png)
9. That should be it, you are all set-up

## 2. Run the .NET Core application via docker-compose
1. Go to src/DataSetEnricher/
2. The ports have been preconfigured in the docker-compose file to be the following, but if those ports are occupied on your machine, feel free to change them to whichever ones you want:
   1. 8060 for HTTP
   2. 7060 for HTTPS
3. Run the following command in the terminal
   ```
   docker-compose up --build
   ```
4. If all succeeded well you should see the following
   ```console
    Creating datasetenricher_datasetenricher_1 ... done
    Attaching to datasetenricher_datasetenricher_1
    datasetenricher_1  | [09:52:30 DBG] Hosting starting
    datasetenricher_1  | [09:52:30 INF] Now listening on: https://[::]:443
    datasetenricher_1  | [09:52:30 INF] Now listening on: http://[::]:80
    datasetenricher_1  | [09:52:30 INF] Application started. Press Ctrl+C to shut down.
    datasetenricher_1  | [09:52:30 INF] Hosting environment: Development
    datasetenricher_1  | [09:52:30 INF] Content root path: /app/
    datasetenricher_1  | [09:52:30 DBG] Hosting started

   ```
4. Go to https://localhost:7060/swagger/index.html
5. Once there, click on Try it out and choose file and add the input_dataset.csv [Swagger screen](Images/DataSetEnricherInput.png)
6. Click on execute
7. Once done processing you would be able to download the enriched csv file, by clicking the Download file button [Download file](Images/DataSetEnricherDownload.png)
8. That should be it, you are all set-up