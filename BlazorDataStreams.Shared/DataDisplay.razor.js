$(document).ready(function () {

    var timer;
    var totalTime = 0;

    function startStopTimer() {
        console.log('startStopTimer called.');
        var startStopButton = document.getElementById('startStopButton');
        var interval = document.getElementById('intervalSlider').value;
        var intervalInSeconds = interval / 10;
        var rapidUpdateButton = document.getElementById('rapidUpdateButton');

        if (startStopButton.innerText === 'Start Timer') {
            updateTextboxValue();
            timer = setInterval(updateTextboxValue, interval * 100);
            totalTime = 0;
            startStopButton.innerText = 'Stop Timer';

            rapidUpdateButton.disabled = true;
            
            enableTableButtons(false);

            // Stop the timer after 60 seconds.
            setTimeout(stopTimer, 60000);
        } else {
            stopTimer();
            rapidUpdateButton.disabled = false;
        
            enableTableButtons(true);
        }
    }

    function runRapidUpdate() {
        // Disable buttons, enable when complete.
        var rapidUpdateButton = document.getElementById('rapidUpdateButton');
        rapidUpdateButton.disabled = true;
        
        var startStopButton = document.getElementById('startStopButton');
        startStopButton.disabled = true;
        
        //enableTableButtons(false);

        var assignedID = document.getElementById('assignedID').value;
        var apiType = document.getElementById('apiType').value;
        var version = document.getElementById('version').value;

        var startTime = new Date().getTime();

        $.ajax({
            url: '/Home/CountUpdatesForThreeSeconds',
            type: 'GET',
            data: { id: assignedID, apiType: apiType, version: version },
            success: function (data) {
                // Update the textbox value with the received data
                $('#apiCallsCount').val(data);
                rapidUpdateButton.disabled = false;
                startStopButton.disabled = false;
                
                enableTableButtons(true);
            },
            error: function () {
                rapidUpdateButton.disabled = false;
                startStopButton.disabled = false;

                enableTableButtons(true);
            }
        });
    }

    function stopTimer() {
        clearInterval(timer);
        timer = undefined;
        document.getElementById('startStopButton').innerText = 'Start Timer';
    }

    function updateTextboxValue() {
        console.log('updatetextboxvalue called.');
        var assignedID = document.getElementById('assignedID').value;
        var apiType = document.getElementById('apiType').value;
        var version = document.getElementById('version').value;

        // my original code
        //$.ajax({
        //    url: '/Home/GetUpdatedValue',
        //    type: 'GET',
        //    data: { id: assignedID, apiType: apiType, version: version },
        //    success: function (data) {
        //        $('#updateTextbox').val(data);
        //    },
        //    error: function () {
        //    }
        //});

        // Chat replacement for above, before I moved away from JS
        //DotNet.invokeMethodAsync('BlazorDataStreams.Shared', 'GetUpdatedValue', id, apiType, version)
        //    .then(data => {
        //        $('#updateTextbox').val(data);
        //    })
        //    .catch(error => {
        //        console.error('Error calling C# method:', error);
        //    });

        totalTime += parseFloat(document.getElementById('intervalSlider').value) / 10;

        // Check if 60 seconds have elapsed, and stop the timer if true.
        if (totalTime >= 60) {
            stopTimer();
        }
    }

    // Update the displayed interval value when the slider changes.
    $(document).on('input', '#intervalSlider', function () {
        var newInterval = (this.value / 10);
        $('#intervalValue').text(newInterval);

        // Update the timer interval if it's already started.
        if (timer) {
            clearInterval(timer);
            timer = setInterval(updateTextboxValue, this.value * 100);
        }
    });

    function enableTableButtons(enable) {
        var tableButtons = document.querySelectorAll('#streamTable .tableButton');

        // Enable or disable all buttons.
        tableButtons.forEach(function (button) {
            button.disabled = !enable;
        });
    }

    $(".displayButton").click(function () {
        var nameValue = $(this).closest("tr").find(".hiddenName").val();
        $.ajax({
            url: "/Home/Display",
            method: "GET",
            data: { name: nameValue }, 
            success: function (data) {
                // Update the content of the container with the loaded partial view.
                $("#partialContainer").html(data);
            },
            error: function () {
                console.error("Failed to load partial view.");
            }
        });
    });
});
