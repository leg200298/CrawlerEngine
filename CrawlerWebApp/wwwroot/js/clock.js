var connection = new signalR.HubConnectionBuilder()
    .withUrl("https://crawlerwebtest.azurewebsites.net/hubs/clock")
    .withAutomaticReconnect()
    .build();

connection.start().catch(err => console.log(err));

connection.on("ShowTime", (dateTime) => {
    document.getElementById("i").textContent = dateTime;
});

connection.onreconnecting(error => {
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
    document.getElementById("i").textContent = `Connection lost due to error "${error}". Reconnecting.`;    
});