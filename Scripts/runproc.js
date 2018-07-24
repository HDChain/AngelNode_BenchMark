var child = require('child_process');

urls = ["http://localhost:10006","http://localhost:10007","http://localhost:10008"]


setTimeout(myfunc,1000);

var runCount = 0;
var totalRunCount = 0;

function myfunc(){
    runCount = 3;

    for(var i = 0;i< 3 ;i++){
        var du = child.spawn('node', ['deploy.js',urls[i]]);
        du.stderr.on('data', function (data) {
            console.log('stderr: ' + data);
        });
        du.on('exit', function (code) {

            runCount --;
            totalRunCount ++;
            if(runCount == 0){
                setTimeout(myfunc,1000);
            }

            console.log('totalRunCount: ' + totalRunCount);
        });

        setTimeout(ProcessRunTimeout,10000,du);
    }
}

function ProcessRunTimeout(du){
    du.kill();
}






