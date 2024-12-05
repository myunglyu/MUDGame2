// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var characterId = document.getElementById("characterId").value;
console.log("Character ID: " + characterId);
document.getElementById("selectCharacterBtn").addEventListener("click", function() {
    selectCharacterBtn.disabled = true;
    selectCharacterBtn.innerText = "Loading...";
    console.log("Character ID: " + characterId);
    document.getElementById("selectedCharacter").value = characterId;
});