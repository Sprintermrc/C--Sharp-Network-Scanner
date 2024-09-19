//import userRoutes from './routes/users.js'
//import bodyParser from 'body-parser'
const express = require('express');
const { createServer, request } = require('node:http');
const { join } = require('node:path');
const { Server } = require('socket.io');



// const { availableParallelism } = require('node:os');
// const cluster = require('node:cluster');
// const { createAdapter, setupPrimary } = require('@socket.io/cluster-adapter');


// if (cluster.isPrimary) {
//     const numCPUs = availableParallelism();
//     // create one worker per available core
//     for (let i = 0; i < numCPUs; i++) {
//       cluster.fork({
//         PORT: 3300 + i
//       });
//     }
    
//     // set up the adapter on the primary thread
//     return setupPrimary();
// }


// app.use(bodyParser.json());
//app.use('/users', userRoutes);

const app = express();
const server = createServer(app);
const io = new Server(server, {
    connectionStateRecovery: {},
   // adapter: createAdapter()
  });


  
//REST STUFF

// app.use('/users', userRoutes);

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
            if (adress.family === 'IPv4' && ! adress.internal) {
                ipAdress = adress.address;
                break;
            }
        }
    }  
});
app.use(express.json());
app.post("/server-info", function (request, response) {
    const hostName = request.body;
    const interfaces = request.body;
    console.log(hostName, interfaces);
    if(!user) return response.sendStatus(400);
    const responseText = `Server name: ${hostName}  Server IP: ${interfaces}`;
    response.json({message: responseText}); // отправляем ответ
});
//END OF REST STUFF



// res.json({
//     hostname: hostname,
//     ipAdress: ipAdress
// });

io.on('connection', (socket) => {
    console.log('a user connected');
    io.emit('chat message', 'user connected')
    socket.on('disconnect', () => {
      console.log('user disconnected');
      io.emit('chat message', 'user disconnected')

    });
});

io.on('connection', (socket) => {
    socket.on('chat message', (msg) => {
        console.log('message: ' + msg);
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

io.on('connection', (socket) => {
    socket.on('request', (arg1, arg2, callback) => {
        console.log(arg1); // { foo: 'bar' }
        console.log(arg2); // 'baz'
        callback({
            status: 'ok'
        });
    });
});
  

server.listen(3300, () => {
  console.log('server running at http://localhost:3300');
});

// // each worker will listen on a distinct port
// const port = process.env.PORT;

// server.listen(port, () => {
// console.log(`server running at http://localhost:${port}`);
// });

'use strict';

const { networkInterfaces } = require('os');
const { hostname } = require('node:os');

const nets = networkInterfaces();
const results = Object.create(null); // Or just '{}', an empty object


io.on('connection', (socket) => {
    for (const name of Object.keys(nets)) {
    for (const net of nets[name]) {
        // Skip over non-IPv4 and internal (i.e. 127.0.0.1) addresses
        // 'IPv4' is in Node <= 17, from 18 it's a number 4 or 6
        const familyV4Value = typeof net.family === 'string' ? 'IPv4' : 4
        if (net.family === familyV4Value && !net.internal) {
            if (!results[name]) {
                results[name] = [];
            }
            results[name].push(net.address);
            io.emit('chat message', results[name]);
        }
    }
}
});