var cloakSpan = document.getElementById("cloakCounter");
var wandSpan = document.getElementById("wandCounter");
var stoneSpan = document.getElementById("stoneCounter");


//create connection
var connectionDeathlyHallows = new signalR.HubConnectionBuilder().withUrl("/hubs/deathlyhallows").build();

//connect to methods that hub invokes aka receive notifications from hub
connectionDeathlyHallows.on("updateDeathlyHallowsCount", (cloak,stone,wand) => {
    cloakSpan.innerText = cloak.toString();
    stoneSpan.innerText = stone.toString();
    wandSpan.innerText = wand.toString();
})

//invoke hub methods aka send notification to hub

//start connection
function fulfilled() {
    connectionDeathlyHallows.invoke("GetRaceStatus").then((raceCounter) => {
        cloakSpan.innerText = raceCounter.cloak.toString();
        stoneSpan.innerText = raceCounter.stone.toString();
        wandSpan.innerText = raceCounter.wand.toString();
    })
    //do something on start
    console.log("Connection to UserHub Successful")
}

function rejected() {
    //rejected logs
}

connectionDeathlyHallows.start().then(fulfilled, rejected);