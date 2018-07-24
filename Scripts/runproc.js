var child = require('child_process');

urls = ["http://localhost:40001","http://localhost:40002","http://localhost:40003",
    "http://localhost:40004","http://localhost:40005","http://localhost:40006",
    "http://localhost:40007","http://localhost:40008","http://localhost:40009","http://localhost:40010"]


setTimeout(myfunc,1000);

var runCount = 0;
var totalRunCount = 0;

function myfunc(){
    runCount = 10;

    for(var i = 0;i< 10 ;i++){
        var du = child.spawn('node', ['deploy.js',urls[i]]);
        du.stderr.on('data', function (data) {
            //console.log('stderr: ' + data);
        });
        du.on('exit', function (code) {

            runCount --;
            totalRunCount ++;
            if(runCount == 0){
                setTimeout(myfunc,1000);
            }

            console.log('totalRunCount: ' + totalRunCount);
        });

        setTimeout(ProcessRunTimeout,30000,du);
    }
}

function ProcessRunTimeout(du){
    du.kill();
}






