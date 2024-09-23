const bodyParser = require('body-parser');
const { json } = require('body-parser');
const express = require('express');
const { createServer, request } = require('http');
const { join } = require('path');
const { Server } = require('socket.io');


const app = express();
const server = createServer(app);
const io = new Server(server, {
    connectionStateRecovery: {},
});


app.use(express.json());

////////////////////////////////////////////////
//REST STUFF



app.get('/chat', (req, res) => {
    res.sendFile(join(__dirname, 'index.html'));
});


app.get('/home', (req, res) => {
    console.log('[GET ROUTE]');
    res.sendFile(join(__dirname, 'home.html'));
});


app.use('/home', function (request, response) {
    Response.redirect('chat');
});


app.get('/server-info', (req, res) => {
    const hostName = require('os').hostname();
    const interfaces = require('os').networkInterfaces();
    let ipAdress = '';
    for (const networkInterface of Object.values(interfaces)) {
        for (const adress of networkInterface) {
            if (adress.family === 'IPv4' && !adress.internal) {
                ipAdress = adress.address;
                break;
            }
        }
    }
    res.json({
        hostname: hostName,
        ipAdress: ipAdress
    });
});


app.post('/message', (req, res) => {
    const message = req.body.message;
    io.emit('chat message', `Собеседник: ${message}`);

    if (!message) {
        return res.status(400).json({
        error: 'Message not found'});
       }
    console.log('Recieved Message:', message);
    res.status(200).json({ message: 'Message recieved' });
});

//END OF REST STUFF
///////////////////////////////////////////////




io.on('connection', (socket) => {
    console.log('a user connected');
    io.emit('chat message', 'user connected');
    socket.on('disconnect', () => {
        console.log('user disconnected');
        io.emit('chat message', 'user disconnected');

    });
});

io.on('connection', (socket) => {
    socket.on('chat message', (msg) => {
        console.log('log: chat message: ' + msg);
    });
});

io.emit('hello', 'world');

io.on('connection', (socket) => {
    socket.on('hello', (arg) => {
        console.log(arg); // 'world'
    });
});

io.on('connection', (socket) => {
    socket.emit('hello', 'world');
});

io.on('connection', (socket) => {
    socket.broadcast.emit('hi');
});

io.on('connection', (socket) => {
    socket.on('chat message', (msg) => {
        io.emit('chat message', msg);
    });
});


//отправление запроса c сообщением на сервер
////////////////////////////


const options = {
    hostname: '',
    port: 3300,
    path: '/message',
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
        //'Content-Lenght': Buffer.byteLength(JSON.stringify(data))    
    }
}


http = require('http');

const message = io.on('connection', (socket) =>{
  socket.on('chat message', (msg) => {
        
    // const Message = json({
    //   message: msg
    // });


  //  try {
  //   request({ method:"POST", port:3300, body:Message});
  //  }
  //  catch{
  //   console.log('error sending');
  //  }


      const data = JSON.stringify({
        message: msg
      });
    const req = http.request(options, (res) => {

      console.log(`request status: ${res.statusCode}`);
      let response = '';
      let body = '';
      res.on('data', (chunk) => {
          response += chunk;
      });
      res.on('end', () => {
          console.log('server response: ', response);
      });

    });
    req.on('error', (error) => {
        console.log(error);
    });
    //console.log(`*-*-*-*-*-*-**-*-*-**-*-msg ${Message}`);
    req.write(data);
    req.end();
  });
});





///////////////////////////




// io.on('connection', (socket) => {
//     socket.on('request', (arg1, arg2, callback) => {
//         console.log(arg1); // { foo: 'bar' }
//         console.log(arg2); // 'baz'
//         callback({
//             status: 'ok'
//         });
//     });
// });
// const hostname = '172.29.9.90';
// const port = 3300;
// const server = http.createServer((req, res) => {
//     res.statusCode = 200;
//     res.setHeader('Content-Type', 'text/plain');
//     res.end('dsgfds');
// });

server.listen(3400, () => {
    console.log('----------------2 COPY server running at http://localhost:3400');
});